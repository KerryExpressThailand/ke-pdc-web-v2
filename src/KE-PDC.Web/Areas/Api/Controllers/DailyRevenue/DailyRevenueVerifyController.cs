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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KE_PDC.Areas.Api.Controllers
{
    [Route("Api/[controller]")]
    [Authorize]
    public class DailyRevenueVerifyController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<DailyRevenueVerifyController> _logger;
        new ApiResponse Response = new ApiResponse();
        private KE_POSContext DB;
        private CultureInfo enUS = new CultureInfo("en-US");

        public DailyRevenueVerifyController(KE_POSContext context, ILogger<DailyRevenueVerifyController> logger, IHostingEnvironment hostingEnvironment)
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

            Pagination pagination = new Pagination(HttpContext);

            // Auth Data
            string userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DateTime dateFrom = DateTime.ParseExact(Filter.DateFrom, "dd/MM/yyyy", enUS);
            DateTime dateTo = DateTime.ParseExact(Filter.DateTo, "dd/MM/yyyy", enUS);

            string parameter = "'" + dateFrom.ToString("yyyyMMdd", enUS) + "', '" + dateTo.ToString("yyyyMMdd", enUS) + "', '" + UserData.Username + "', '" + Filter.BranchList + "'";

            string sql = "EXEC sp_PDC_Report_DailyRevenueVerify_Get " + parameter;

            _logger.LogInformation(sql);

            IQueryable<DailyRevenueVerify> queryableDailyRevenueVerify = DB.DailyRevenueVerify.FromSql(sql);

            if (filetype.Equals("excel"))
            {
                //string exec = $"sp_PDC_Dashboard_EODCoverPage_Get '{dateFrom.ToString("yyyyMMdd", new CultureInfo("en-US"))}', '" + dateTo.ToString("yyyyMMdd", enUS) + "', '{UserData.Username}', '{filter.BranchList}'";
                string exec = $"sp_PDC_Dashboard_CloseShopCoverPage_Get '{dateFrom.ToString("yyyyMMdd", new CultureInfo("en-US"))}', '{dateTo.ToString("yyyyMMdd", new CultureInfo("en-US"))}', '{UserData.Username}', '{Filter.BranchList}'";
                IQueryable<EOD> queryableEOD = DB.EOD.FromSql(exec);

                return ExportExcelDailyRevenueVerify(dateFrom, dateTo, Filter.BranchList, await queryableDailyRevenueVerify.ToListAsync(), await queryableEOD.ToListAsync());
            }

            int totalCount = queryableDailyRevenueVerify.Count();
            List<DailyRevenueVerify> DailyRevenueVerify = queryableDailyRevenueVerify.Skip(pagination.From()).Take(pagination.To()).ToList();

            Response.Success = true;
            Response.Result = DailyRevenueVerify;
            Response.ResultInfo = new
            {
                page = pagination.Page,
                perPage = pagination.PerPage,
                count = DailyRevenueVerify.Count(),
                totalCount
            };

            DB.Dispose();

            return Json(Response.Render());
        }

        // POST Api/<controller>
        [HttpPost]
        public async Task<JsonResult> Post(DailyRevenueDetailViewModel adjustment)
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

            sqlStrBuilder.Append("EXEC sp_PDC_Report_DailyRevenueVerify_Set '");

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

        private FileStreamResult ExportExcelDailyRevenueVerify(DateTime dateFrom, DateTime dateTo, string branchList, List<DailyRevenueVerify> queryableDailyRevenueVerify, List<EOD> queryableEOD)
        {
            // Load the Excel Template
            Stream xlsxStreamDailyRevenueVerify = System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + @"\assets\templates\DailyRevenueVerifyReport.xlsx");

            ExcelEngine excelEngineDailyRevenueVerify = new ExcelEngine();

            // Loads or open an existing workbook through Open method of IWorkbooks
            IWorkbook workbookDailyRevenueVerify = excelEngineDailyRevenueVerify.Excel.Workbooks.Open(xlsxStreamDailyRevenueVerify);


            xlsxStreamDailyRevenueVerify.Dispose();

            workbookDailyRevenueVerify.Version = ExcelVersion.Excel2013;

            // Sheet #1
            IWorksheet worksheetDailyRevenueVerify = workbookDailyRevenueVerify.Worksheets[0];

            worksheetDailyRevenueVerify.Range["E2"].Text = $"{dateFrom.ToString("dd/MM/yyyy")} - {dateTo.ToString("dd/MM/yyyy")}";

            worksheetDailyRevenueVerify.ImportData(queryableDailyRevenueVerify.Select(i => new
            {
                i.ShopType,
                i.ERP_ID,
                i.BranchID,
                i.ReportDate,
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
                i.rabbitTopUp,
                i.mPayService,              
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
                //i.TUD,
                i.TotalTransfer,
                i.TotalCon,
                i.TotalBoxes,
                i.TotalDropOffBoxes,
                //VerifyDate = i.VerifyDate.HasValue ? i.VerifyDate.Value : DateTime.Now
            }), 5, 1, false);


            #region Add WorkSheet EOD
            //// Load the Excel Template
            Stream xlsxStreamEOD = System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + @"\assets\templates\CloseShopReport.xlsx");

            ExcelEngine excelEngineEOD = new ExcelEngine();

            // Loads or open an existing workbook through Open method of IWorkbooks
            IWorkbook workbookEOD = excelEngineEOD.Excel.Workbooks.Open(xlsxStreamEOD);

            xlsxStreamEOD.Dispose();

            workbookEOD.Version = ExcelVersion.Excel2013;

            // Sheet #1
            IWorksheet worksheetEOD = workbookEOD.Worksheets[0];

            worksheetEOD.Range["E2"].Text = $"{dateFrom.ToString("dd/MM/yyyy")}";

            worksheetEOD.ImportData(queryableEOD.Select(o => new
            {
                o.BranchType,
                o.ERPID,
                o.BranchID,
                o.Report_Date,
                o.TotalTransfer,
                o.TotalShipments,
                o.TotalBoxes,

                o.TransportService,
                o.AMService,
                o.PUPService,
                o.SATService,
                o.RASService,
                o.CODService,
                o.INSURService,
                o.PACKAGEService,
                o.SALEService,
                o.LNTUPService,
                ServiceDiscount = o.Discount,
                o.Shipment,
                o.Boxes,
                o.DropOffBoxes,
                o.TotalDetailService,

                o.Cash,
                o.Rabbit,
                o.CreditBBL,
                o.CreditSCB,
                o.LinePay,
                o.TotalDetailPay,

                o.Transportation,
                o.VASSurcharge,
                o.Discount,
                o.VAT,
                o.TotalDetailSurcharge,

                o.TotalFreightRevenue,

                o.BSDCity,
                o.BSDCityn,
                o.BSDCitys,
                o.BSDGrab,
                o.BSDDiscount,

                o.BSDTotalDetailService,

                o.BSDCash,
                o.BSDLinePay,
                o.BSDTotalPayment,
                o.BSDLineTopUp,
                o.BSDTotalPaymentCash,

                o.BSDConsignment,
                o.BSDBoxes,

                LastedUpdate = o.LastedUpdate.HasValue ? o.LastedUpdate.Value.ToString("yyyy-MM-dd HH:ss", enUS) : "-",
            }), 5, 1, false);
            #endregion

            workbookDailyRevenueVerify.Worksheets.AddCopy(worksheetEOD);

            MemoryStream ms = new MemoryStream();
            workbookDailyRevenueVerify.SaveAs(ms);
            ms.Position = 0;

            excelEngineDailyRevenueVerify.Dispose();
            excelEngineEOD.Dispose();

            return File(ms, "Application/msexcel", "KE_PDC_DailyRevenueVerify_Report_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".xlsx");
        }
    }
}
