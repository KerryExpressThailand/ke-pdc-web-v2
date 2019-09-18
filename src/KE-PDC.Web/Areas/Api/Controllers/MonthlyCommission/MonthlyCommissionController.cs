using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using KE_PDC.Models;
using Microsoft.Extensions.Logging;
using KE_PDC.ViewModel;
using System.Text;
using System.Security.Claims;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Syncfusion.XlsIO;
using System.IO;
using Syncfusion.Drawing;
using KE_PDC.Services;
using Newtonsoft.Json;
using KE_PDC.Models.POS;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KE_PDC.Areas.Api.Controllers
{
    [Route("Api/[controller]")]
    [Authorize]
    public class MonthlyCommissionController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<MonthlyExpenseController> _logger;
        new ApiResponse Response = new ApiResponse();
        private KE_POSContext DB;
        private CultureInfo enUS = new CultureInfo("en-US");

        public MonthlyCommissionController(KE_POSContext context, ILogger<MonthlyExpenseController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            DB = context;
        }

        // GET Api/<controller>
        public async Task<ActionResult> Get(BranchMonthlyViewModel Filter, String FileType, String Type)
        {
            Pagination pagination = new Pagination(HttpContext);

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            if(Type == null)
            {
                Type = "";
            }

            DateTime monthYear = DateTime.ParseExact(Filter.MonthYear, "MM/yyyy", enUS);

            string exec = $"sp_RPT300_FN_MonthlyExpenseSync '{UserData.Username}', '{monthYear.Month.ToString()}', '{monthYear.Year.ToString()}'";
            _logger.LogInformation(exec);
            await DB.Database.ExecuteSqlCommandAsync(exec);

            exec = $"EXEC sp_PDC_Report_MonthlyCommissionSummary_Get '{UserData.Username}', '{Filter.BranchList}', '{monthYear.Month.ToString()}', '{monthYear.Year.ToString()}', { (Type.Equals("rt") ? "0" : "1") }, ''";
            _logger.LogInformation(exec);
            List<MonthlySummaryCommission> MonthlySummaryCommission = await DB.MonthlySummaryCommission.FromSql(exec).ToListAsync();

            if (FileType != null)
            {
                if (FileType.Equals("excel"))
                {
                    return ExportExcelMonthlyCommission(Type, MonthlySummaryCommission, monthYear);
                }
            }

            int totalCount = MonthlySummaryCommission.Count();

            MonthlySummaryCommission = MonthlySummaryCommission.Skip(pagination.From()).Take(pagination.To()).ToList();

            Response.Success = true;
            Response.Result = MonthlySummaryCommission;
            Response.ResultInfo = new
            {
                page = pagination.Page,
                perPage = pagination.PerPage,
                count = MonthlySummaryCommission.Count(),
                totalCount = totalCount
            };

            DB.Dispose();

            return Json(Response.Render());
        }

        [HttpPost]
        //[Route ("GetMonthlyCommission")]
        public async Task<ActionResult> GetMonthlyCommission(BranchMonthlyViewModel Filter, String FileType, String Type)
        {
            Pagination pagination = new Pagination(HttpContext);

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            if (Type == null)
            {
                Type = "";
            }

            DateTime monthYear = DateTime.ParseExact(Filter.MonthYear, "MM/yyyy", enUS);

            string exec = $"sp_RPT300_FN_MonthlyExpenseSync '{UserData.Username}', '{monthYear.Month.ToString()}', '{monthYear.Year.ToString()}'";
            _logger.LogInformation(exec);
            await DB.Database.ExecuteSqlCommandAsync(exec);

            exec = $"EXEC sp_PDC_Report_MonthlyCommissionSummary_Get '{UserData.Username}', '{Filter.BranchList}', '{monthYear.Month.ToString()}', '{monthYear.Year.ToString()}', { (Type.Equals("rt") ? "0" : "1") }, ''";
            _logger.LogInformation(exec);
            List<MonthlySummaryCommission> MonthlySummaryCommission = await DB.MonthlySummaryCommission.FromSql(exec).ToListAsync();

            if (FileType != null)
            {
                if (FileType.Equals("excel"))
                {
                    return ExportExcelMonthlyCommission(Type, MonthlySummaryCommission, monthYear);
                }
            }

            int totalCount = MonthlySummaryCommission.Count();

            MonthlySummaryCommission = MonthlySummaryCommission.Skip(pagination.From()).Take(pagination.To()).ToList();

            Response.Success = true;
            Response.Result = MonthlySummaryCommission;
            Response.ResultInfo = new
            {
                page = pagination.Page,
                perPage = pagination.PerPage,
                count = MonthlySummaryCommission.Count(),
                totalCount = totalCount
            };

            DB.Dispose();

            return Json(Response.Render());
        }

        private ActionResult ExportExcelMonthlyCommission(string type, List<MonthlySummaryCommission> List, DateTime MonthYear)
        {
            // Load the Excel Template
            Stream xlsxStream = System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + @"\assets\templates\CommissionReport.xlsx");

            ExcelEngine excelEngine = new ExcelEngine();

            // Loads or open an existing workbook through Open method of IWorkbooks
            IWorkbook workbook = excelEngine.Excel.Workbooks.Open(xlsxStream);
            IWorksheet worksheet = workbook.Worksheets[0];

            workbook.Version = ExcelVersion.Excel2013;

            worksheet.Range["C2"].Text = $"Commission {MonthYear.ToString("y")}";

            string dateStart = $"26 {MonthYear.AddMonths(-1).ToString("MMMM")}";

            if (MonthYear.Month < 3 && MonthYear.Year < 2018)
                dateStart = $"01 {MonthYear.ToString("MMMM")}";

            //worksheet.Range["X3"].Text = $"Package Order {dateStart} - {MonthYear.ToString("25 MMMM yyyy")}";

            worksheet.ImportData(List.Select(l => new {
                l.ERPID,
                l.BranchID,

                /* KERRY */
                Boxes = ToInteger(l.Boxes),
                TotalRevenue = ToDecimal(l.TotalRevenue),
                PackageSurcharge = ToDecimal(l.PackageSurcharge),
                TotalFreightRevenue = ToDecimal(l.TotalFreightRevenue),
                CommissionRate = (decimal)(l.CommissionRate ?? 0) / 100,
                IncomeTotalFreightRevenue = ToDecimal(l.IncomeTotalFreightRevenue),

                Discount = 0,

                /* DHL */
                DHLAmount = ToDecimal(l.DHLAmount) + ToDecimal(l.DHLAdjustment),
                DHLRate = (decimal)(l.DHLRate ?? 0)/100,
                IncomeDHL = ToDecimal(l.IncomeDHL),

                /* COD */
                CODAmount = ToDecimal(l.CODAmount),
                CODRate = (decimal)(l.CODRate ?? 0) / 100,
                IncomeCOD = ToDecimal(l.IncomeCOD),
                ExpenseCOD = ToDecimal(l.ExpenseCOD),

                /* INSURANCE */
                InsuranceAmount = ToDecimal(l.InsuranceAmount),
                InsuranceRate = (decimal)(l.InsureRate ?? 0) / 100,
                IncomeInsurance = ToDecimal(l.IncomeInsurance),
                ExpenseInsurance = ToDecimal(l.ExpenseInsurance),

                /* SAMEDAY */
                TotalSamedayRevenue = ToDecimal(l.TotalSamedayRevenue),
                SamedayRate = (decimal)(l.BSDRate ?? 0) / 100,
                IncomeSameday = ToDecimal(l.IncomeSameday),

                /* DROP OFF */
                DropOffRevenue = ToInteger(l.DropOffRevenue),
                DropOffRate = (decimal)(l.DropOffRate ?? 0) / 100,
                IncomeDropoff = ToInteger(l.IncomeDropoff),

                /* RTSP */
                //RTSPRevenue = ToDecimal(l.RTSPRevenue),
                RTSPRate = (decimal)(l.RTSPRate ?? 0) / 100,
                IncomeRTSP = ToDecimal(l.IncomeRTSP),

                /* PSP */
                //PSPRevenue = ToDecimal(l.PSPRevenue),
                PSPRate = (decimal)(l.PSPRate ?? 0) / 100,
                IncomePSP = ToDecimal(l.IncomePSP),

                /* FREE */
                ExpenseFeeManagement = ToDecimal(l.ExpenseFeeManagement),
                ExpenseFeeIT = ToDecimal(l.ExpenseFeeIT),
                ExpenseFeeSupply = ToDecimal(l.ExpenseFeeSupply),
                ExpenseFeeFacility = ToDecimal(l.ExpenseFeeFacility),

                /* SALES PACKAGE */
                BoxMini = ToDecimal(l.BoxMini),
                BoxS = ToDecimal(l.BoxS),
                BoxSPlus = ToDecimal(l.BoxSPlus),
                BoxM = ToDecimal(l.BoxM),
                BoxMPlus = ToDecimal(l.BoxMPlus),
                BoxL = ToDecimal(l.BoxL),
                ExpenseSalesPackage = ToDecimal(l.ExpenseSalesPackage),

                /* */
                Adjustment = ToDecimal(l.Adjustment),
                l.AdjustmentRemark,

                /* COMMISSION */
                TotalCommission = ToDecimal(l.TotalCommission),
                NetCommission = ToDecimal(l.NetCommission),

                /* ERP */
                SendToERPDate = l.SendToERPDate.HasValue ? l.SendToERPDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : "-",
                ReciveERPDate = l.PRDate.HasValue ? l.PRDate.Value.ToString("dd-MM-yyyy HH:mm:ss") : "-",
                l.PRNo
            }), 5, 1, false);


            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            // Close the instance of IWorkbook.
            workbook.Close();

            // Dispose the instance of ExcelEngine.
            excelEngine.Dispose();

            xlsxStream.Dispose();

            //return File(ms, "Application/msexcel", "KE_PDC_MonthlyCommission" + type.ToUpper() + "_Report_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".xlsx");
            return File(ms, "Application/msexcel", $"KE_PDC_MonthlyCommission_Report_{MonthYear.ToString("MMM_yyyy")}_{DateTime.Now.ToString("yyyMMdd_HHmmss")}.xlsx");
        }

        // GET Api/<controller>/ASK
        [HttpGet("{id}")]
        public async Task<JsonResult> Detail(BranchMonthlyViewModel Filter, string id)
        {
            Pagination pagination = new Pagination(HttpContext);

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DateTime monthYear = DateTime.ParseExact(Filter.MonthYear, "MM/yyyy", enUS);

            string exec = $"EXEC sp_PDC_Report_MonthlyCommissionSummary_Get '{UserData.Username}', '{id}', '{monthYear.Month.ToString()}', '{monthYear.Year.ToString()}', 1, ''";
            _logger.LogInformation(exec);
            List<MonthlySummaryCommission> MonthlyCommissionFC = await DB.MonthlySummaryCommission.FromSql(exec).ToListAsync();

            if (MonthlyCommissionFC.Count() > 0)
            {
                exec = $"EXEC sp_RPT310_GetMonthlyExpenseDetail '{UserData.Username}', '{ id }', '{monthYear.Month.ToString()}', '{monthYear.Year.ToString()}'";
                _logger.LogInformation(exec);
                MonthlyCommissionFC.FirstOrDefault().MonthlyExpenseDetail = await DB.MonthlyExpenseDetail
                    .FromSql(exec)
                    .ToListAsync();
            }

            Response.Success = true;
            Response.Result = MonthlyCommissionFC;

            DB.Dispose();

            return Json(Response.Render());
        }

        // POST Api/<controller>/Verify
        [HttpPost("Verify")]
        public async Task<JsonResult> Verify(MonthlyCommissionVerifyViewModel MonthlyCommissionVerify)
        {
            if (!ModelState.IsValid)
            {
                return Json(Response.RenderError(ModelState));
            }

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DateTime monthYear = DateTime.ParseExact(MonthlyCommissionVerify.MonthYear, "MM/yyyy", enUS);
            StringBuilder SQLStringBuilder = new StringBuilder();

            SQLStringBuilder.AppendLine($"EXEC sp_RPT311_SaveMonthlyCommissionVerifyFC '{UserData.Username}', '{MonthlyCommissionVerify.BranchID}', {monthYear.Month.ToString()}, {monthYear.Year.ToString()}");
            string strSQL = SQLStringBuilder.ToString();

            #region Product
            if (_hostingEnvironment.IsProduction())
            {
                SaveDBResponse save = await DB.SaveDBResponse.FromSql(strSQL).FirstAsync();

                Response.Success = save.code.Equals("000");
                Response.Messages.Add(save.desc);
            }
            #endregion

            #region Tester
            else
            {
                Response.Success = true;
                Response.Messages.Add("Test");
            }
            #endregion

            DB.Dispose();

            return Json(Response.Render());
        }

        private decimal ToDecimal(decimal? value)
        {
            if (value.HasValue)
            {
                return value.Value;
            }

            return (decimal)0.00;
        }
        private int ToInteger(int? value)
        {
            if (value.HasValue)
            {
                return value.Value;
            }

            return 0;
        }

        private IStyle AddHeaderStyle(IWorkbook workbook)
        {
            // Header Style
            IStyle headerStyle = workbook.Styles.Add("HeaderStyle");

            headerStyle.BeginUpdate();
            headerStyle.Color = Color.FromArgb(234, 112, 33);
            headerStyle.Font.Color = ExcelKnownColors.White;
            headerStyle.Font.Bold = true;
            headerStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
            headerStyle.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            headerStyle.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
            headerStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            headerStyle.EndUpdate();

            return headerStyle;
        }
    }
}
