using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KE_PDC.Areas.Api.ExternalServices.Utils;
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

namespace KE_PDC.Areas.Api.Controllers
{
    [Route("Api/[controller]")]
    [Authorize]

    public class DiscountController : Controller
    {
        new ApiResponse Response = new ApiResponse();
        private readonly ILogger<DiscountController> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        private KE_POSContext DB;
        private KE_PMGWContext DBPMGW;
        private CultureInfo enUS = new CultureInfo("en-US");

        public DiscountController(KE_POSContext context, KE_PMGWContext PMGWcontext, ILogger<DiscountController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            DB = context;
            DBPMGW = PMGWcontext;
        }

        // POST: /<controller>/
        [HttpPost]
        public async Task<ActionResult> DiscountReport(ReqDiscount ReqDiscount, string FileType)
        {
            try
            {
                string[] lines = ReqDiscount.BranchList.Split(",");

                Pagination pagination = new Pagination(HttpContext);


                string filetype = (FileType ?? "").ToLower();

                DateTime DateTo = DateTime.ParseExact(ReqDiscount.DateTo, "dd/MM/yyyy", new CultureInfo("en-US"));
                DateTime DateFrom = DateTime.ParseExact(ReqDiscount.DateFrom, "dd/MM/yyyy", new CultureInfo("en-US"));
                if (filetype.Equals("excel"))
                {
                    return ExportExcelDiscountReport(ReqDiscount, DateTo, DateFrom);
                }
                List<BranchIdList> _items = new List<BranchIdList>();
                foreach (var branch in lines)
                {
                    BranchIdList _b = new BranchIdList
                    {
                        BranchId = branch.ToString()
                    };
                    _items.Add(_b);
                }
                List<DiscountTypeList> _Dis = new List<DiscountTypeList>();
                foreach (var DiscountType in ReqDiscount.DiscountTypeList)
                {
                    DiscountTypeList _d = new DiscountTypeList
                    {
                        DiscountType = DiscountType.ToString()
                    };
                    _Dis.Add(_d);
                }

                ReqDiscountType DiscountTypeList = new ReqDiscountType
                {
                    DateFrom = DateFrom.ToString("yyyyMMdd"),
                    DateTo = DateTo.ToString("yyyyMMdd"),
                    BranchIdList = _items,
                    DiscountTypeList = _Dis
                };

                string json = JsonConvert.SerializeObject(DiscountTypeList);
                SqlParameter jsonInput = new SqlParameter()
                {
                    ParameterName = "@jsonreq",
                    SqlDbType = SqlDbType.NVarChar,
                    SqlValue = json,
                    Size = int.MaxValue
                };

                SqlParameter jsonOutput = new SqlParameter()
                {
                    ParameterName = "@jsonOutput",
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Output,
                    Size = int.MaxValue
                };



                DB.Database.ExecuteSqlCommand(" sp_PDC_Discount_Report_Detail @jsonreq, @jsonOutput OUTPUT ", jsonInput, jsonOutput);
                var data = jsonOutput.Value.ToString();
                if (data == "" || data == null)
                {
                    Response.Success = false;
                    Response.Result = DiscountTypeList;
                    Response.ResultInfo = new
                    {
                        page = pagination.Page,
                        perPage = pagination.PerPage,
                        count = 0,
                        totalCount = 0
                    };
                    return Json(Response.Render());
                }
                //JObject dd = JObject.Parse("[" + jsonOutput.Value.ToString() + "]");
                ResResultDiscount Discount = JsonConvert.DeserializeObject<ResResultDiscount>(jsonOutput.Value.ToString());

                //GlobalVal._listDiscount = Discount;

                List<DiscountModel> disc = new List<DiscountModel>();
                foreach (var item in Discount.Result)
                {
                    DiscountModel dc = new DiscountModel
                    {
                        BranchType = item.BranchType,
                        ERPID = item.ERPID,
                        BranchId = item.BranchId,
                        ReceiptNo = item.ReceiptNo,
                        ReceiptDate = item.ReceiptDate,
                        MemberId = item.MemberId,
                        SenderName = item.SenderName,
                        SenderMobile = item.SenderMobile,
                        DiscountCode = item.DiscountCode,
                        DiscountType = item.DiscountType,
                        Surcharge = item.Surcharge,
                        DiscountAmount = item.DiscountAmount

                    };
                    disc.Add(dc);
                }

                GlobalVal._listDiscount = disc;

                int totalCount = Discount.Result.Count();

                var _discount = Discount.Result.Skip(pagination.From()).Take(pagination.To()).ToList();

                Response.Success = true;
                Response.Result = disc;
                Response.ResultInfo = new
                {
                    page = pagination.Page,
                    perPage = pagination.PerPage,
                    count = _discount.Count(),
                    totalCount = totalCount
                };

                DB.Dispose();

                return Json(Response.Render());
            }
            catch (Exception ex)
            {
                var mss = ex.Message.ToString();
                return null;
            }

        }

        private FileStreamResult ExportExcelDiscountReport(ReqDiscount ReqDiscount, DateTime dateTo, DateTime dateFrom)
        {
            // Load the Excel Template
            Stream xlsxStreamDailyRevenueVerify = System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + @"\assets\templates\DiscountReport.xlsx");

            ExcelEngine excelEngineDailyRevenueVerify = new ExcelEngine();

            // Loads or open an existing workbook through Open method of IWorkbooks
            IWorkbook worksheetDiscount = excelEngineDailyRevenueVerify.Excel.Workbooks.Open(xlsxStreamDailyRevenueVerify);


            xlsxStreamDailyRevenueVerify.Dispose();

            worksheetDiscount.Version = ExcelVersion.Excel2013;

            // Sheet #1
            IWorksheet _worksheetDiscount = worksheetDiscount.Worksheets[0];

            _worksheetDiscount.Range["E2"].Text = $"{dateFrom.ToString("dd/MM/yyyy")} - {dateTo.ToString("dd/MM/yyyy")}";

            _worksheetDiscount.ImportData(GlobalVal._listDiscount.Select(i => new
            {
                i.BranchType,
                i.ERPID,
                i.BranchId,
                i.ReceiptNo,
                i.ReceiptDate,
                i.MemberId,
                i.SenderName,
                i.SenderMobile,
                i.DiscountCode,
                i.DiscountType,
                i.Surcharge,
                i.DiscountAmount,

            }), 5, 1, false);
            //// Load the Excel Template
            Stream xlsxStreamEOD = System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + @"\assets\templates\CloseShopReport.xlsx");

            ExcelEngine excelEngineEOD = new ExcelEngine();

            // Loads or open an existing workbook through Open method of IWorkbooks
            IWorkbook workbookEOD = excelEngineEOD.Excel.Workbooks.Open(xlsxStreamEOD);

            xlsxStreamEOD.Dispose();

            workbookEOD.Version = ExcelVersion.Excel2013;



            MemoryStream ms = new MemoryStream();
            worksheetDiscount.SaveAs(ms);
            ms.Position = 0;

            excelEngineDailyRevenueVerify.Dispose();
            excelEngineEOD.Dispose();

            return File(ms, "Application/msexcel", "KE_PDC_Discount_Report_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".xlsx");
        }
       
    }
   
}
