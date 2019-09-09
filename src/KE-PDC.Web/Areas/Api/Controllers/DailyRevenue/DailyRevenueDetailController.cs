using KE_PDC.Models;
using KE_PDC.Models.POS;
using KE_PDC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KE_PDC.Areas.Api.Controllers
{
    [Route("Api/[controller]")]
    [Authorize]
    public class DailyRevenueDetailController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<DailyRevenueDetailController> _logger;
        new ApiResponse Response = new ApiResponse();
        private KE_POSContext DB;
        private CultureInfo enUS = new CultureInfo("en-US");

        public DailyRevenueDetailController(KE_POSContext context, ILogger<DailyRevenueDetailController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            DB = context;
        }

        // GET Api/<controller>/5
        [HttpGet("{id}")]
        public async Task<object> Detail(string id, DateTime ReportDate)
        {
            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            //DateTime dateStr = DateTime.ParseExact(ReportDate, "dd/MM/yyyy", enUS);
            string exec = $"EXEC sp_PDC_Report_DailyRevenueVerifyMatchCash_Get '{UserData.Username}', '{id}', '{ReportDate.ToString("yyyyMMdd", enUS)}'";
            //string exec1 = $"EXEC sp_PDC_Report_DailyRevenueVerifyMatchCash_Get  '{id}', '{ReportDate.ToString("yyyyMMdd", enUS)}'";
            _logger.LogInformation(exec);           
            
            DailyRevenueDetailCash Detail = await DB.DailyRevenueDetailCash.FromSql(exec).FirstOrDefaultAsync();

            if (Detail == null)
            {
                return ResponseNotFound(id);
            }

            Response.Success = true;
            Response.Result = Detail;
            DB.Dispose();

            return Response.Render();
        }

        private JsonResult ResponseNotFound(string BranchId)
        {
            Response.Success = false;
            Response.Errors.Add(new
            {
                Key = "BranchId",
                Message = "No data found for " + BranchId
            });

            HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;

            return Json(Response.Render());
        }
    }
}
