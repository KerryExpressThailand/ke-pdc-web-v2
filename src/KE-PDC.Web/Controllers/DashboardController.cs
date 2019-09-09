using KE_PDC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace KE_PDC.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private KE_POSContext DB;

        public DashboardController(KE_POSContext context) => DB = context;

        // GET: /<controller>
        //[Authorize(Roles = "Administrators,DashboardIndex")]
        public IActionResult Index()
        {
            try
            {
              
                ViewData["email"] = TempData["email"];               
               
                ViewData["username"] = TempData["username"];
                return View();
            }
            catch (Exception ex)
            {
                string mss = ex.Message.ToString();
                return View();
            }
           
        }
    }
}
