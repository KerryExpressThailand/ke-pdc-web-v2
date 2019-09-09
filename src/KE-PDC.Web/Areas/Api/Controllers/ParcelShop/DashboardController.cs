using KE_PDC.Models;
using KE_PDC.Services;
using KE_PDC.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KE_PDC.Areas.Api.Controllers
{
    [Area("Api")]
    [Authorize]
    public class DashboardController : Controller
    {
        new ApiResponse Response = new ApiResponse();
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<DashboardController> _logger;
        private KE_POSContext DB;
        private CultureInfo enUS = new CultureInfo("en-US");

        public DashboardController(KE_POSContext context, ILogger<DashboardController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            DB = context;
        }
    }
}
