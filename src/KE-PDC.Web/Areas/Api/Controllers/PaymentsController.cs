using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using KE_PDC.Models;
using KE_PDC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KE_PDC.Web.Areas.Api.Controllers
{
    [Route("Api/[controller]")]
    [Authorize]
    public class PaymentsController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<PaymentsController> _logger;
        new ApiResponse Response = new ApiResponse();
        private KE_POSContext DB;
        private CultureInfo enUS = new CultureInfo("en-US");
        private List<ReviewBalanceReport> ReviewBalanceReport;

        public PaymentsController(KE_POSContext context, ILogger<PaymentsController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            DB = context;
        }

        [HttpPost]
        public IActionResult RabbitOnPostImport()
        {

            return View();
        }
    }
}