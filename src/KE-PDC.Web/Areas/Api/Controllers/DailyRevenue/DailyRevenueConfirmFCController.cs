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
    public class DailyRevenueConfirmFCController : Controller
    {
        new ApiResponse Response = new ApiResponse();
        private readonly ILogger<DailyRevenueConfirmFCController> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        private KE_POSContext DB;
        private KE_PMGWContext DBPMGW;
        private CultureInfo enUS = new CultureInfo("en-US");

        public DailyRevenueConfirmFCController(KE_POSContext context, KE_PMGWContext PMGWcontext, ILogger<DailyRevenueConfirmFCController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            DB = context;
            DBPMGW = PMGWcontext;
        }

        // GET: /<controller>/
        [HttpGet]
        public async Task<ActionResult> Get(DailyRevenueConfirmFCViewModel Filter)
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

            sqlStringBuilder.Append("EXEC sp_PDC_Report_DailyRevenueConfirmFC_Get '");
            sqlStringBuilder.Append(Filter.ConfirmBy);
            sqlStringBuilder.Append("','");
            sqlStringBuilder.Append(UserData.Username);
            sqlStringBuilder.Append("','");
            sqlStringBuilder.Append(Filter.BranchList);
            sqlStringBuilder.Append("','");
            sqlStringBuilder.Append(dateFrom.ToString("yyyyMMdd", enUS));
            sqlStringBuilder.Append("','");
            sqlStringBuilder.Append(dateTo.ToString("yyyyMMdd", enUS));
            sqlStringBuilder.Append("'");

            string strSQL = sqlStringBuilder.ToString();

            List<DailyRevenueConfirmFC> DailyRevenues = await DB.DailyRevenueConfirmFC.FromSql(strSQL).ToListAsync();

            if (filetype.Equals("excel"))
            {
                return ExportExcelDailyRevenueConfirmFC(DailyRevenues);
            }

            int totalCount = DailyRevenues.Count();

            DailyRevenues = DailyRevenues.Skip(pagination.From()).Take(pagination.To()).ToList();

            Response.Success = true;
            Response.Result = DailyRevenues;
            Response.ResultInfo = new
            {
                page = pagination.Page,
                perPage = pagination.PerPage,
                count = DailyRevenues.Count(),
                totalCount
            };

            DB.Dispose();

            return Json(Response.Render());
        }

        private ActionResult ExportExcelDailyRevenueConfirmFC(List<DailyRevenueConfirmFC> Items)
        {
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;

            // Creating new workbook
            IWorkbook workbook = application.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            workbook.Version = ExcelVersion.Excel2013;

            worksheet.Range["A1"].Text = "Branch";
            worksheet.Range["B1"].Text = "Status";
            worksheet.Range["C1"].Text = "ReportDate";
            worksheet.Range["D1"].Text = "Freight";
            worksheet.Range["E1"].Text = "Transport";
            worksheet.Range["F1"].Text = "AM";
            worksheet.Range["G1"].Text = "PUP";
            worksheet.Range["H1"].Text = "SAT";
            worksheet.Range["I1"].Text = "RAS";
            worksheet.Range["J1"].Text = "COD";
            worksheet.Range["K1"].Text = "Insurance";
            worksheet.Range["L1"].Text = "Package";
            worksheet.Range["M1"].Text = "SalePackage";
            worksheet.Range["N1"].Text = "LineTopUp";
            worksheet.Range["O1"].Text = "Cash";
            worksheet.Range["P1"].Text = "Rabbit";
            worksheet.Range["Q1"].Text = "Credit";
            worksheet.Range["R1"].Text = "LinePay";
            worksheet.Range["S1"].Text = "Transportation";
            worksheet.Range["T1"].Text = "VASSurcharge";
            worksheet.Range["U1"].Text = "Discount";
            worksheet.Range["V1"].Text = "Vat";
            worksheet.Range["W1"].Text = "Total";
            worksheet.Range["X1"].Text = "City";
            worksheet.Range["Y1"].Text = "Cityn";
            worksheet.Range["Y1"].Text = "Citys";
            worksheet.Range["Z1"].Text = "Grab";
            worksheet.Range["AA1"].Text = "BSDCash";
            worksheet.Range["AB1"].Text = "BSDLinePay";
            worksheet.Range["AC1"].Text = "BSDLineTopUp";
            worksheet.Range["AD1"].Text = "TotalTransfer";
            worksheet.Range["AE1"].Text = "TotalCon";
            worksheet.Range["AF1"].Text = "TotalBoxes";
            worksheet.Range["AG1"].Text = "FCConfirmedDate";
            worksheet.Range["AH1"].Text = "FCConfirmedBy";
            // Header Style
            IStyle headerStyle = AddHeaderStyle(workbook);

            worksheet.Rows[0].CellStyle = headerStyle;

            worksheet.ImportData(Items.Select(d => new {
                d.BranchID,
                Status = d.FCConfirmed ? "Confirmed" : "",
                d.ReportDate,
                d.Freight,
                d.Transport,
                d.AM,
                d.PUP,
                d.SAT,
                d.RAS,
                d.COD,
                d.Insur,
                d.Pkg,
                d.SalePackage,
                d.LineTopUp,
                d.Cash,
                d.Rabbit,
                d.CreditBBL,
                d.CreditSCB,
                d.LinePay,
                d.Transportation,
                d.VASSurcharge,
                d.Discount,
                d.Vat,
                d.Total,
                d.City,
                d.Cityn,
                d.Citys,
                d.Grab,
                d.BSDCash,
                d.BSDLinePay,
                d.BSDLineTopUp,
                d.TotalTransfer,
                d.TotalCon,
                d.TotalBoxes,
                d.FCConfirmedDate,
                d.FCConfirmedBy,
            }), 2, 1, false);

            //worksheet.Columns[36].CellStyle.NumberFormat = "#,##0";
            //worksheet.Columns[37].CellStyle.NumberFormat = "#,##0";

            //for (int i = 3; i < worksheet.Columns.Count() - 6; i++)
            //{
            //    worksheet.Columns[i].CellStyle.NumberFormat = "#,##0.00";
            //}

            worksheet.Rows[1].FreezePanes();

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, "Application/msexcel", "KE_PDC_DailyRevenueConfirmFC_Report_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".xlsx");
        }

        // POST: /<controller>/
        [HttpPost]
        public async Task<ActionResult> Post()
        {
            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            string idList = HttpContext.Request.Form["IDs"].ToString().ToUpper();

            if(string.IsNullOrEmpty(idList))
            {
                Response.Success = false;
                Response.Errors.Add(new
                {
                    Code = 404,
                    Message = "Not Found"
                });
                HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(Response.Render());
            }

            string[] idLists = idList.Split(',');
            StringBuilder sqlStrBuilder = new StringBuilder();

            foreach (var x in idLists)
            {
                string[] xSplit = x.Split('-');
                sqlStrBuilder.Append("EXEC sp_RPT301_SaveDailyRevenueConfirmFC '");

                sqlStrBuilder.Append(xSplit[1]);
                sqlStrBuilder.Append("', '");
                sqlStrBuilder.Append(UserData.Username);
                sqlStrBuilder.Append("', '");
                sqlStrBuilder.Append(xSplit[0]);

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

            //Response.Result = adjustment;

            DB.Dispose();

            return Json(Response.Render());
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
