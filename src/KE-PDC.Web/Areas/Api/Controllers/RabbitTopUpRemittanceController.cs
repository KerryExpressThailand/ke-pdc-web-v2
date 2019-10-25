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
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KE_PDC.Web.Areas.Api.Controllers
{
    [Route("Api/[controller]")]
    [Authorize]
    public class RabbitTopUpRemittanceController : Controller
    {
        new ApiResponse Response = new ApiResponse();
        private readonly ILogger<RabbitTopUpRemittanceController> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        private KE_POSContext DB;
        private KE_PMGWContext DBPMGW;
        private CultureInfo enUS = new CultureInfo("en-US");
        public RabbitTopUpRemittanceController(KE_POSContext context, KE_PMGWContext PMGWcontext, ILogger<RabbitTopUpRemittanceController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            DB = context;
            DBPMGW = PMGWcontext;
        }

        // POST: /<controller>/
        [HttpPost]
        public async Task<ActionResult> Get(ReportViewModel Filter, string fileType)
        {
            if (!ModelState.IsValid)
            {
                return Json(Response.RenderError(ModelState));
            }

            try
            {
                // Parameter
                fileType = (fileType ?? "").ToLower();

                Pagination pagination = new Pagination(HttpContext);

                // Auth Data
                var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
                UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

                DateTime dateRemittance = DateTime.ParseExact(Filter.DateRemittance, "dd/MM/yyyy", enUS);
                Remittance rmt = new Remittance
                {

                    RemittanceDate = dateRemittance.ToString("yyyyMMdd", enUS)
                };

                string _dateRemittance = JsonConvert.SerializeObject(rmt);
                SqlParameter jsonInput = new SqlParameter()
                {
                    ParameterName = "@jsonreq",
                    SqlDbType = SqlDbType.NVarChar,
                    SqlValue = _dateRemittance,
                    Size = int.MaxValue

                };

                SqlParameter jsonOutput = new SqlParameter()
                {
                    ParameterName = "@jsonOutput",
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Output,
                    Size = int.MaxValue
                };

                DB.Database.ExecuteSqlCommand(" sp_PDC_TopUpRemittanceDate @jsonreq, @jsonOutput OUTPUT ", jsonInput, jsonOutput);
                ResultRemittancedetailRabbit MPayRemittance = JsonConvert.DeserializeObject<ResultRemittancedetailRabbit>(jsonOutput.Value.ToString());


                List<RabbitTopUpRabbit> mPay = new List<RabbitTopUpRabbit>();
                foreach (var item in MPayRemittance.Result)
                {
                    RabbitTopUpRabbit dc = new RabbitTopUpRabbit
                    {
                        id = item.id,
                        BranchID = item.BranchID,
                        ReportDate = item.ReportDate,
                        DMSID = item.DMSID,
                        branch_type = item.branch_type,
                        ERP_ID = item.ERP_ID,
                        BranchName = item.BranchName,
                        TUC = item.TUC,
                        TUP = item.TUP,
                        TUD = item.TUD,
                        Captured = item.Captured,
                        CapturedBy = item.CapturedBy,
                        CapturedDate = item.CapturedDate,
                        RemittanceDate = item.RemittanceDate,
                        unit_price = item.unit_price,
                        Rabbit_transaction_Id = item.MPay_transaction_Id,
                        created_datetime = item.created_datetime,
                    };
                    mPay.Add(dc);
                }

                int count = 0;
                int totalCount = 0;
                object result = new List<object>();


                if (fileType.Equals("excel"))
                {
                    return ExportExcelLINEPayRemittance(mPay);
                }


                totalCount = mPay.Count();

                mPay = mPay.Skip(pagination.From()).Take(pagination.To()).ToList();

                result = mPay;
                count = mPay.Count();

                Response.Success = true;
                Response.Result = result;
                Response.ResultInfo = new
                {
                    page = pagination.Page,
                    perPage = pagination.PerPage,
                    count = count,
                    totalCount = totalCount
                };

                return Json(Response.Render());
            }
            catch (Exception ex)
            {
                string mss = ex.Message.ToString();
                return null;
            }

        }

        private FileStreamResult ExportExcelLINEPayRemittance(List<RabbitTopUpRabbit> Remittancedetail)
        {
            // Load the Excel Template
            Stream xlsxStream_mPayRemittancey = System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + @"\assets\templates\RabbitTopUpRemittance.xlsx");

            ExcelEngine excelEnginemPayRemittancey = new ExcelEngine();

            // Loads or open an existing workbook through Open method of IWorkbooks
            IWorkbook worksheetDiscount = excelEnginemPayRemittancey.Excel.Workbooks.Open(xlsxStream_mPayRemittancey);

            xlsxStream_mPayRemittancey.Dispose();

            worksheetDiscount.Version = ExcelVersion.Excel2013;

            // Sheet #1
            IWorksheet _worksheetDiscount = worksheetDiscount.Worksheets[0];

            //_worksheetDiscount.Range["E2"].Text = $"{LINEPayRemittance.ToString("dd/MM/yyyy")} - {dateTo.ToString("dd/MM/yyyy")}";

            _worksheetDiscount.ImportData(Remittancedetail.Select(i => new
            {
                i.id,
                i.ERP_ID,
                i.BranchID,
                i.branch_type,
                i.BranchName,
                i.TUC,
                i.TUP,
                i.TUD,
                i.Captured,
                i.CapturedBy,
                i.CapturedDate,
                i.ReportDate,
                i.unit_price,
                i.Rabbit_transaction_Id,
                i.RemittanceDate,

            }), 4, 1, false);
            //// Load the Excel Template

            ExcelEngine excelEngineEOD = new ExcelEngine();
            MemoryStream ms = new MemoryStream();
            worksheetDiscount.SaveAs(ms);
            ms.Position = 0;

            excelEnginemPayRemittancey.Dispose();
            excelEngineEOD.Dispose();

            return File(ms, "Application/msexcel", "KE_PDC_Discount_Report_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".xlsx");
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