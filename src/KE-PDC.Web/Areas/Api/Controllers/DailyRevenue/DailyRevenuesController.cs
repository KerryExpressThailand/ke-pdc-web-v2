using KE_PDC.Models;
using KE_PDC.Models.POS;
using KE_PDC.Services;
using KE_PDC.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace KE_PDC.Web.Areas.Api.Controllers.DailyRevenue
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyRevenuesController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<DailyRevenuesController> _logger;
        new readonly ApiResponse Response = new ApiResponse();
        private readonly KE_POSContext DB;
        private readonly CultureInfo enUS = new CultureInfo("en-US");

        public DailyRevenuesController(KE_POSContext context, ILogger<DailyRevenuesController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            DB = context;
        }
    }
}