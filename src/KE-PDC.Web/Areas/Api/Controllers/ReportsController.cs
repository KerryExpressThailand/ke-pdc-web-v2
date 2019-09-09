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
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf.Parsing;
using Syncfusion.XlsIO;
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
    [Route("Api/[controller]")]
    [Authorize]
    public class ReportsController : Controller
    {
        new ApiResponse Response = new ApiResponse();
        private readonly ILogger<ReportsController> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        private KE_POSContext DB;
        private KE_PMGWContext DBPMGW;
        private CultureInfo enUS = new CultureInfo("en-US");

        public ReportsController(KE_POSContext context, KE_PMGWContext PMGWcontext, ILogger<ReportsController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            DB = context;
            DBPMGW = PMGWcontext;
        }

        // POST: /<controller>/
        [HttpPost]
        public async Task<ActionResult> Get(ReportViewModel Filter, string type, string fileType)
        {
            if (!ModelState.IsValid)
            {
                return Json(Response.RenderError(ModelState));
            }

            // Parameter
            type = (type ?? "").ToLower();
            fileType = (fileType ?? "").ToLower();

            Pagination pagination = new Pagination(HttpContext);

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DateTime dateFrom = DateTime.ParseExact(Filter.DateFrom, "dd/MM/yyyy", enUS);
            DateTime dateTo = DateTime.ParseExact(Filter.DateTo, "dd/MM/yyyy", enUS);

            string parameter = "'" + dateFrom.ToString("yyyyMMdd", enUS) + "', '" + dateTo.ToString("yyyyMMdd", enUS) + "', '" + UserData.Username + "', '" + Filter.BranchList + "'";
            int count = 0;
            int totalCount = 0;
            object result = new List<object>();

            #region ShopDailyRevenue
            if (type.Equals("shopdailyrevenue"))
            {
                List<ShopDailyRevenue> DailyRevenues = await DB.ShopDailyRevenue.FromSql("EXEC sp_RPT304_ShopDailyRevenue " + parameter + ", " + Filter.OrderBy.ToString()).ToListAsync();

                if (fileType.Equals("excel"))
                {
                    return ExportExcelShopDailyRevenue(dateFrom, dateTo, DailyRevenues);
                }

                totalCount = DailyRevenues.Count();

                DailyRevenues = DailyRevenues.Skip(pagination.From()).Take(pagination.To()).ToList();

                result = DailyRevenues;
                count = DailyRevenues.Count();
            }
            #endregion

            #region Receipt
            else if (type.Equals("receipt"))
            {
                List<Receipt> Receipts = await DB.Receipt.FromSql("EXEC sp_RPT305_ReceiptReport " + parameter).ToListAsync();

                if (fileType.Equals("excel"))
                {
                    return ExportExcelReceipt(Receipts, dateFrom, dateTo);
                }

                totalCount = Receipts.Count();

                Receipts = Receipts.Skip(pagination.From()).Take(pagination.To()).ToList();

                result = Receipts;
                count = Receipts.Count();
            }
            #endregion

            #region TaxInvoice
            else if (type.Equals("taxinvoice"))
            {
                List<TaxInvoice> TaxInvoices = await DB.TaxInvoice.FromSql("EXEC sp_RPT306_TaxInvoiceReport " + parameter + ", " + (Filter.Canceled == null || Filter.Canceled.Equals(false) ? "0" : "1")).ToListAsync();

                if (fileType.Equals("excel"))
                {
                    return ExportExcelTaxInvoice(TaxInvoices, dateFrom, dateTo);
                }

                totalCount = TaxInvoices.Count();

                TaxInvoices = TaxInvoices.Skip(pagination.From()).Take(pagination.To()).ToList();

                result = TaxInvoices;
                count = TaxInvoices.Count();
            }
            #endregion

            #region MonthlyCommission
            else if (type.Equals("commission"))
            {
                DateTime MonthYear = DateTime.ParseExact(Filter.MonthYear, "MM/yyyy", enUS);
                List<MonthlyCommission> MonthlyCommissions = await DB.MonthlyCommission.FromSql("EXEC sp_RPT307_MonthlyCommission '" + MonthYear.ToString("yyyyMM", enUS) + "', '" + UserData.Username + "', '" + Filter.BranchList + "'").ToListAsync();

                if (fileType.Equals("excel"))
                {
                    dateFrom = MonthYear;
                    dateTo = MonthYear.AddMonths(1).AddDays(-1);
                    List<DailyCommission> DailyCommissions = await DB.DailyCommission.FromSql("EXEC sp_RPT307_DailyCommission '" + dateFrom.ToString("yyyyMMdd", enUS) + "', '" + dateTo.ToString("yyyyMMdd", enUS) + "', '" + UserData.Username + "', '" + Filter.BranchList + "'").ToListAsync();

                    return ExportExcelMonthlyCommissions(DailyCommissions, MonthlyCommissions);
                }

                totalCount = MonthlyCommissions.Count();

                MonthlyCommissions = MonthlyCommissions.Skip(pagination.From()).Take(pagination.To()).ToList();

                result = MonthlyCommissions;
                count = MonthlyCommissions.Count();
            }
            #endregion

            #region LINEPay
            else if (type.Equals("linepay"))
            {
                string LineType = HttpContext.Request.Query["TypeLine"].ToString().ToLower();

                List<LINEPay> LINEPay = new List<Models.LINEPay> { };
                List<LINETopUp> LINETopUp = new List<Models.LINETopUp> { };

                if (LineType.Equals("payment") || fileType.Equals("excel"))
                {
                    LINEPay = await DBPMGW.LINEPay.FromSql("EXEC sp_RLP001_LINEPayReport '" + dateFrom.ToString("yyyyMMdd", enUS) + "', '" + Filter.BranchList + "'").ToListAsync();

                    totalCount = LINEPay.Count();

                    LINEPay = LINEPay.Skip(pagination.From()).Take(pagination.To()).ToList();
                    result = LINEPay;

                    count = LINEPay.Count();
                }

                if (LineType.Equals("topup") || fileType.Equals("excel"))
                {
                    LINETopUp = await DBPMGW.LINETopUp.FromSql("EXEC sp_RLP002_LINETopUpReport '" + dateFrom.ToString("yyyyMMdd", enUS) + "', '" + Filter.BranchList + "'").ToListAsync();

                    totalCount = LINETopUp.Count();

                    LINETopUp = LINETopUp.Skip(pagination.From()).Take(pagination.To()).ToList();
                    result = LINETopUp;

                    count = LINETopUp.Count();
                }

                if (fileType.Equals("excel"))
                {
                    return ExportExcelLINEPay(LINEPay, LINETopUp);
                }
            }
            #endregion

            #region LINEPayRemittance
            else if (type.Equals("linetopupremittance"))
            {
                DateTime dateRemittance = DateTime.ParseExact(Filter.DateRemittance, "dd/MM/yyyy", enUS);

                StringBuilder sqlStringBuilder = new StringBuilder();
                sqlStringBuilder.Append("EXEC sp_RPT308_LINEPayRemittanceReport '");
                //sqlStringBuilder.Append(dateFrom.ToString("yyyyMMdd", enUS));
                //sqlStringBuilder.Append("','");
                //sqlStringBuilder.Append(dateTo.ToString("yyyyMMdd", enUS));
                //sqlStringBuilder.Append("','");
                sqlStringBuilder.Append(dateRemittance.ToString("yyyyMMdd", enUS));
                sqlStringBuilder.Append("'");

                List<LINEPayRemittance> LINEPayRemittance = await DB.LINEPayRemittance.FromSql(sqlStringBuilder.ToString()).ToListAsync();

                if (fileType.Equals("excel"))
                {
                    return ExportExcelLINEPayRemittance(LINEPayRemittance);
                }
                else if (fileType.Equals("pdf"))
                {
                    return ExportPdfLINEPayRemittance(LINEPayRemittance, dateRemittance);
                }

                totalCount = LINEPayRemittance.Count();

                LINEPayRemittance = LINEPayRemittance.Skip(pagination.From()).Take(pagination.To()).ToList();

                result = LINEPayRemittance;
                count = LINEPayRemittance.Count();
            }
            #endregion

            Response.Success = true;
            Response.Result = result;
            Response.ResultInfo = new
            {
                page = pagination.Page,
                perPage = pagination.PerPage,
                count = count,
                totalCount = totalCount
            };

            DB.Dispose();

            return Json(Response.Render());
        }

        private FileStreamResult ExportExcelShopDailyRevenue(DateTime dateFrom, DateTime dateTo, List<ShopDailyRevenue> Items)
        {
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;

            // Creating new workbook
            IWorkbook workbook = application.Workbooks.Create(new[] { "ShopDailyRevenue" });
            IWorksheet worksheet = workbook.Worksheets[0];

            workbook.Version = ExcelVersion.Excel2013;
            worksheet.Range["A1:A2"].Merge();
            worksheet.Range["B1:B2"].Merge();
            worksheet.Range["C1:H1"].Merge();
            worksheet.Range["I1:N1"].Merge();

            // Header First
            worksheet.Range["A1"].Text = "Branch";
            worksheet.Range["B1"].Text = "ERP_ID";
            worksheet.Range["C1"].Text = $"{dateTo.ToString("dd/MM/yyyy", enUS)} to {dateTo.ToString("dd/MM/yyyy", enUS)}";
            worksheet.Range["I1"].Text = $"As of {dateTo.ToString("dd/MM/yyyy", enUS)}";

            //worksheet.Range["A2"].Text = "BranchID";
            worksheet.Range["C2"].Text = "Sender";
            worksheet.Range["D2"].Text = "Con";
            worksheet.Range["E2"].Text = "Box";
            worksheet.Range["F2"].Text = "Revenue";
            worksheet.Range["G2"].Text = "YPC";
            worksheet.Range["H2"].Text = "YPB";
            worksheet.Range["I2"].Text = "Sender";
            worksheet.Range["J2"].Text = "Con";
            worksheet.Range["K2"].Text = "Box";
            worksheet.Range["L2"].Text = "Revenue";
            worksheet.Range["M2"].Text = "YPC";
            worksheet.Range["N2"].Text = "YPB";

            IStyle headerStyleBranch = workbook.Styles.Add("HeaderStyleBranch");
            headerStyleBranch.BeginUpdate();
            headerStyleBranch.Color = Color.FromArgb(224, 224, 224);
            headerStyleBranch.Font.Color = ExcelKnownColors.Black;
            headerStyleBranch.Font.Bold = true;
            headerStyleBranch.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            headerStyleBranch.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            headerStyleBranch.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(191, 191, 191);
            headerStyleBranch.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(191, 191, 191);
            headerStyleBranch.VerticalAlignment = ExcelVAlign.VAlignCenter;
            headerStyleBranch.EndUpdate();
            worksheet.Range["A1:B2"].CellStyle = headerStyleBranch;

            IStyle headerStyleMtd = workbook.Styles.Add("HeaderStyleMtd");
            headerStyleMtd.BeginUpdate();
            headerStyleMtd.Color = Color.FromArgb(187, 222, 251);
            headerStyleMtd.Font.Color = ExcelKnownColors.Black;
            headerStyleMtd.Font.Bold = true;
            headerStyleMtd.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            headerStyleMtd.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            headerStyleMtd.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(191, 191, 191);
            headerStyleMtd.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(191, 191, 191);
            headerStyleMtd.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            headerStyleMtd.VerticalAlignment = ExcelVAlign.VAlignCenter;
            headerStyleMtd.EndUpdate();
            worksheet.Range["C1:H2"].CellStyle = headerStyleMtd;

            IStyle headerStyleDaily = workbook.Styles.Add("HeaderStyleDaily");
            headerStyleDaily.BeginUpdate();
            headerStyleDaily.Color = Color.FromArgb(225, 248, 225);
            headerStyleDaily.Font.Color = ExcelKnownColors.Black;
            headerStyleDaily.Font.Bold = true;
            headerStyleDaily.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
            headerStyleDaily.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
            headerStyleDaily.Borders[ExcelBordersIndex.EdgeBottom].ColorRGB = Color.FromArgb(191, 191, 191);
            headerStyleDaily.Borders[ExcelBordersIndex.EdgeRight].ColorRGB = Color.FromArgb(191, 191, 191);
            headerStyleDaily.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            headerStyleDaily.VerticalAlignment = ExcelVAlign.VAlignCenter;
            headerStyleDaily.EndUpdate();
            worksheet.Range["I1:N2"].CellStyle = headerStyleDaily;

            worksheet.ImportData(Items, 3, 1, false);

            worksheet.Columns[1].CellStyle.NumberFormat = "#,##0";
            worksheet.Columns[2].CellStyle.NumberFormat = "#,##0";
            worksheet.Columns[3].CellStyle.NumberFormat = "#,##0";
            worksheet.Columns[4].CellStyle.NumberFormat = "#,##0.00";
            worksheet.Columns[5].CellStyle.NumberFormat = "#,##0.00";
            worksheet.Columns[6].CellStyle.NumberFormat = "#,##0.00";
            worksheet.Columns[7].CellStyle.NumberFormat = "#,##0";
            worksheet.Columns[8].CellStyle.NumberFormat = "#,##0";
            worksheet.Columns[9].CellStyle.NumberFormat = "#,##0";
            worksheet.Columns[10].CellStyle.NumberFormat = "#,##0.00";
            worksheet.Columns[11].CellStyle.NumberFormat = "#,##0.00";
            worksheet.Columns[12].CellStyle.NumberFormat = "#,##0.00";

            worksheet.Range["C3"].FreezePanes();

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, "Application/msexcel", "KE_PDC_ShopDailyRevenue_Report_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".xlsx");
        }

        private FileStreamResult ExportExcelReceipt(List<Receipt> Items, DateTime DateFrom, DateTime DateTo)
        {
            // Load the Excel Template
            Stream xlsxStream = System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + @"\assets\templates\Receipt.xlsx");

            ExcelEngine excelEngine = new ExcelEngine();

            // Loads or open an existing workbook through Open method of IWorkbooks
            IWorkbook workbook = excelEngine.Excel.Workbooks.Open(xlsxStream);

            workbook.Version = ExcelVersion.Excel2013;

            // Sheet #1
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.Range["D2"].Text = $"{DateFrom.ToString("dd/MM/yyyy")} - {DateTo.ToString("dd/MM/yyyy")}";

            worksheet.ImportData(Items.Select(i => new
            {
                i.ERP_ID,
                i.BranchID,
                i.ReceiptDate.Value.Date,
                i.ReceiptDate.Value.TimeOfDay,
                i.ReceiptBranchNo,
                i.BranchName,
                i.BranchType,
                i.ReceiptNo,
                i.CustomerName,
                i.CustomerTaxID,
                i.CustomerIsHQ,
                i.CustomerBranchName,
                i.Amount,
                i.Canceled
            }), 4, 1, false);

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, "Application/msexcel", "KE_PDC_Receipt_Report_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".xlsx");
        }

        private FileStreamResult ExportExcelTaxInvoice(List<TaxInvoice> Items, DateTime DateFrom, DateTime DateTo)
        {
            // Load the Excel Template
            Stream xlsxStream = System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + @"\assets\templates\TaxInvoice.xlsx");

            ExcelEngine excelEngine = new ExcelEngine();

            // Loads or open an existing workbook through Open method of IWorkbooks
            IWorkbook workbook = excelEngine.Excel.Workbooks.Open(xlsxStream);

            workbook.Version = ExcelVersion.Excel2013;

            // Sheet #1
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.Range["D2"].Text = $"{DateFrom.ToString("dd/MM/yyyy")} - {DateTo.ToString("dd/MM/yyyy")}";

            worksheet.ImportData(Items.Select(i => new
            {
                i.ERP_ID,
                i.BranchID,
                i.ReceiptDate.Value.Date,
                i.ReceiptDate.Value.TimeOfDay,
                i.ReceiptBranchNo,
                i.BranchName,
                i.BranchType,
                i.TaxInvoiceNo,
                i.CustomerName,
                i.CustomerTaxID,
                i.CustomerIsHQ,
                i.CustomerBranchName,
                i.Amount,
                i.Vat,
                i.Total,
                i.Canceled
            }), 4, 1, false);

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, "Application/msexcel", "KE_PDC_TaxInvoice_Report_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".xlsx");
        }

        private FileStreamResult ExportExcelMonthlyCommissions(List<DailyCommission> DailyCommission, List<MonthlyCommission> MonthlyCommission)
        {
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;

            // Creating new workbook
            IWorkbook workbook = application.Workbooks.Create(2);
            workbook.Version = ExcelVersion.Excel2013;

            // Header Style
            IStyle headerStyle = AddHeaderStyle(workbook);

            //Creating a Sheet with name "DailyCommission"
            IWorksheet worksheetDailyCommission = workbook.Worksheets[0];

            worksheetDailyCommission.Name = "DailyCommission";

            worksheetDailyCommission.Range["A1"].Text = "BranchID";
            worksheetDailyCommission.Range["B1"].Text = "ERP_ID";
            worksheetDailyCommission.Range["C1"].Text = "ReportDate";
            worksheetDailyCommission.Range["D1"].Text = "Boxes";
            worksheetDailyCommission.Range["E1"].Text = "TotalRevenue";
            worksheetDailyCommission.Range["F1"].Text = "Package";
            worksheetDailyCommission.Range["G1"].Text = "SalesPackage";
            worksheetDailyCommission.Range["H1"].Text = "CODSurcharge";
            worksheetDailyCommission.Range["I1"].Text = "InsurSurcharge";
            worksheetDailyCommission.Range["J1"].Text = "FreightRevenue";
            worksheetDailyCommission.Range["K1"].Text = "DHL";
            worksheetDailyCommission.Range["L1"].Text = "BSD";

            worksheetDailyCommission.Rows[0].CellStyle = headerStyle;

            worksheetDailyCommission.ImportData(DailyCommission.Select(dc => new {
                dc.BranchID,
                dc.erp_id,
                dc.ReportDate,
                dc.Boxes,
                dc.TotalRevenue,
                dc.Package,
                dc.SalesPackage,
                dc.CODSurcharge,
                dc.InsurSurcharge,
                dc.FreightRevenue,
                dc.DHL,
                dc.BSD,
            }), 2, 1, false);

            worksheetDailyCommission.Columns[2].CellStyle.NumberFormat = "#,##0";
            for(int i = 3; i < worksheetDailyCommission.Columns.Count(); i++)
            {
                worksheetDailyCommission.Columns[i].CellStyle.NumberFormat = "#,##0.00";
            }

            worksheetDailyCommission.Rows[1].FreezePanes();

            //Creating a Sheet with name "DailyCommission"
            IWorksheet worksheetMonthlyCommission = workbook.Worksheets[1];

            worksheetMonthlyCommission.Name = "MonthlyCommission";

            worksheetMonthlyCommission.Range["A1"].Text = "BranchID";
            worksheetMonthlyCommission.Range["B1"].Text = "ERP_ID";
            worksheetMonthlyCommission.Range["C1"].Text = "Boxes";
            worksheetMonthlyCommission.Range["D1"].Text = "TotalRevenue";
            worksheetMonthlyCommission.Range["E1"].Text = "Package";
            worksheetMonthlyCommission.Range["F1"].Text = "SalesPackage";
            worksheetMonthlyCommission.Range["G1"].Text = "CODSurcharge";
            worksheetMonthlyCommission.Range["H1"].Text = "InsurSurcharge";
            worksheetMonthlyCommission.Range["I1"].Text = "FreightRevenue";
            worksheetMonthlyCommission.Range["J1"].Text = "DHL";
            worksheetMonthlyCommission.Range["K1"].Text = "BSD";

            worksheetMonthlyCommission.Rows[0].CellStyle = headerStyle;

            worksheetMonthlyCommission.ImportData(MonthlyCommission, 2, 1, false);

            worksheetMonthlyCommission.Columns[1].CellStyle.NumberFormat = "#,##0";
            for (int i = 2; i < worksheetMonthlyCommission.Columns.Count(); i++)
            {
                worksheetMonthlyCommission.Columns[i].CellStyle.NumberFormat = "#,##0.00";
            }

            worksheetMonthlyCommission.Rows[1].FreezePanes();

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, "Application/msexcel", "KE_PDC_Commission_Report_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".xlsx");
        }

        private FileStreamResult ExportExcelLINEPay(List<LINEPay> LINEPay, List<LINETopUp> LINETopUp)
        {
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;

            // Creating new workbook
            IWorkbook workbook = application.Workbooks.Create(2);
            workbook.Version = ExcelVersion.Excel2013;

            // Header Style
            IStyle headerStyle = AddHeaderStyle(workbook);

            //Creating a Sheet with name "LINEPay"
            IWorksheet worksheetLINEPay = workbook.Worksheets[0];

            worksheetLINEPay.Name = "LINEPay";

            worksheetLINEPay.Range["A1"].Text = "No";
            worksheetLINEPay.Range["B1"].Text = "Transition Number";
            worksheetLINEPay.Range["C1"].Text = "Payment Type";
            worksheetLINEPay.Range["D1"].Text = "Amount";
            worksheetLINEPay.Range["E1"].Text = "Status";
            worksheetLINEPay.Range["F1"].Text = "Order No";
            //worksheetLINEPay.Range["G1"].Text = "Ref No";
            worksheetLINEPay.Range["G1"].Text = "Branch";

            worksheetLINEPay.Rows[0].CellStyle = headerStyle;

            worksheetLINEPay.ImportData(LINEPay, 2, 1, false);

            worksheetLINEPay.Columns[3].CellStyle.NumberFormat = "#,##0.00";

            if(LINEPay.Count() > 0) worksheetLINEPay.Rows[1].FreezePanes();

            //Creating a Sheet with name "LINETopUp"
            IWorksheet worksheetLINETopUp = workbook.Worksheets[1];

            worksheetLINETopUp.Name = "LINETopUp";

            worksheetLINETopUp.Range["A1"].Text = "No";
            worksheetLINETopUp.Range["B1"].Text = "Transition Number";
            worksheetLINETopUp.Range["C1"].Text = "Payment Type";
            worksheetLINETopUp.Range["D1"].Text = "Amount";
            worksheetLINETopUp.Range["E1"].Text = "Status";
            worksheetLINETopUp.Range["F1"].Text = "Order No";
            //worksheetLINETopUp.Range["G1"].Text = "Ref No";
            worksheetLINETopUp.Range["G1"].Text = "Branch";
            worksheetLINETopUp.Range["H1"].Text = "Return Code";

            worksheetLINETopUp.Rows[0].CellStyle = headerStyle;

            worksheetLINETopUp.ImportData(LINETopUp, 2, 1, false);

            worksheetLINETopUp.Columns[3].CellStyle.NumberFormat = "#,##0.00";

            if(LINETopUp.Count() > 0) worksheetLINETopUp.Rows[1].FreezePanes();

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, "Application/msexcel", "KE_PDC_LINE_Report_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".xlsx");
        }

        private FileStreamResult ExportExcelLINEPayRemittance(List<LINEPayRemittance> LINEPayRemittance)
        {
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;

            // Creating new workbook
            IWorkbook workbook = application.Workbooks.Create(new[] { "LINEPayRemittance" });
            IWorksheet worksheet = workbook.Worksheets[0];

            workbook.Version = ExcelVersion.Excel2013;

            worksheet.Range["A1"].Text = "BranchID";
            worksheet.Range["B1"].Text = "DMSID";
            worksheet.Range["C1"].Text = "Branch Type";
            worksheet.Range["D1"].Text = "ERP ID";
            worksheet.Range["E1"].Text = "BranchName";
            worksheet.Range["F1"].Text = "TUC DCSP";
            worksheet.Range["G1"].Text = "TUC";
            worksheet.Range["H1"].Text = "TUP";
            worksheet.Range["I1"].Text = "TUD";
            worksheet.Range["J1"].Text = "Captured";
            worksheet.Range["K1"].Text = "CapturedBy";
            worksheet.Range["L1"].Text = "CapturedDate";
            worksheet.Range["M1"].Text = "ReportDate";

            // Header Style
            IStyle headerStyle = AddHeaderStyle(workbook);

            worksheet.Rows[0].CellStyle = headerStyle;

            worksheet.ImportData(LINEPayRemittance.Select(l => new {
                l.BranchID,
                l.DMSID,
                l.branch_type,
                l.ERP_ID,
                l.BranchName,
                TUCDCSP = l.branch_type.Equals("DCSP-SHOP") ? l.TUC : 0,
                TUC = !l.branch_type.Equals("DCSP-SHOP") ? l.TUC : 0,
                l.TUP,
                l.TUD,
                Captured = l.TUDVerifyDate == null ? l.Captured : "Yes",
                CapturedBy = string.IsNullOrEmpty(l.TUDVerifyBy) ? l.CapturedBy : l.TUDVerifyBy,
                CapturedDate = l.TUDVerifyDate == null ? (l.CapturedDate == null ? null : l.CapturedDate.Value.ToString("dd/MM/yyyy", enUS)) : l.TUDVerifyDate.Value.ToString("dd/MM/yyyy", enUS),
                ReportDate = l.ReportDate.ToString("dd/MM/yyyy", enUS),
            }), 2, 1, false);

            worksheet.Rows[1].FreezePanes();

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, "Application/msexcel", "KE_PDC_LINEPayRemittance_Report_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".xlsx");
        }

        private ActionResult ExportPdfLINEPayRemittance(List<LINEPayRemittance> LINEPayRemittance, DateTime RemittanceDate)
        {
            // Load the PDF Template
            Stream pdfStream = System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + @"\assets\templates\LINEPayRemittance.pdf");

            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, (float)6.5);
            PdfFont fontRemittanceDate = new PdfStandardFont(PdfFontFamily.Helvetica, (float)8);

            // Load a PDF document.
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(pdfStream);

            //Create a new PDF document.
            PdfDocument pdfDocument = new PdfDocument();

            pdfDocument.ImportPage(loadedDocument, 0);

            PdfPage pdfPage = pdfDocument.Pages[0];
            
            //Create a new PdfGrid.
            PdfGrid pdfGrid = new PdfGrid();

            //Add three columns.
            pdfGrid.Columns.Add(9);

            //Add header.
            pdfGrid.Headers.Add(1);

            PdfGridRow pdfGridHeader = pdfGrid.Headers[0];

            string[] headerStr = { "ERP ID", "Branch ID", "Report Date", "Branch Name", "TUC DCSP", "TUC", "TUP", "TUD", "Captured" };

            int[] columnsWidth = { 40, 40, 50, 0, 50, 50, 50, 50, 30 };
            pdfGridHeader.Style.Font = font;
            pdfGridHeader.Style.BackgroundBrush = PdfBrushes.LightGray;
            pdfGridHeader.Height = (float)11;

            for (int i = 0; i < headerStr.Count(); i++)
            {
                if(!i.Equals(3))
                {
                    pdfGrid.Columns[i].Width = columnsWidth[i];
                }

                pdfGridHeader.Cells[i].StringFormat.Alignment = PdfTextAlignment.Center;

                pdfGridHeader.Cells[i].Value = headerStr[i];
                pdfGridHeader.Cells[i].Style.CellPadding = new PdfPaddings((float)1.5, (float)1.5, (float)1.5, (float)1.5);
                pdfGridHeader.Cells[i].Style.Borders.Left.Width = (float)0.5;
                pdfGridHeader.Cells[i].Style.Borders.Right.Width = (float)0.5;
                pdfGridHeader.Cells[i].Style.Borders.Top.Width = (float)0.5;
                pdfGridHeader.Cells[i].Style.Borders.Bottom.Width = (float)0.5;
            }

            pdfGridHeader.Cells[4].StringFormat.Alignment = PdfTextAlignment.Right;
            pdfGridHeader.Cells[5].StringFormat.Alignment = PdfTextAlignment.Right;
            pdfGridHeader.Cells[6].StringFormat.Alignment = PdfTextAlignment.Right;

            PdfGridRow pdfGridRow;

            LINEPayRemittance.ForEach(line => {
                //Add rows.
                pdfGridRow = pdfGrid.Rows.Add();
                pdfGridRow.Style.Font = font;

                pdfGridRow.Height = (float)11;

                pdfGridRow.Cells[0].Value = line.ERP_ID;
                pdfGridRow.Cells[1].Value = line.BranchID;
                pdfGridRow.Cells[2].Value = line.ReportDate.ToString("dd/MM/yyyy", enUS);
                pdfGridRow.Cells[3].Value = line.BranchName;
                pdfGridRow.Cells[4].Value = line.branch_type.Equals("DCSP-SHOP") ? line.TUC.ToString("#,0.00"):"-";
                pdfGridRow.Cells[5].Value = !line.branch_type.Equals("DCSP-SHOP") ? line.TUC.ToString("#,0.00") : "-";
                pdfGridRow.Cells[6].Value = line.TUP.ToString("#,0.00");
                pdfGridRow.Cells[7].Value = line.TUD.ToString("#,0.00");
                pdfGridRow.Cells[8].Value = line.TUDVerifyDate == null ? line.Captured : "Yes";

                pdfGridRow.Cells[0].StringFormat.Alignment = PdfTextAlignment.Center;
                pdfGridRow.Cells[2].StringFormat.Alignment = PdfTextAlignment.Center;
                pdfGridRow.Cells[4].StringFormat.Alignment = PdfTextAlignment.Right;
                pdfGridRow.Cells[5].StringFormat.Alignment = PdfTextAlignment.Right;
                pdfGridRow.Cells[6].StringFormat.Alignment = PdfTextAlignment.Right;
                pdfGridRow.Cells[7].StringFormat.Alignment = PdfTextAlignment.Right;
                pdfGridRow.Cells[8].StringFormat.Alignment = PdfTextAlignment.Center;

                for (int i = 0; i < pdfGridRow.Cells.Count; i++)
                {
                    pdfGridRow.Cells[i].Style.Borders.Left.Width = (float)0.5;
                    pdfGridRow.Cells[i].Style.Borders.Right.Width = (float)0.5;
                    pdfGridRow.Cells[i].Style.Borders.Top.Width = (float)0.5;
                    pdfGridRow.Cells[i].Style.Borders.Bottom.Width = (float)0.5;

                    pdfGridRow.Cells[i].Style.CellPadding = new PdfPaddings((float)1.5, (float)1.5, (float)1.5, (float)1.5);
                }
            });

            //Add rows Sum
            pdfGridRow = pdfGrid.Rows.Add();
            pdfGridRow.Cells[4].Value = LINEPayRemittance.Where(line => line.branch_type.Equals("DCSP-SHOP")).Sum(line => line.TUC).ToString("#,0.00");
            pdfGridRow.Cells[5].Value = LINEPayRemittance.Where(line => !line.branch_type.Equals("DCSP-SHOP")).Sum(line => line.TUC).ToString("#,0.00");
            pdfGridRow.Cells[6].Value = LINEPayRemittance.Sum(line => line.TUP).ToString("#,0.00");
            pdfGridRow.Cells[7].Value = LINEPayRemittance.Sum(line => line.TUD).ToString("#,0.00");
            pdfGridRow.Cells[4].Style.CellPadding = new PdfPaddings((float)1.5, (float)1.5, (float)1.5, (float)1.5);
            pdfGridRow.Cells[5].Style.CellPadding = new PdfPaddings((float)1.5, (float)1.5, (float)1.5, (float)1.5);
            pdfGridRow.Cells[6].Style.CellPadding = new PdfPaddings((float)1.5, (float)1.5, (float)1.5, (float)1.5);
            pdfGridRow.Cells[7].Style.CellPadding = new PdfPaddings((float)1.5, (float)1.5, (float)1.5, (float)1.5);

            for (int i = 0; i < pdfGridRow.Cells.Count; i++)
            {
                pdfGridRow.Cells[i].Style.Borders.Left.Color = PdfColor.Empty;
                pdfGridRow.Cells[i].Style.Borders.Right.Color = PdfColor.Empty;
                pdfGridRow.Cells[i].Style.Borders.Bottom.Color = PdfColor.Empty;
                pdfGridRow.Cells[i].Style.Borders.Left.Width = (float)0.5;
                pdfGridRow.Cells[i].Style.Borders.Right.Width = (float)0.5;
                pdfGridRow.Cells[i].Style.Borders.Top.Width = (float)0.5;
                pdfGridRow.Cells[i].Style.Borders.Bottom.Width = (float)0.5;
            }

            pdfGridRow.Cells[4].StringFormat.Alignment = PdfTextAlignment.Right;
            pdfGridRow.Cells[5].StringFormat.Alignment = PdfTextAlignment.Right;
            pdfGridRow.Cells[6].StringFormat.Alignment = PdfTextAlignment.Right;
            pdfGridRow.Cells[7].StringFormat.Alignment = PdfTextAlignment.Right;

            pdfGridRow.Height = (float)11;
            pdfGridRow.Style.Font = font;

            for (int i = 4; i <= 7; i++)
            {
                pdfGridRow.Cells[i].Style.Borders.Top.Color = new PdfColor(Color.Black);
                pdfGridRow.Cells[i].Style.Borders.Left.Color = new PdfColor(Color.Black);
                pdfGridRow.Cells[i].Style.Borders.Right.Color = new PdfColor(Color.Black);
                pdfGridRow.Cells[i].Style.Borders.Bottom.Color = new PdfColor(Color.Black);
            }

            //Add rows Total
            pdfGridRow = pdfGrid.Rows.Add();
            pdfGridRow.Cells[3].Value = "Total";
            pdfGridRow.Cells[4].Value = (
                LINEPayRemittance.Sum(line => line.TUC)
                +
                LINEPayRemittance.Sum(line => line.TUP)
                +
                LINEPayRemittance.Sum(line => line.TUD)).ToString("#,0.00");
            pdfGridRow.Cells[4].ColumnSpan = 4;
            pdfGridRow.Cells[3].Style.CellPadding = new PdfPaddings((float)1.5, (float)1.5, (float)1.5, (float)1.5);
            pdfGridRow.Cells[4].Style.CellPadding = new PdfPaddings((float)1.5, (float)1.5, (float)1.5, (float)1.5);

            pdfGridRow.Cells[3].StringFormat.Alignment = PdfTextAlignment.Right;
            pdfGridRow.Cells[4].StringFormat.Alignment = PdfTextAlignment.Center;

            pdfGridRow.Height = (float)11;
            pdfGridRow.Style.Font = font;

            for (int i = 0; i < pdfGridRow.Cells.Count; i++)
            {
                pdfGridRow.Cells[i].Style.Borders.Top.Color = PdfColor.Empty;
                pdfGridRow.Cells[i].Style.Borders.Left.Color = PdfColor.Empty;
                pdfGridRow.Cells[i].Style.Borders.Right.Color = PdfColor.Empty;
                pdfGridRow.Cells[i].Style.Borders.Bottom.Color = PdfColor.Empty;
            }

            pdfGridRow.Cells[4].Style.Borders.Top.Color = new PdfColor(Color.Black);
            pdfGridRow.Cells[4].Style.Borders.Left.Color = new PdfColor(Color.Black);
            pdfGridRow.Cells[4].Style.Borders.Right.Color = new PdfColor(Color.Black);
            pdfGridRow.Cells[4].Style.Borders.Bottom.Color = new PdfColor(Color.Black);
            pdfGridRow.Cells[4].Style.Borders.Left.Width = (float)0.5;
            pdfGridRow.Cells[4].Style.Borders.Right.Width = (float)0.5;
            pdfGridRow.Cells[4].Style.Borders.Top.Width = (float)0.5;
            pdfGridRow.Cells[4].Style.Borders.Bottom.Width = (float)0.5;


            //Create PDF graphics for the page.
            PdfGraphics graphics = pdfPage.Graphics;

            //Draw the text.
            graphics.DrawString(RemittanceDate.ToString("dd MMM yyyy", enUS), fontRemittanceDate, PdfBrushes.Black, new PointF((float)458, (float)57.5));

            //Draw the PdfGrid.
            //pdfGrid.Draw(pdfPage, (float)20, (float)90, (float)555.28,);
            //pdfGrid.Draw(pdfPage, new RectangleF(20, 90, pdfDocument.Pages[0].GetClientSize().Width - 40, pdfDocument.Pages[0].GetClientSize().Height - 140));

            //Set properties to paginate the table.
            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
            layoutFormat.Break = PdfLayoutBreakType.FitElement;
            layoutFormat.Layout = PdfLayoutType.Paginate;
            layoutFormat.PaginateBounds = new RectangleF(20, 20, pdfDocument.Pages[0].GetClientSize().Width - 40, pdfDocument.Pages[0].GetClientSize().Height - 50);

            //Draw PdfLightTable.
            pdfGrid.Draw(pdfPage, 20f, 90f, (pdfDocument.Pages[0].GetClientSize().Width - 40), layoutFormat);

            //Create a Page template that can be used as footer.
            RectangleF bounds = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 50);
            PdfPageTemplateElement footer = new PdfPageTemplateElement(bounds);
            PdfBrush brush = new PdfSolidBrush(Color.Black);

            //Create page number field.
            PdfPageNumberField pageNumber = new PdfPageNumberField(font, brush);

            //Create page count field.
            PdfPageCountField count = new PdfPageCountField(font, brush);

            //Add the fields in composite fields.
            PdfCompositeField compositeField = new PdfCompositeField(font, brush, "Page {0} of {1}", pageNumber, count);

            string printDate = DateTime.Now.ToString("dd MMM yyyy HH:mm:ss", enUS);
            PdfCompositeField compositePrintDate = new PdfCompositeField(font, brush, string.Format("Printed Date : {0}", printDate));

            compositeField.Bounds = footer.Bounds;

            //Draw the composite field in footer.
            compositeField.Draw(footer.Graphics, new PointF(pdfDocument.Pages[0].GetClientSize().Width - 50, 30));
            compositePrintDate.Draw(footer.Graphics, new PointF(20, 30));

            //Add the footer template at the bottom.
            pdfDocument.Template.Bottom = footer;




            MemoryStream ms = new MemoryStream();
            pdfDocument.Save(ms);
            ms.Position = 0;

            //Close the document
            pdfDocument.Close(true);

            //Save the document.
            return File(ms, "Application/pdf");
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
