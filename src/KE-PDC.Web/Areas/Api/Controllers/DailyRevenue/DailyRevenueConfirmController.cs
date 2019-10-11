using KE_PDC.Models;
using KE_PDC.Models.POS;
using KE_PDC.Services;
using KE_PDC.ViewModel;
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

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KE_PDC.Areas.Api.Controllers
{
    [Route("Api/[controller]")]
    [Authorize]
    public class DailyRevenueConfirmController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<DailyRevenueConfirmController> _logger;
        new ApiResponse Response = new ApiResponse();
        private KE_POSContext DB;
        private CultureInfo enUS = new CultureInfo("en-US");
        private List<ReviewBalanceReport> ReviewBalanceReport;

        public DailyRevenueConfirmController(KE_POSContext context, ILogger<DailyRevenueConfirmController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            DB = context;
        }

        // POST: /<controller>/Get
        [HttpPost("Get")]
        public async Task<ActionResult> Get(TypeBranchDateFromDateToViewModel Filter, string FileType)
        {
            if (!ModelState.IsValid)
            {
                return Json(Response.RenderError(ModelState));
            }

            // Parameter
            string filetype = (FileType ?? "").ToLower();

            bool withReviewBalance = filetype.Equals("excelreviewbalance");

            Pagination pagination = new Pagination(HttpContext);

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DateTime dateFrom = DateTime.ParseExact(Filter.DateFrom, "dd/MM/yyyy", enUS);
            DateTime dateTo = DateTime.ParseExact(Filter.DateTo, "dd/MM/yyyy", enUS);

            string parameter = $"'{ dateFrom.ToString("yyyyMMdd", enUS) }', '{ dateTo.ToString("yyyyMMdd", enUS) }', '{ UserData.Username }', '{ Filter.BranchList }'";
            string sql = "EXEC sp_PDC_Report_DailyRevenueConfirm_Get " + parameter;

            _logger.LogInformation(sql);

            List<DailyRevenueConfirm> DailyRevenueConfirm = await DB.DailyRevenueConfirm.FromSql(sql).ToListAsync();

            if (withReviewBalance)
            {
                string[] branchList = Filter.BranchList.Split(',', StringSplitOptions.RemoveEmptyEntries);
                ReviewBalanceReport = DB.ReviewBalanceReport
                    .Where(
                        rb => rb.ReportDate.Value >= dateFrom
                        && rb.ReportDate <= dateTo
                        && branchList.Contains(rb.BranchID))
                    .OrderBy(rb => rb.OracleDC)
                    .OrderBy(rb => rb.ReceiptDate)
                    .ToList();
            }

            if (filetype.Equals("excel") || filetype.Equals("excelreviewbalance"))
            {
                return ExportExcelDailyRevenueConfirm(dateFrom, dateTo, DailyRevenueConfirm, withReviewBalance, ReviewBalanceReport);
            }

            int totalCount = DailyRevenueConfirm.Count();

            DailyRevenueConfirm = DailyRevenueConfirm.Skip(pagination.From()).Take(pagination.To()).ToList();

            Response.Success = true;
            Response.Result = DailyRevenueConfirm;
            Response.ResultInfo = new
            {
                page = pagination.Page,
                perPage = pagination.PerPage,
                count = DailyRevenueConfirm.Count(),
                totalCount
            };

            DB.Dispose();

            return Json(Response.Render());
        }

        // POST: /<controller>/Pivot
        [HttpPost("Pivot")]
        public async Task<ActionResult> Pivot(TypeBranchDateFromDateToViewModel Filter)
        {
            if (!ModelState.IsValid)
            {
                return Json(Response.RenderError(ModelState));
            }

            DateTime DateTimeStartGen = DateTime.Now;

            // Auth Data
            string userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DateTime dateFrom = DateTime.ParseExact(Filter.DateFrom, "dd/MM/yyyy", enUS);
            DateTime dateTo = DateTime.ParseExact(Filter.DateTo, "dd/MM/yyyy", enUS);

            string parameter = $"'{ dateFrom.ToString("yyyyMMdd", enUS) }', '{ dateTo.ToString("yyyyMMdd", enUS) }', '{ UserData.Username }', '{ Filter.BranchList }'";
            string sql = "EXEC sp_PDC_Report_DailyRevenueConfirm_Pivot " + parameter;

            _logger.LogInformation(sql);

            List<DailyRevenuePivot> DailyRevenuePivot = await DB.DailyRevenuePivot.FromSql(sql).ToListAsync();
            
            // Load the Excel Template
            Stream xlsxStream = System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + @"\assets\templates\DailyRevenueConfirmPivot.xlsx");

            ExcelEngine excelEngine = new ExcelEngine();

            // Loads or open an existing workbook through Open method of IWorkbooks
            IWorkbook workbook = excelEngine.Excel.Workbooks.Open(xlsxStream);

            workbook.Version = ExcelVersion.Excel2016;

            string strDateRange = $"{dateFrom.ToString("dd/MM/yyyy")} - {dateTo.ToString("dd/MM/yyyy")}";

            // Sheet #2 (Daily Revenue Confirm)
            IWorksheet worksheet = workbook.Worksheets[1];
            worksheet.Range["E2"].Text = strDateRange;
            worksheet.ImportData(DailyRevenuePivot, 4, 1, false);

            // Copying a Range “A1” to “A5”.
            int rangeStart = 4;
            int rangeEnd = (rangeStart + DailyRevenuePivot.Count())-1;

            //IRange source = worksheet.Range[$"AH{rangeStart}:AR{rangeStart}"];
            //IRange destination = worksheet.Range[$"AH{rangeStart}:AR{rangeEnd}"];

            //source.CopyTo(destination, ExcelCopyRangeOptions.All);

            // Sheet #3 - #7
            workbook.Worksheets[2].Range["C3"].Text = strDateRange;
            workbook.Worksheets[3].Range["C3"].Text = strDateRange;
            workbook.Worksheets[4].Range["C3"].Text = strDateRange;
            workbook.Worksheets[5].Range["C3"].Text = strDateRange;
            workbook.Worksheets[6].Range["C3"].Text = strDateRange;

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            // Close the instance of IWorkbook.
            workbook.Close();

            // Dispose the instance of ExcelEngine.
            excelEngine.Dispose();

            xlsxStream.Dispose();

            _logger.LogInformation($"Generate duration: {(DateTime.Now - DateTimeStartGen).TotalSeconds}");

            return File(ms, "Application/msexcel", "KE_PDC_DailyRevenuePivot_Report_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".xlsx");
        }

        // PUT Api/<controller>
        [HttpPut]
        public async Task<JsonResult> Put(DailyRevenueDetailViewModel adjustment)
        {
            if (!ModelState.IsValid)
            {
                return Json(Response.RenderError(ModelState));
            }

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DateTime verifydate = DateTime.ParseExact(adjustment.VerifyDate, "dd/MM/yyyy", new CultureInfo("en-US"));
            DateTime remittanceDate = DateTime.ParseExact(adjustment.RemittanceDate, "dd/MM/yyyy", new CultureInfo("en-US"));

            StringBuilder sqlStrBuilder = new StringBuilder();

            sqlStrBuilder.Append("EXEC sp_PDC_Report_DailyRevenueConfirm_Put '");
            sqlStrBuilder.Append(adjustment.ReportDate.ToString("yyyyMMdd", new CultureInfo("en-US")));
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(UserData.Username);
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(adjustment.Branch);
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(adjustment.Comm);
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(adjustment.AdjCreditCard);
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(adjustment.Other);
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(adjustment.Return);
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(adjustment.Suspensse);
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(adjustment.WithHoldingTax);
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(adjustment.Promotion);
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(adjustment.BankCharge);
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(adjustment.AdjLinePay);

            //add by kathawutpa 17/7/2019 for project sales x
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(adjustment.Project);

            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(verifydate.ToString("yyyyMMdd", new CultureInfo("en-US")));
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(remittanceDate.ToString("yyyyMMdd", new CultureInfo("en-US")));

            //Remark
            sqlStrBuilder.Append("', N'");
            sqlStrBuilder.Append(adjustment.CommRemark);
            sqlStrBuilder.Append("', N'");
            sqlStrBuilder.Append(adjustment.AdjCreditCardRemark);
            sqlStrBuilder.Append("', N'");
            sqlStrBuilder.Append(adjustment.OtherRemark);
            sqlStrBuilder.Append("', N'");
            sqlStrBuilder.Append(adjustment.ReturnRemark);
            sqlStrBuilder.Append("', N'");
            sqlStrBuilder.Append(adjustment.SuspensseRemark);
            sqlStrBuilder.Append("', N'");
            sqlStrBuilder.Append(adjustment.WithHoldingTaxRemark);
            sqlStrBuilder.Append("', N'");
            sqlStrBuilder.Append(adjustment.PromotionRemark);
            sqlStrBuilder.Append("', N'");
            sqlStrBuilder.Append(adjustment.BankChargeRemark);
            sqlStrBuilder.Append("', N'");
            sqlStrBuilder.Append(adjustment.AdjLinePayRemark);

            //add by kathawutpa 17/7/2019 for project sales x
            sqlStrBuilder.Append("', N'");
            sqlStrBuilder.Append(adjustment.ProjectRemark);

            sqlStrBuilder.Append("'");
            string strSQL = sqlStrBuilder.ToString();

            #region Product
            if(_hostingEnvironment.IsProduction())
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

            Response.Result = adjustment;

            DB.Dispose();

            return Json(Response.Render());
        }

        // POST Api/<controller>
        [HttpPost]
        public async Task<JsonResult> Post()
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

            foreach (var x in idLists)
            {
                string[] xSplit = x.Split('-');
                sqlStrBuilder.Append("EXEC sp_PDC_Report_DailyRevenueConfirm_Set '");

                sqlStrBuilder.Append(xSplit[1]);
                sqlStrBuilder.Append("', '");
                sqlStrBuilder.Append(UserData.Username);
                sqlStrBuilder.Append("', '");
                sqlStrBuilder.Append(xSplit[0]);

                sqlStrBuilder.AppendLine("'");
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

        private FileStreamResult ExportExcelDailyRevenueConfirm(DateTime dateFrom, DateTime dateTo, List<DailyRevenueConfirm> Items, bool withReviewBalance, List<ReviewBalanceReport> ReviewBalanceReport)
        {
            // Load the Excel Template
            Stream xlsxStream = System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + @"\assets\templates\DailyRevenueConfirmReport.xlsx");

            ExcelEngine excelEngine = new ExcelEngine();

            // Loads or open an existing workbook through Open method of IWorkbooks
            IWorkbook workbook = excelEngine.Excel.Workbooks.Open(xlsxStream);

            workbook.Version = ExcelVersion.Excel2013;

            // Sheet #1
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.Range["E2"].Text = $"{dateFrom.ToString("dd/MM/yyyy")} - {dateTo.ToString("dd/MM/yyyy")}";

            worksheet.ImportData(Items.Select(i => new
            {
                i.ShopType,
                i.ERP_ID,
                i.BranchID,
                i.ReportDate, //ReportDate = i.ReportDate.ToString("d/M/yyyy"),
                i.Freight,
                i.Transport,
                i.AM,
                i.PUP,
                i.SAT,
                i.RAS,
                i.COD,//10
                i.Insur,
                i.Pkg,
                i.SalePackage,
                i.LineTopUp,
                i.mPayService,
                i.rabbitTopUp,
                i.Cash,
                i.Rabbit,
                i.CreditBBL,
                i.CreditSCB,
                i.QRPay,
                i.LinePay,
                i.Transportation,
                i.VASSurcharge,//20
                i.Discount,
                i.Vat,
                i.Total,
                i.BSDSurcharge,
                i.BSDCash,// BSD
                i.BSDLinePay,// BSD
                i.BSDLineTopUp,// BSD
                i.BSDCODSurcharge,// BSD
                i.BSDCODVasSurcharge,// BSD
                i.BSDCODVat,// BSD
                i.BSDConsignment,// BSD
                i.BSDBoxes,// BSD
                i.TUD,
                i.TotalTransfer,
                //i.BonusCommission,
                i.AdjCreditCard,
                i.AdjOther,
                i.ReturnCharge,
                i.Suspense,
                i.WithHoldingTax,
                i.Promotion,
                i.BankCharge,
                i.AdjLinePay,

                //add by kathawutpa 17/7/2019 for project sales x
                i.Project,

                i.TotalCon,
                i.TotalBoxes,
                i.TotalDropOffBoxes,
                RemittanceDate = i.RemittanceDate ?? DateTime.Now,
                VerifyDate = i.VerifyDate ?? DateTime.Now,
                Captured = i.TUDVerifyDate.HasValue || i.Captured ? "YES" : "NO",
                CapturedDate = i.TUDVerifyDate ?? (i.CapturedDate.HasValue ? i.CapturedDate.Value : DateTime.Now),
                CapturedBy = string.IsNullOrEmpty(i.TUDVerifyBy) ? i.CapturedBy : i.TUDVerifyBy,
                Approved = i.Approved ? "YES" : "NO",
                ApprovedDate = i.ApprovedDate ?? DateTime.Now,
                i.ApprovedBy,
            }), 5, 1, false);

            if (withReviewBalance)
            {
                // Sheet #2
                worksheet = workbook.Worksheets[1];

                worksheet.ImportData(ReviewBalanceReport.Select(b => new
                {
                    b.ShopType,
                    b.OracleDC,
                    SaleDate = b.SaleDate.Value.Date, //Sale_Date = b.Sale_Date.ToString("d/M/yyyy"),
                    ReceiptDate = b.ReceiptDate.Value.Date, //Receipt_Date = b.Receipt_Date.ToString("d/M/yyyy"),
                    b.OracleAccount,
                    b.ItemName,
                    Debit = b.Debit ?? (decimal)0,
                    Credit = b.Credit ?? (decimal)0,
                    Total = b.Total ?? (decimal)0,
                    b.Remark,
                    b.SortBy,
                    b.Company,
                    b.Account,
                    b.BU,
                    b.CostCenter,
                    b.DcDel,
                    b.Truck,
                    b.Reserve1,
                    b.Reserve2,
                    b.Reserve3,
                }), 4, 1, false);
            }
            else
            {
                workbook.Worksheets[1].Remove();
            }

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            // Close the instance of IWorkbook.
            workbook.Close();

            // Dispose the instance of ExcelEngine.
            excelEngine.Dispose();

            xlsxStream.Dispose();

            return File(ms, "Application/msexcel", "KE_PDC_DailyRevenueConfirm_Report_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".xlsx");
        }
    }
}
