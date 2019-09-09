using KE_PDC.Models;
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
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KE_PDC.Areas.Api.Controllers
{
    [Route("Api/[controller]")]
    [Authorize]
    public class ParcelShopShopDailyController : Controller
    {
        new ApiResponse Response = new ApiResponse();
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<ParcelShopShopDailyController> _logger;
        private KE_POSContext DB;
        private CultureInfo enUS = new CultureInfo("en-US");

        public ParcelShopShopDailyController(KE_POSContext context, ILogger<ParcelShopShopDailyController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            DB = context;
        }

        // GET: /Api/<controller>
        [HttpGet]
        public async Task<ActionResult> Get(ShopDailyViewModel Filter)
        {
            if (!ModelState.IsValid)
            {
                return Json(Response.RenderError(ModelState));
            }

            // Parameter
            string type = HttpContext.Request.Query["Type"].ToString().ToLower();
            string filetype = HttpContext.Request.Query["FileType"].ToString().ToLower();

            Pagination pagination = new Pagination(HttpContext);

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DateTime date = DateTime.ParseExact(Filter.Date, "dd/MM/yyyy", new CultureInfo("en-US"));

            List<DashboardShopDaily> DashboardShopDaily = await DB.DashboardShopDaily
                .FromSql($"EXEC sp_RPT316_ShopDaily '{UserData.Username}', '{Filter.BranchList}', '{date.ToString("yyyyMMdd", new CultureInfo("en-US"))}'")
                .ToListAsync();

            Response.Success = true;
            Response.Result = DashboardShopDaily;

            DB.Dispose();

            return Json(Response.Render());
        }



        // GET: /Api/<controller>/AccountingExport
        [HttpGet("AccountingExport")]
        public async Task<ActionResult> AccountingExportAsync(ShopDailyViewModel Filter)
        {
            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DateTime date = DateTime.ParseExact(Filter.Date, "dd/MM/yyyy", new CultureInfo("en-US"));

            // Load the Excel Template
            Stream xlsxStream = System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + @"\assets\templates\ShopDailyAccounting.xlsx");

            ExcelEngine excelEngine = new ExcelEngine();

            // Loads or open an existing workbook through Open method of IWorkbooks
            IWorkbook workbook = excelEngine.Excel.Workbooks.Open(xlsxStream);

            workbook.Version = ExcelVersion.Excel2013;

            List<CashReport> cashReport = await DB.CashReport.FromSql($"sp_RPT316_ShopDailyAccounting '{UserData.Username}', '{Filter.BranchList}', '{date.ToString("yyyyMMdd", new CultureInfo("en-US"))}', 1").ToListAsync();
            List<Invoice> InvoiceMTD = await DB.Invoice.FromSql($"sp_RPT316_ShopDailyAccounting '{UserData.Username}', '{Filter.BranchList}', '{date.ToString("yyyyMMdd", new CultureInfo("en-US"))}', 2").ToListAsync();
            List<LinePayInvoice> linePayInvoiceDaily = await DB.LinePayInvoice.FromSql($"sp_RPT316_ShopDailyAccounting '{UserData.Username}', '{Filter.BranchList}', '{date.ToString("yyyyMMdd", new CultureInfo("en-US"))}', 3").ToListAsync();
            List<LinePayInvoice> linePayInvoiceMTD = await DB.LinePayInvoice.FromSql($"sp_RPT316_ShopDailyAccounting '{UserData.Username}', '{Filter.BranchList}', '{date.ToString("yyyyMMdd", new CultureInfo("en-US"))}', 4").ToListAsync();
            List<LinePayTopup> lineTopUp = await DB.LinePayTopup.FromSql($"sp_RPT316_ShopDailyAccounting '{UserData.Username}', '{Filter.BranchList}', '{date.ToString("yyyyMMdd", new CultureInfo("en-US"))}', 5").ToListAsync();
            List<BoxRevenue> boxRevenue = await DB.BoxRevenue.FromSql($"sp_RPT316_ShopDailyAccounting '{UserData.Username}', '{Filter.BranchList}', '{date.ToString("yyyyMMdd", new CultureInfo("en-US"))}', 6").ToListAsync();

            workbook.Worksheets[0].Range["B2"].Text = $"Cash Report {date.ToString("dd/MM/yyyy")}";
            workbook.Worksheets[0].ImportData(cashReport, 5, 1, false);

            workbook.Worksheets[1].Range["B2"].Text = $"Invoice 01/{date.ToString("MM/yyyy")} - {date.ToString("dd/MM/yyyy")}";
            workbook.Worksheets[1].ImportData(InvoiceMTD, 4, 1, false);

            workbook.Worksheets[2].Range["D2"].Text = $"LinePay Invoice {date.ToString("dd/MM/yyyy")}";
            workbook.Worksheets[2].ImportData(linePayInvoiceDaily, 4, 1, false);

            workbook.Worksheets[3].Range["D2"].Text = $"LinePay Invoice 01/{date.ToString("MM/yyyy")} - {date.ToString("dd/MMMM/yyyy")}";
            workbook.Worksheets[3].ImportData(linePayInvoiceMTD, 4, 1, false);

            workbook.Worksheets[4].Range["B2"].Text = $"LinePay Topup {date.ToString("dd/MM/yyyy")}";
            workbook.Worksheets[4].ImportData(lineTopUp, 4, 1, false);

            workbook.Worksheets[5].Range["C2"].Text = $"Box Revenue {date.ToString("dd/MM/yyyy")}";
            workbook.Worksheets[5].ImportData(boxRevenue, 4, 1, false);

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            // Close the instance of IWorkbook.
            workbook.Close();

            // Dispose the instance of ExcelEngine.
            excelEngine.Dispose();

            xlsxStream.Dispose();

            //return File(ms, "Application/msexcel", "KE_PDC_MonthlyCommission" + type.ToUpper() + "_Report_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".xlsx");
            return File(ms, "Application/msexcel", $"KE_PDC_ShopDaily_Accounting_Report_{date.ToString("dd_MMM_yyyy")}_{DateTime.Now.ToString("yyyMMdd_HHmmss")}.xlsx");
        }



        // GET: /Api/<controller>/MDEExport
        [HttpGet("MDEExport")]
        public async Task<ActionResult> MDEExportAsync(ShopDailyViewModel Filter)
        {
            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DateTime date = DateTime.ParseExact(Filter.Date, "dd/MM/yyyy", new CultureInfo("en-US"));

            // Load the Excel Template
            Stream xlsxStream = System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + @"\assets\templates\ShopDailyMDE.xlsx");

            ExcelEngine excelEngine = new ExcelEngine();

            // Loads or open an existing workbook through Open method of IWorkbooks
            IWorkbook workbook = excelEngine.Excel.Workbooks.Open(xlsxStream);

            workbook.Version = ExcelVersion.Excel2013;

            List<MDEConsignments> mdeConsignments = await DB.MDEConsignments.FromSql($"sp_RPT316_ShopDailyMDE '{UserData.Username}', '{Filter.BranchList}', '{date.ToString("yyyyMMdd", new CultureInfo("en-US"))}', 1").ToListAsync();
            List<MDEPackages> mdePackages = await DB.MDEPackages.FromSql($"sp_RPT316_ShopDailyMDE '{UserData.Username}', '{Filter.BranchList}', '{date.ToString("yyyyMMdd", new CultureInfo("en-US"))}', 2").ToListAsync();

            workbook.Worksheets[0].Range["B2"].Text = $"Manifest Data Entry (MDE) Consignments {date.ToString("dd/MM/yyyy")}";
            workbook.Worksheets[0].ImportData(mdeConsignments, 4, 1, false);

            workbook.Worksheets[1].Range["B2"].Text = $"Manifest Data Entry (MDE) Packages {date.ToString("dd/MM/yyyy")}";
            workbook.Worksheets[1].ImportData(mdePackages, 4, 1, false);

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            // Close the instance of IWorkbook.
            workbook.Close();

            // Dispose the instance of ExcelEngine.
            excelEngine.Dispose();

            xlsxStream.Dispose();

            //return File(ms, "Application/msexcel", "KE_PDC_MonthlyCommission" + type.ToUpper() + "_Report_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".xlsx");
            return File(ms, "Application/msexcel", $"KE_PDC_ShopDaily_MDE_Report_{date.ToString("dd_MMM_yyyy")}_{DateTime.Now.ToString("yyyMMdd_HHmmss")}.xlsx");
        }
    }
}
