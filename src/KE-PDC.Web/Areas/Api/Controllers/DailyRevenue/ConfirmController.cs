using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace KE_PDC.Web.Areas.Api.Controllers.DailyRevenue
{
    public class ConfirmController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}