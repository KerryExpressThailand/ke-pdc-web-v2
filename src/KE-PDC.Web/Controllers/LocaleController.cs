using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KE_PDC.Controllers
{
    public class LocaleController : Controller
    {
        // GET: /<controller>/
        public JsonResult Index(string culture)
        {
            if (String.IsNullOrEmpty(culture))
            {
                culture = "en-US";
            }

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return Json(new
            {
                result = true
            });
        }
    }
}
