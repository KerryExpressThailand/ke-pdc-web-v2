using KE_PDC.Models;
using KE_PDC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KE_PDC.Areas.Api.Controllers
{
    [Area("Api")]
    [Authorize]
    public class UserShopsController : Controller
    {
        new ApiResponse Response = new ApiResponse();
        private readonly ILogger<UserShopsController> _logger;
        private KE_POSContext DB;

        public UserShopsController(KE_POSContext context, ILogger<UserShopsController> logger)
        {
            _logger = logger;
            DB = context;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            // Parameter
            string profile_id = HttpContext.Request.Query["FC"].ToString().ToUpper();

            if (profile_id.Equals("ALL"))
            {
                profile_id = "";
            }

            Response.Success = true;
            Response.Result = await DB.BranchList.FromSql("EXEC sp_RPT309_GetUserShopListByProfileId '" + UserData.Username + "', '" + profile_id + "'").ToListAsync();

            DB.Dispose();

            return Json(Response.Render());
        }
    }
}
