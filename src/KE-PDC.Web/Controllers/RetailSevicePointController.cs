using KE_PDC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KE_PDC.Web.Controllers
{
    [Authorize(Roles = "Administrators,RetailSevicePoint")]
    public class RetailSevicePointController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<RetailSevicePointController> _logger;
        private KE_POSContext DB;

        public RetailSevicePointController(KE_POSContext context, ILogger<RetailSevicePointController> logger, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            DB = context;
        }

        // GET: /<controller>/MonthlyCommissionVerify
        [Authorize(Roles = "Administrators,RetailSevicePointMonthlyCommissionVerify")]
        public IActionResult MonthlyCommissionVerify()
        {
            return View();
        }
    }
}
