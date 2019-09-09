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
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KE_PDC.Areas.Api.Controllers
{
    [Route("Api/[controller]")]
    [Authorize]
    public class DCTopUpVerifyController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<DCTopUpVerifyController> _logger;
        new ApiResponse Response = new ApiResponse();
        private KE_POSContext DB;
        private CultureInfo enUS = new CultureInfo("en-US");

        public DCTopUpVerifyController(KE_POSContext context, ILogger<DCTopUpVerifyController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            DB = context;
        }

        // POST Api/<controller>/get
        [HttpPost("get")]
        public async Task<Object> Post(BranchDateFromDateToViewModel Filter)
        {
            Pagination pagination = new Pagination(HttpContext);

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            string FileType = Request.Form["filetype"];
            string Type = Request.Form["Type"];

            if (Type == null)
            {
                Type = "";
            }

            DateTime dateFrom = DateTime.ParseExact(Filter.DateFrom, "dd/MM/yyyy", enUS);
            DateTime dateTo = DateTime.ParseExact(Filter.DateTo, "dd/MM/yyyy", enUS);

            string sql = $"EXEC sp_PDC_Report_DCTopUpVerify_Get '{dateFrom.ToString("yyyyMMdd", enUS)}', '{dateTo.ToString("yyyyMMdd", enUS)}', '{UserData.Username}', '{Filter.BranchList}'";
            _logger.LogInformation(sql);
            List<LINEPayRemittance> LINEPayRemittance = await DB.LINEPayRemittance.FromSql(sql).ToListAsync();

            if (FileType != null)
            {
                if (FileType.Equals("excel"))
                {
                    return ExportExcel(dateFrom, dateTo, LINEPayRemittance);
                }
            }

            int totalCount = LINEPayRemittance.Count;

            LINEPayRemittance = LINEPayRemittance.Skip(pagination.From()).Take(pagination.To()).ToList();

            Response.Success = true;
            Response.Result = LINEPayRemittance;
            Response.ResultInfo = new
            {
                page = pagination.Page,
                perPage = pagination.PerPage,
                count = LINEPayRemittance.Count,
                totalCount = totalCount
            };

            DB.Dispose();

            return Response.Render();
        }

        // POST Api/<controller>/ASK
        [HttpGet("{BranchID}")]
        public async Task<object> Get(string BranchID, string ReportDate)
        {
            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DateTime reportDate = DateTime.ParseExact(ReportDate, "yyyyMMdd", enUS);
            string sql = $"EXEC sp_PDC_Report_DCTopUpVerify_Get '{reportDate.ToString("yyyyMMdd", enUS)}', '{reportDate.ToString("yyyyMMdd", enUS)}', '{UserData.Username}', '{BranchID}'";
            _logger.LogInformation(sql);
            List<LINEPayRemittance> LINEPayRemittance = await DB.LINEPayRemittance.FromSql(sql).ToListAsync();

            if (LINEPayRemittance.Count() == 0)
            {
                return NotFound();
            }

            Response.Success = true;
            Response.Result = LINEPayRemittance;

            return Response.Render();
        }

        // GET Api/<controller>/Download
        [HttpGet("Download")]
        public async Task<ActionResult> Download()
        {
            Pagination pagination = new Pagination(HttpContext);

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            string ReportDate = Request.Query["ReportDate"];
            string FileType = Request.Query["FileType"];
            string Type = Request.Query["Type"];

            if (Type == null)
            {
                Type = "";
            }

            DateTime reportDate = DateTime.ParseExact(ReportDate, "dd/MM/yyyy", enUS);

            string sql = $"EXEC sp_RPT308_DowloadDCTopUpReport '{UserData.Username}', '{reportDate.ToString("yyyyMMdd", enUS)}'";

            _logger.LogInformation(sql);

            List<LINEPayRemittance> LINEPayRemittance = await DB.LINEPayRemittance.FromSql(sql).ToListAsync();

            if (FileType != null)
            {
                if (FileType.Equals("excel"))
                {
                    return ExportExcelDownload(LINEPayRemittance);
                }
            }

            int totalCount = LINEPayRemittance.Count;

            LINEPayRemittance = LINEPayRemittance.Skip(pagination.From()).Take(pagination.To()).ToList();

            Response.Success = true;
            Response.Result = LINEPayRemittance;
            Response.ResultInfo = new
            {
                page = pagination.Page,
                perPage = pagination.PerPage,
                count = LINEPayRemittance.Count,
                totalCount = totalCount
            };

            DB.Dispose();

            return Json(Response.Render());
        }

        // POST Api/<controller>/ASK
        [HttpPost("{BranchID}")]
        public async Task<JsonResult> Post(string BranchID, DCTopUpVerifyViewModel Item)
        {
            if (!ModelState.IsValid)
            {
                return Json(Response.RenderError(ModelState));
            }

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            string idList = HttpContext.Request.Form["IDs"].ToString().ToUpper();
            string[] idLists = idList.Split(',');
            StringBuilder sqlStrBuilder = new StringBuilder();

            sqlStrBuilder.Append("EXEC sp_PDC_Report_DCTopUpVerify_Set '");

            sqlStrBuilder.Append(UserData.Username);
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(BranchID);
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(Item.ReportDate);
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(Item.VerifyDate);
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(Item.RemittanceDate);
            sqlStrBuilder.Append("', '");

            sqlStrBuilder.Append(Item.BonusCommission);
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(Item.AdjustmentCreditCard);
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(Item.AdjustmentOther);
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(Item.ReturnCharge);
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(Item.Suspense);
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(Item.WithHoldingTax);
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(Item.Promotion);
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(Item.BankCharge);
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(Item.AdjustmentLinePay);

            sqlStrBuilder.Append("', N'");
            sqlStrBuilder.Append(Item.BonusCommissionRemark);
            sqlStrBuilder.Append("', N'");
            sqlStrBuilder.Append(Item.AdjustmentCreditCardRemark);
            sqlStrBuilder.Append("', N'");
            sqlStrBuilder.Append(Item.AdjustmentOtherRemark);
            sqlStrBuilder.Append("', N'");
            sqlStrBuilder.Append(Item.ReturnChargeRemark);
            sqlStrBuilder.Append("', N'");
            sqlStrBuilder.Append(Item.SuspenseRemark);
            sqlStrBuilder.Append("', N'");
            sqlStrBuilder.Append(Item.WithHoldingTaxRemark);
            sqlStrBuilder.Append("', N'");
            sqlStrBuilder.Append(Item.PromotionRemark);
            sqlStrBuilder.Append("', N'");
            sqlStrBuilder.Append(Item.BankChargeRemark);
            sqlStrBuilder.Append("', N'");
            sqlStrBuilder.Append(Item.AdjustmentCreditCardRemark);

            sqlStrBuilder.Append("'");

            string strSQL = sqlStrBuilder.ToString();

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

        // POST Api/<controller>/
        [HttpPost]
        public async Task<JsonResult> PostVerifySelection(DCTopUpVerifySelectionViewModel Item)
        {
            if (!ModelState.IsValid)
            {
                return Json(Response.RenderError(ModelState));
            }

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DateTime verifyDate = DateTime.ParseExact(Item.VerifyDate, "dd/MM/yyyy", enUS);
            DateTime remittanceDate = DateTime.ParseExact(Item.RemittanceDate, "dd/MM/yyyy", enUS);

            string idList = Item.IDList.ToUpper();
            string[] idLists = idList.Split(',');
            StringBuilder sqlStrBuilder = new StringBuilder();

            foreach (var x in idLists)
            {
                string[] xSplit = x.Split('-');
                sqlStrBuilder.Append("EXEC sp_PDC_Report_DCTopUpVerify_Set '");

                sqlStrBuilder.Append(UserData.Username);
                sqlStrBuilder.Append("', '");
                sqlStrBuilder.Append(xSplit[1]);
                sqlStrBuilder.Append("', '");
                sqlStrBuilder.Append(xSplit[0]);
                sqlStrBuilder.Append("', '");
                sqlStrBuilder.Append(verifyDate.ToString("yyyyMMdd", enUS));
                sqlStrBuilder.Append("', '");
                sqlStrBuilder.Append(remittanceDate.ToString("yyyyMMdd", enUS));
                sqlStrBuilder.Append("', '");
                sqlStrBuilder.Append("0"); //@BonusCommission
                sqlStrBuilder.Append("', '");
                sqlStrBuilder.Append("0"); //@AdjCreditCard
                sqlStrBuilder.Append("', '");
                sqlStrBuilder.Append("0"); //@AdjustmentOther
                sqlStrBuilder.Append("', '");
                sqlStrBuilder.Append("0"); //@ReturnCharge
                sqlStrBuilder.Append("', '");
                sqlStrBuilder.Append("0"); //@Suspense
                sqlStrBuilder.Append("', '");
                sqlStrBuilder.Append("0"); //@WithHoldingTax
                sqlStrBuilder.Append("', '");
                sqlStrBuilder.Append("0"); //@Promotion
                sqlStrBuilder.Append("', '");
                sqlStrBuilder.Append("0"); //@BankCharge
                sqlStrBuilder.Append("', '");
                sqlStrBuilder.Append("0"); //@AdjLinePay

                sqlStrBuilder.Append("', N'");
                sqlStrBuilder.Append(""); //@BonusCommissionRemark
                sqlStrBuilder.Append("', N'");
                sqlStrBuilder.Append(""); //@AdjCreditCardRemark
                sqlStrBuilder.Append("', N'");
                sqlStrBuilder.Append(""); //@AdjustmentOtherRemark
                sqlStrBuilder.Append("', N'");
                sqlStrBuilder.Append(""); //@ReturnChargeRemark
                sqlStrBuilder.Append("', N'");
                sqlStrBuilder.Append(""); //@SuspenseRemark
                sqlStrBuilder.Append("', N'");
                sqlStrBuilder.Append(""); //@WithHoldingTaxRemark
                sqlStrBuilder.Append("', N'");
                sqlStrBuilder.Append(""); //@PromotionRemark
                sqlStrBuilder.Append("', N'");
                sqlStrBuilder.Append(""); //@BankChargeRemark
                sqlStrBuilder.Append("', N'");
                sqlStrBuilder.Append(""); //@AdjLinePayRemark

                sqlStrBuilder.Append("'");
                sqlStrBuilder.AppendLine();
            }

            string strSQL = sqlStrBuilder.ToString();

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

        private FileStreamResult ExportExcel(DateTime dateFrom, DateTime dateTo, List<LINEPayRemittance> Items)
        {
            // Load the Excel Template
            Stream xlsxStream = System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + @"\assets\templates\DCTopUpVerifyReport.xlsx");

            ExcelEngine excelEngine = new ExcelEngine();

            // Loads or open an existing workbook through Open method of IWorkbooks
            IWorkbook workbook = excelEngine.Excel.Workbooks.Open(xlsxStream);

            workbook.Version = ExcelVersion.Excel2013;

            // Sheet #1
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.Range["D2"].Text = $"{dateFrom.ToString("dd/MM/yyyy")} - {dateTo.ToString("dd/MM/yyyy")}";

            worksheet.ImportData(Items.Select(i => new
            {
                i.ERP_ID,
                i.BranchID,
                i.ReportDate,
                i.TUD,
                Status = i.TUDVerifyDate.HasValue ? "Verified" : "",
                i.TUDVerifyDate,
                RemittanceDate = i.RemittanceDate ?? DateTime.Now,
            }), 4, 1, false);

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, "Application/msexcel", "KE_PDC_DCTopUpVerify_Report_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".xlsx");
        }

        private FileStreamResult ExportExcelDownload(List<LINEPayRemittance> Items)
        {
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;

            // Creating new workbook
            IWorkbook workbook = application.Workbooks.Create(new[] { "DownloadTUDReport" });
            IWorksheet worksheet = workbook.Worksheets[0];

            workbook.Version = ExcelVersion.Excel2013;

            worksheet.Range["A1"].Text = "Report Date";
            worksheet.Range["B1"].Text = "Branch ID";
            worksheet.Range["C1"].Text = "ERP ID";
            worksheet.Range["D1"].Text = "DMSID";
            worksheet.Range["E1"].Text = "Branch Type";
            worksheet.Range["F1"].Text = "Branch Name";
            worksheet.Range["G1"].Text = "TUD Amount";
            worksheet.Range["H1"].Text = "Verified";
            worksheet.Range["I1"].Text = "TUDVerify By";
            worksheet.Range["J1"].Text = "TUDVerify Date";
            worksheet.Range["K1"].Text = "Remittance Date";

            // Header Style
            IStyle headerStyle = AddHeaderStyle(workbook);

            worksheet.Rows[0].CellStyle = headerStyle;

            worksheet.ImportData(Items.Select(i => new {
                i.ReportDate,
                i.BranchID,
                i.ERP_ID,
                i.DMSID,
                i.branch_type,
                i.BranchName,
                i.TUD,
                Verified = i.TUDVerifyDate == null ? "No" : "Yes",
                i.TUDVerifyBy,
                TUDVerifyDate = i.TUDVerifyDate == null ? new DateTime(1990, 1, 1) : i.TUDVerifyDate.Value,
                RemittanceDate = i.RemittanceDate == null ? new DateTime(1990, 1, 1) : i.RemittanceDate.Value,
            }), 2, 1, false);

            worksheet.Columns[0].CellStyle.NumberFormat = "dd/mm/yyyy";
            worksheet.Columns[6].CellStyle.NumberFormat = "#,##0.00";
            worksheet.Columns[9].CellStyle.NumberFormat = "dd/mm/yyyy hh:mm";
            worksheet.Columns[10].CellStyle.NumberFormat = "dd/mm/yyyy";

            worksheet.Range["D2"].FreezePanes();

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, "Application/msexcel", "KE_PDC_DownloadTUDReport_Report_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".xlsx");
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
