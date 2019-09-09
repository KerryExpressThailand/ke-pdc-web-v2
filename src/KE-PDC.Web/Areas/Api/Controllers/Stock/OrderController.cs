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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KE_PDC.Web.Areas.Api.Controllers.Stock
{
    [Route("Api/Stock/[controller]")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<OrderController> _logger;
        new ApiResponse Response = new ApiResponse();
        private KE_POSContext DB;
        private CultureInfo enUS = new CultureInfo("en-US");

        public OrderController(KE_POSContext context, ILogger<OrderController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            DB = context;
        }

        // POST: /Api/Stock/<controller>/Get
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
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DateTime dateFrom = DateTime.ParseExact(Filter.DateFrom, "dd/MM/yyyy", enUS);
            DateTime dateTo = DateTime.ParseExact(Filter.DateTo, "dd/MM/yyyy", enUS);

            string parameter = "'" + dateFrom.ToString("yyyyMMdd", enUS) + "', '" + dateTo.ToString("yyyyMMdd", enUS) + "', '" + UserData.Username + "', '" + Filter.BranchList + "'";

            string sql = "EXEC sp_PDC_Report_StockOrder_Get " + parameter;

            _logger.LogInformation(sql);

            List<StockOrder> StockOrder = await DB.StockOrder.FromSql(sql).ToListAsync();

            if (filetype.Equals("excel"))
            {
                return ExportExcel(dateFrom, dateTo, StockOrder);
            }

            int totalCount = StockOrder.Count();

            StockOrder = StockOrder.Skip(pagination.From()).Take(pagination.To()).ToList();

            Response.Success = true;
            Response.Result = StockOrder;
            Response.ResultInfo = new
            {
                page = pagination.Page,
                perPage = pagination.PerPage,
                count = StockOrder.Count(),
                totalCount = totalCount
            };

            DB.Dispose();

            return Json(Response.Render());
        }

        private FileStreamResult ExportExcel(DateTime dateFrom, DateTime dateTo, List<StockOrder> Items)
        {
            // Load the Excel Template
            Stream xlsxStream = System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + @"\assets\templates\StockOrderReport.xlsx");

            ExcelEngine excelEngine = new ExcelEngine();

            // Loads or open an existing workbook through Open method of IWorkbooks
            IWorkbook workbook = excelEngine.Excel.Workbooks.Open(xlsxStream);
            xlsxStream.Dispose();

            workbook.Version = ExcelVersion.Excel2013;

            // Sheet #1
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.ImportData(Items.Select(i => new
            {
                i.BranchType,
                i.ERP_ID,
                i.BranchID,
                i.OrderID,
                i.Status,
                OrderDate = i.OrderDate.HasValue ? i.OrderDate.Value.ToString("yyyy-MM-dd HH:ss", enUS) : "-",
                DeliveryDate = i.DeliveryDate.HasValue ? i.DeliveryDate.Value.ToString("yyyy-MM-dd HH:ss", enUS): "-",
                i.PackageID,
                i.PackageDescription,
                i.Unit,
                i.OrderQuantity,
                i.UnitPrice,
                i.PackingQuantity,
                i.Amount
            }), 2, 1, false);

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, "Application/msexcel", "KE_PDC_StockOrder_Report_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".xlsx");
        }
    }
}