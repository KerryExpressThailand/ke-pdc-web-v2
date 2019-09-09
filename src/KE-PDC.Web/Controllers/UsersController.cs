using KE_PDC.Models;
using KE_PDC.Services;
using KE_PDC.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KE_PDC.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<UsersController> _logger;
        private KE_POSContext DB;
        private readonly IEDIServicesAD _api; 

        public UsersController(KE_POSContext context, ILogger<UsersController> logger, IHostingEnvironment hostingEnvironment, IEDIServicesAD api)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            DB = context;
            _api = api;
        }

        // GET: /<controller>/
        //[AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        // GET: /<controller>/Login
        [HttpGet] 
        [AllowAnonymous]
        [ImportModelState]
        public IActionResult Login(string returnUrl, string LandingPage, string status)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard"); 
            }
            if (LandingPage != null)
            {
                if (status == "405")
                {
                    ModelState.AddModelError("failed", "your password is expire, time to change it. Please click ");
                    ViewBag.page = LandingPage;
                    ViewBag.word = "Change Password";
                    ViewBag.word_01 = " to setup new password and try to login again. Thank you";
                }
                else if (status == "406")
                {
                    ModelState.AddModelError("failed", "Your account is locked. Please click ");
                    ViewBag.page = LandingPage;
                    ViewBag.word = "Reset your account";
                    ViewBag.word_01 = "to reset and try to login again. Thank you";
                }
               
                return View();
            }
            returnUrl = null;
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /<controller>/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ExportModelState]     
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Login", "Users");
            }

            try
            {
                
                if (model.option == "ssgn")
                {
                    //var ad = _api.LoginAD(model.Username, model.Password);
                    //SSOService EDI = new SSOService();
                    //EDI = JsonConvert.DeserializeObject<SSOService>(ad);

                    //if (EDI.ResultCode == "406" || EDI.ResultCode == "405")
                    //{
                    //    string LandingPage = EDI.LandingPage.ToString();


                    //    //return RedirectToAction("Login", "Users");
                    //    return RedirectToAction("Login", new RouteValueDictionary(
                    //      new { controller = "Users", action = "Login", LandingPage = LandingPage, status = EDI.ResultCode }));
                    //    //return Redirect(EDI.LandingPage.ToString());
                    //}
                    //if (EDI.ResultCode != "200")
                    //{
                    //    ModelState.AddModelError("failed", "Plase login by Single Sign On Only.");
                    //    return RedirectToAction("Login", "Users");
                    //}

                    UserMapRole user = new UserMapRole();

                    UserRole users = await DB.UserRole.FromSql("EXEC sp_PDC_Login_Get '" + model.Username + "'").Where(u => u.status != false).FirstAsync();
                    List<UserMenuRole> UserMenu = await DB.UserMenuRole.FromSql("EXEC sp_PDC_Get_Menu '" + model.Username + "'").ToListAsync();

                    user.UserMenu = UserMenu;
                    user.Username = users.user_id;
                    user.Name = users.user_name;
                    user.RoleId = users.user_role;
                    user.email = users.user_role;
                    user.LastLogin = users.last_login;

                    //User user = await DB.User
                    //    .Include(u => u.UserGroup)
                    //    .Include(u => u.UserMenu)
                    //    .Include(u => u.UserProfile)
                    //    .Where(u => u.email.Equals(EDI.Email.ToLower()))
                    //    .FirstAsync(); 
                    var AuthUser = DB.AuthUser.SingleOrDefault(c => c.user_id == model.Username);

                    AuthUser.last_login = DateTime.Now;
                    DB.SaveChanges();
                    

                    _logger.LogInformation($"Logged in {model.Username}.");

                    List<Claim> claims = new List<Claim> {
                        new Claim(ClaimTypes.NameIdentifier, user.Username, ClaimValueTypes.String),
                        new Claim(ClaimTypes.Name, user.Name, ClaimValueTypes.String),
                        new Claim(ClaimTypes.Role, user.RoleId.ToString(), ClaimValueTypes.String),
                        new Claim("User", JsonConvert.SerializeObject(user), new User().GetType().ToString()),
                        };

                    List<Menu> menus;

                    Stream fs = System.IO.File.OpenRead($@"{_hostingEnvironment.ContentRootPath}\menus.json");
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        menus = JsonConvert.DeserializeObject<List<Menu>>(await reader.ReadToEndAsync());
                    }
                    fs.Dispose();

                    AddRole(user, menus, claims);

                    ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "ApplicationCookie");
                    ClaimsPrincipal userPrincipal = new ClaimsPrincipal(userIdentity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

                    //await DB.SaveChangesAsync();

                    DB.Dispose();
                }
                else
                {
                    UserMapRole user = new UserMapRole();
                    UserRole users = await DB.UserRole.FromSql("EXEC sp_PDC_Login_Get '" + model.Username + "'").Where(u => u.status != false).FirstAsync();
                    List<UserMenuRole> UserMenu = await DB.UserMenuRole.FromSql("EXEC sp_PDC_Get_Menu '" + model.Username + "'").ToListAsync();

                    user.UserMenu = UserMenu;
                    user.Username = users.user_id;
                    user.Name = users.user_name;
                    user.RoleId = users.user_role;
                    user.email = users.user_role;
                    user.LastLogin = users.last_login;

                    bool is_auth = DB.AuthUser.Any(c => c.email == user.email);
                    if (is_auth)
                    {
                        ModelState.AddModelError("failed", "Plase login by Single Sign On Only.");
                        return RedirectToAction("Login", "Users");
                    }

                    user.LastLogin = DateTime.Now;
                    DB.SaveChanges();

                    //var users = DB.User.FromSql("SELECT userid AS Username, username AS name, role_id AS RoleId, profile_id AS ProfileId FROM tb_master_user AS u WITH(NOLOCK) WHERE userid = '" + model.Username + "' AND pwd = '" + model.Password + "'");

                    _logger.LogInformation($"Logged in {model.Username}.");
                    
                   
                    user.Password = string.Empty;

                    List<Claim> claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, user.Username, ClaimValueTypes.String),
                    new Claim(ClaimTypes.Name, user.Name, ClaimValueTypes.String),
                    new Claim(ClaimTypes.Role, user.RoleId.ToString(), ClaimValueTypes.String),
                    new Claim("User", JsonConvert.SerializeObject(user), new User().GetType().ToString()),
                    };

                    List<Menu> menus;

                    Stream fs = System.IO.File.OpenRead($@"{_hostingEnvironment.ContentRootPath}\menus.json");
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        menus = JsonConvert.DeserializeObject<List<Menu>>(await reader.ReadToEndAsync());
                    }
                    fs.Dispose();

                    AddRole(user, menus, claims);

                    ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "ApplicationCookie");
                    ClaimsPrincipal userPrincipal = new ClaimsPrincipal(userIdentity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

                    //await DB.SaveChangesAsync();
                    var email = user.email;
                    if (email == null || email == "")
                    {
                        TempData["email"] = "email";
                        TempData["username"] = user.Username;
                    }
                    else
                    {
                        TempData["email"] = user.email;
                    }

                    DB.Dispose();
                }
               
                
                return RedirectToLocal(returnUrl);
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Failed to login {model.Username}.");
                _logger.LogCritical(e.Message);
                ModelState.AddModelError("failed", "Invalid login attempt.");
                return RedirectToAction("Login", "Users");
            }
        }

        private void AddRole(UserMapRole user, List<Menu> menus, List<Claim> claims)
        {
            user.UserMenu.ForEach(m => {
                Menu tempMenu = menus.Find(n => n.Id.Equals(m.MenuId) && string.IsNullOrEmpty(n.Url) && !string.IsNullOrEmpty(n.Controller) && !string.IsNullOrEmpty(n.Action));

                if (tempMenu != null)
                {
                    string roleName = $"{tempMenu.Controller}{tempMenu.Action}";
                    if (!claims.Exists(c => c.Value.Equals(roleName)))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, roleName, ClaimValueTypes.String));

                        if (tempMenu.Children.Count() > 0 && tempMenu.Children.FirstOrDefault().Count() > 0)
                        {
                            AddRole(user, tempMenu.Children.FirstOrDefault(), claims);
                        }
                    }
                }
            });

            //return claims;
        }

        // GET: /<controller>/Logout
        [AllowAnonymous]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            HttpContext.Session.Clear();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            _logger.LogInformation("{userName} logged out.", User.Identity.Name);

            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Profile()
        {
            return View("Index");
        }

        [ImportModelState]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: /<controller>/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ExportModelState]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("ChangePassword", "Users");
            }

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            var users = DB.User.FromSql("SELECT userid AS Username FROM tb_master_user AS u WITH(NOLOCK) WHERE userid = '" + UserData.Username + "' AND pwd = '" + model.OldPassword + "'");

            if (await users.CountAsync() > 0)
            {
                string strSQL = $"UPDATE tb_master_user SET pwd = '{model.Password}' WHERE userid = '{UserData.Username}' AND pwd = '{model.OldPassword}'";
                var password = DB.Database.ExecuteSqlCommand(strSQL);
                _logger.LogWarning("Change password success.");
                ModelState.AddModelError("succeed", "Change password success.");
            }
            else
            {
                _logger.LogWarning("Invalid old password.");
                ModelState.AddModelError("failed", "Invalid old password.");
            }
            return RedirectToAction("ChangePassword", "Users");
        }

        #region Helpers
        //private void AddErrors(IdentityResult result)
        //{
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError("", error.Description);
        //        _logger.LogWarning("Error in creating user: {error}", error.Description);
        //    }
        //}

        //private Task<ApplicationUser> GetCurrentUserAsync()
        //{
        //    return UserManager.GetUserAsync(HttpContext.User);
        //}

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Dashboard");
            }
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> SaveEmail(string email, string user)
        {
            if (user != null)
            {
                var userid = DB.User.SingleOrDefault(c => c.Username == user);
                userid.email = email;
                DB.SaveChanges();
            }
            return RedirectToAction("Index", "Dashboard");
        }
            
        

    }

}
