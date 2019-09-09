using KE_PDC.Models;
using KE_PDC.Models.POS.SevicePoint;
using KE_PDC.Models.SevicePoint;
using KE_PDC.Services;
using KE_PDC.ViewModel;
using KE_PDC.ViewModel.ServicePoint;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KE_PDC.Web.Areas.Api.Controllers.ParcelSevicePoint
{
    [Produces("application/json")]
    [Route("Api/ParcelSevicePoint/DailyCommissionVerify")]
    [Authorize]
    public class DailyCommissionVerifyController : Controller
    {
        new ApiResponse Response = new ApiResponse();
        private readonly ILogger<DailyCommissionVerifyController> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        private KE_POSContext DB;
        private KE_RTSPContext DBRTSP;
        private CultureInfo enUS = new CultureInfo("en-US");

        public DailyCommissionVerifyController(KE_POSContext context, KE_RTSPContext contextRTSP, ILogger<DailyCommissionVerifyController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            DB = context;
            DBRTSP = contextRTSP;
        }

        // POST: /<controller>/Get
        [HttpPost("Get")]
        public async Task<IActionResult> Get(ServicePointDaliyGetViewModel Filter)
        {
            if (!ModelState.IsValid)
            {
                return Json(Response.RenderError(ModelState));
            }

            Pagination pagination = new Pagination(HttpContext);

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DateTime dateFrom = DateTime.ParseExact(Filter.DateFrom, "dd/MM/yyyy", enUS);
            DateTime dateTo = DateTime.ParseExact(Filter.DateTo, "dd/MM/yyyy", enUS);

            StringBuilder sqlStringBuilder = new StringBuilder();

            sqlStringBuilder.Append("EXEC sp_SynRTSP07_GetCommisionFee N'");
            sqlStringBuilder.Append("<dbxml>");
            sqlStringBuilder.Append("<req>");
            sqlStringBuilder.Append($"<date_from>{ dateFrom.ToString("yyyyMMdd", enUS) }</date_from>");
            sqlStringBuilder.Append($"<date_to>{ dateTo.ToString("yyyyMMdd", enUS) }</date_to>");
            sqlStringBuilder.Append($"<project_id>PSP</project_id>");
            sqlStringBuilder.Append("</req>");

            string[] profiles = (Filter.ProfileIds ?? "ALL").Split(',');

            foreach (string profile in profiles)
            {
                sqlStringBuilder.Append($"<profile><profile_id>{ profile }</profile_id></profile>");
            }

            sqlStringBuilder.Append("</dbxml>'");

            string strSQL = sqlStringBuilder.ToString();
            _logger.LogInformation(strSQL);
            List<DailyCommission> DailyCommission = await DBRTSP.DailyCommission.FromSql(strSQL)
                .OrderBy(d => d.Verified)
                .ToListAsync();

            if(Filter.Excel ?? false)
            {
                return ExportExcel(DailyCommission);
            }

            int totalCount = DailyCommission.Count();

            DailyCommission = DailyCommission.Skip(pagination.From()).Take(pagination.To()).ToList();

            Response.Success = true;
            Response.Result = DailyCommission.Select(c => new {
                id = $"{c.ProfileId}-{c.ReportDate.ToString("yyyyMMdd", enUS)}",
                c.ProfileId,
                c.ReportDate,
                c.ProfileName,
                c.BranchId,
                c.Consignment,
                c.Boxes,
                c.Cash,
                c.Verified
            });
            Response.ResultInfo = new
            {
                page = pagination.Page,
                perPage = pagination.PerPage,
                count = DailyCommission.Count(),
                totalCount = totalCount
            };

            DB.Dispose();

            return Json(Response.Render());
        }

        private IActionResult ExportExcel(List<DailyCommission> dailyCommission)
        {
            // Load the Excel Template
            Stream xlsxStream = System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + @"\assets\templates\ParcelSevicePoint-DailyCommissionVerify.xlsx");

            ExcelEngine excelEngine = new ExcelEngine();

            // Loads or open an existing workbook through Open method of IWorkbooks
            IWorkbook workbook = excelEngine.Excel.Workbooks.Open(xlsxStream);
            IWorksheet worksheet = workbook.Worksheets[0];

            workbook.Version = ExcelVersion.Excel2013;

            worksheet.ImportData(dailyCommission.Select(d => new {
                d.ProfileId,
                d.ReportDate,
                Status = d.Verified ? "Verified" : "Pending",
                d.ProfileName,
                d.BranchId,
                d.Consignment,
                d.Boxes,
                d.Cash,
                d.Commission
            }), 4, 1, false);

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            // Close the instance of IWorkbook.
            workbook.Close();

            // Dispose the instance of ExcelEngine.
            excelEngine.Dispose();

            xlsxStream.Dispose();

            return File(ms, "Application/msexcel", $"ParcelSevicePoint_DailyCommissionVerify_{DateTime.Now.ToString("yyyMMdd_HHmmss")}.xlsx");
        }

        // POST: /<controller>/
        [HttpPost]
        public async Task<object> Post(List<ServicePointDaliyVerifyViewModel> Profile)
        {
            if (!ModelState.IsValid)
            {
                return Json(Response.RenderError(ModelState));
            }

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DailyCommission profile;

            StringBuilder sqlStringBuilder = new StringBuilder();
            sqlStringBuilder.Append("EXEC sp_SynRTSP09_SetVerify N'");
            sqlStringBuilder.Append("<dbxml>");

            Profile.ForEach(p => {
                sqlStringBuilder.Append("<profile>");
                sqlStringBuilder.Append($"<profile_id>{ p.ProfileId }</profile_id>");
                sqlStringBuilder.Append($"<report_date>{ p.ReportDate }</report_date>");
                sqlStringBuilder.Append($"<user>{ UserData.Username }</user>");
                sqlStringBuilder.Append("</profile>");
            });

            sqlStringBuilder.Append("</dbxml>'");

            string strSQL = sqlStringBuilder.ToString();
            _logger.LogInformation(strSQL);

            //var sqlResponse = await DBRTSP.Database.ExecuteSqlCommandAsync(strSQL);

            Profile.ForEach(p =>
            {
                sqlStringBuilder.Clear();

                sqlStringBuilder.Append("EXEC sp_SynRTSP07_GetCommisionFee N'");
                sqlStringBuilder.Append("<dbxml>");
                sqlStringBuilder.Append("<req>");
                sqlStringBuilder.Append($"<date_from>{ p.ReportDate }</date_from>");
                sqlStringBuilder.Append($"<date_to>{ p.ReportDate }</date_to>");
                sqlStringBuilder.Append($"<project_id>PSP</project_id>");
                sqlStringBuilder.Append("</req>");
                sqlStringBuilder.Append($"<profile><profile_id>{ p.ProfileId }</profile_id></profile>");
                sqlStringBuilder.Append("</dbxml>'");

                strSQL = sqlStringBuilder.ToString();
                _logger.LogInformation(strSQL);

                profile = DBRTSP.DailyCommission.FromSql(strSQL).First();

                DB.SevicePointDailyRevenue.Add(new SevicePointDailyRevenue
                {
                    BranchID = profile.BranchId,
                    ProfileID = profile.ProfileId,
                    ReportDate = profile.ReportDate,
                    Type = "PSP",
                    Consignment = profile.Consignment,
                    Boxes = profile.Boxes,
                    Cash = profile.Cash,
                    Captured = true,
                    CapturedDate = DateTime.Now,
                    CapturedBy = UserData.Username
                });
            });

            await DB.SaveChangesAsync();

            DBRTSP.Dispose();
            DB.Dispose();

            Response.Success = true;
            Response.Result = Profile;
            Response.Messages.Add("Verified Successful");

            return Response.Render();
        }
        
        // PUT: /<controller>/
        [HttpPut]
        public async Task<object> Put(ServicePointDaliyUpdateViewModel Profile)
        {
            if (!ModelState.IsValid)
            {
                return Json(Response.RenderError(ModelState));
            }

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            StringBuilder sqlStringBuilder = new StringBuilder();
            sqlStringBuilder.Append("EXEC sp_SynRTSP08_SetCommisionFee N'");
            sqlStringBuilder.Append("<dbxml>");
            sqlStringBuilder.Append("<daily_report>");
            sqlStringBuilder.Append($"<profile_id>{ Profile.ProfileId }</profile_id>");
            sqlStringBuilder.Append($"<report_date>{ Profile.ReportDate }</report_date>");
            sqlStringBuilder.Append($"<Con>{ Profile.Consignment }</Con>");
            sqlStringBuilder.Append($"<Cash>{ Profile.Cash }</Cash>");
            sqlStringBuilder.Append($"<user>{ UserData.Username }</user>");
            sqlStringBuilder.Append("</daily_report>");
            sqlStringBuilder.Append("</dbxml>'");

            string strSQL = sqlStringBuilder.ToString();
            _logger.LogInformation(strSQL);

            var sqlResponse = await DBRTSP.Database.ExecuteSqlCommandAsync(strSQL);

            Response.Success = true;
            Response.Result = Profile;
            Response.Messages.Add("Updated Successful");

            DB.Dispose();

            return Response.Render();
        }
    }
}