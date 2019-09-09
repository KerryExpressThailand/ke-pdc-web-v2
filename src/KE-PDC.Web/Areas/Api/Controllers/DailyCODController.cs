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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KE_PDC.Areas.Api.Controllers
{
    [Route("Api/[controller]")]
    [Authorize]
    public class DailyCODController : Controller
    {
        new ApiResponse Response = new ApiResponse();
        private readonly ILogger<DailyCODController> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        private KE_POSContext DB;
        private KE_PMGWContext DBPMGW;
        private CultureInfo enUS = new CultureInfo("en-US");

        public DailyCODController(KE_POSContext context, KE_PMGWContext PMGWcontext, ILogger<DailyCODController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            DB = context;
            DBPMGW = PMGWcontext;
        }

        // GET: /<controller>/
        [HttpGet]
        public async Task<ActionResult> Get(TypeBranchDateFromDateToViewModel Filter)
        {
            if (!ModelState.IsValid)
            {
                return Json(Response.RenderError(ModelState));
            }

            // Parameter
            string filetype = HttpContext.Request.Query["FileType"].ToString().ToLower();

            Pagination pagination = new Pagination(HttpContext);

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DateTime dateFrom = DateTime.ParseExact(Filter.DateFrom, "dd/MM/yyyy", enUS);
            DateTime dateTo = DateTime.ParseExact(Filter.DateTo, "dd/MM/yyyy", enUS);

            StringBuilder sqlStringBuilder = new StringBuilder();

            sqlStringBuilder.Append("EXEC sp_RPT314_DailyCOD '");
            sqlStringBuilder.Append(dateFrom.ToString("yyyyMMdd", enUS));
            sqlStringBuilder.Append("','");
            sqlStringBuilder.Append(dateTo.ToString("yyyyMMdd", enUS));
            sqlStringBuilder.Append("','");
            sqlStringBuilder.Append(UserData.Username);
            sqlStringBuilder.Append("','");
            sqlStringBuilder.Append(Filter.BranchList);
            sqlStringBuilder.Append("'");

            string strSQL = sqlStringBuilder.ToString();

            List<DailyCOD> DailyCOD = await DB.DailyCOD.FromSql(strSQL).ToListAsync();

            if (filetype.Equals("excel"))
            {
                return ExportExcelDailyCOD(dateFrom, dateTo, DailyCOD);
            }

            int totalCount = DailyCOD.Count();

            DailyCOD = DailyCOD.Skip(pagination.From()).Take(pagination.To()).ToList();

            Response.Success = true;
            Response.Result = DailyCOD;
            Response.ResultInfo = new
            {
                page = pagination.Page,
                perPage = pagination.PerPage,
                count = DailyCOD.Count(),
                totalCount = totalCount
            };

            DB.Dispose();

            return Json(Response.Render());
        }

        // GET Api/<controller>/ASK
        [HttpGet("{id}")]
        public async Task<ActionResult> Detail(TypeBranchDateFromDateToViewModel Filter, string id)
        {
            // Parameter
            string filetype = HttpContext.Request.Query["FileType"].ToString().ToLower();

            Pagination pagination = new Pagination(HttpContext);

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DateTime dateFrom = DateTime.ParseExact(Filter.DateFrom, "dd/MM/yyyy", enUS);
            DateTime dateTo = DateTime.ParseExact(Filter.DateTo, "dd/MM/yyyy", enUS);

            List<DailyCODDetail> DailyCODDetail = await DB.DailyCODDetail.FromSql($"EXEC sp_RPT314_DailyCODDetail '{UserData.Username}', '{id}', '{dateFrom.ToString("yyyyMMdd", enUS)}', '{dateTo.ToString("yyyyMMdd", enUS)}'--, {pagination.From()}, {pagination.To()}").ToListAsync();

            if (filetype.Equals("excel"))
            {
                return ExportExcelDailyCODDetail(id, dateFrom, dateTo, DailyCODDetail);
            }

            int totalCount = DailyCODDetail.Count();

            DailyCODDetail = DailyCODDetail.Skip(pagination.From()).Take(pagination.To()).ToList();

            Response.Success = true;
            Response.Result = DailyCODDetail;
            Response.ResultInfo = new
            {
                page = pagination.Page,
                perPage = pagination.PerPage,
                count = DailyCODDetail.Count(),
                totalCount = totalCount
            };

            DB.Dispose();

            return Json(Response.Render());
        }

        private ActionResult ExportExcelDailyCOD(DateTime dateFrom, DateTime dateTo, List<DailyCOD> Items)
        {
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;

            // Creating new workbook
            IWorkbook workbook = application.Workbooks.Create(new[] { "Daily COD" });
            IWorksheet worksheet = workbook.Worksheets[0];

            workbook.Version = ExcelVersion.Excel2013;
            worksheet.Range["A1:A2"].Merge();
            worksheet.Range["B1:B2"].Merge();
            worksheet.Range["C1:F1"].Merge();
            worksheet.Range["G1:J1"].Merge();

            // Header First
            worksheet.Range["A1"].Text = "Branch";
            worksheet.Range["B1"].Text = "ERP_ID";
            worksheet.Range["C1"].Text = $"{dateTo.ToString("dd/MM/yyyy", enUS)} to {dateTo.ToString("dd/MM/yyyy", enUS)}";
            worksheet.Range["G1"].Text = $"As of {dateTo.ToString("dd/MM/yyyy", enUS)}";

            worksheet.Range["C2"].Text = "Consignment";
            worksheet.Range["D2"].Text = "Amount";
            worksheet.Range["E2"].Text = "Surcharge";
            worksheet.Range["F2"].Text = "Amount/Con";

            worksheet.Range["G2"].Text = "Consignment";
            worksheet.Range["H2"].Text = "amount";
            worksheet.Range["I2"].Text = "Surcharge";
            worksheet.Range["J2"].Text = "Amount/Con";

            // Header Style
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
            worksheet.Range["C1:F1"].CellStyle = headerStyleMtd;

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
            worksheet.Range["G1:J1"].CellStyle = headerStyleDaily;

            IStyle headerStyle = AddHeaderStyle(workbook);

            worksheet.Range["C2:J2"].CellStyle = headerStyle;
            // Header Style

            worksheet.ImportData(Items.Select(d => new {
                d.BranchID,
                d.ERPID,
                d.MonthlyCODConsignment,
                d.MonthlyCODAmount,
                d.MonthlyCODSurcharge,
                d.MonthlyAmountPerConsignment,
                d.DailyCODConsignment,
                d.DailyCODAmount,
                d.DailyCODSurcharge,
                d.DailyAmountPerConsignment,
            }), 3, 1, false);

            worksheet.Columns[1].CellStyle.NumberFormat = "#,##0";
            worksheet.Columns[5].CellStyle.NumberFormat = "#,##0";

            for (int i = 2; i < 5; i++)
            {
                worksheet.Columns[i].CellStyle.NumberFormat = "#,##0.00";
            }

            for (int i = 6; i < 9; i++)
            {
                worksheet.Columns[i].CellStyle.NumberFormat = "#,##0.00";
            }

            worksheet.Range["C3"].FreezePanes();

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, "Application/msexcel", "KE_PDC_DailyCOD_Report_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".xlsx");
        }

        private ActionResult ExportExcelDailyCODDetail(string BranchID, DateTime dateFrom, DateTime dateTo, List<DailyCODDetail> Items)
        {
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;

            // Creating new workbook
            IWorkbook workbook = application.Workbooks.Create(new[] { $"Daily COD Detail" });
            IWorksheet worksheet = workbook.Worksheets[0];

            workbook.Version = ExcelVersion.Excel2013;

            worksheet.Range["A1:F1"].Merge();
            worksheet.Range["A1"].Text = $"Branch #{BranchID} ({dateFrom.ToString("dd/MM/yyyy", enUS)} to {dateTo.ToString("dd/MM/yyyy", enUS)})";
            worksheet.Range["A1"].RowHeight = 25;
            worksheet.Range["A1"].CellStyle.VerticalAlignment = ExcelVAlign.VAlignCenter;
            worksheet.Range["A1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            worksheet.Range["A1"].CellStyle.Font.Size = 11;
            worksheet.Range["A1"].CellStyle.Font.Bold = true;

            worksheet.Range["A2"].Text = "Branch ID";
            worksheet.Range["B2"].Text = "Consignment";
            worksheet.Range["C2"].Text = "Account No";
            worksheet.Range["D2"].Text = "COD Amount";
            worksheet.Range["E2"].Text = "Month";
            worksheet.Range["F2"].Text = "Pickup Date";

            // Header Style
            IStyle headerStyle = AddHeaderStyle(workbook);

            worksheet.Range["A2:F2"].CellStyle = headerStyle;
            // Header Style

            worksheet.ImportData(Items.Select(d => new {
                d.BranchID,
                d.Consignment,
                d.AccountID,
                d.CODAmount,
                Month = d.PickupDate.ToString("MMMM", enUS),
                d.PickupDate,
            }), 3, 1, false);

            worksheet.Columns[3].CellStyle.NumberFormat = "#,##0.00";

            worksheet.Range["C3"].FreezePanes();

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, "Application/msexcel", "KE_PDC_DailyCODDetail_Report_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".xlsx");
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
