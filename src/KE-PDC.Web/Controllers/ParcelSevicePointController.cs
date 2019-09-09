using KE_PDC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace KE_PDC.Web.Controllers
{
    [Authorize]
    public class ParcelSevicePointController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<ParcelSevicePointController> _logger;
        private KE_POSContext DB;
        private KE_RTSPContext DBRTSP;

        public ParcelSevicePointController(KE_POSContext context, KE_RTSPContext contextRTSP, ILogger<ParcelSevicePointController> logger, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            DB = context;
            DBRTSP = contextRTSP;
        }

        // GET: /<controller>/DailyCommissionVerify
        [Authorize(Roles = "Administrators,ParcelSevicePointDailyCommissionVerify")]
        public IActionResult DailyCommissionVerify()
        {
            ViewData["ProfileMaster"] = DBRTSP.ProfileMaster.Where(p => p.ProjectId.Equals("PSP")).ToList();

            return View();
        }
    }
}
