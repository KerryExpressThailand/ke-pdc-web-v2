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
using System.Text;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KE_PDC.Areas.Api.Controllers
{
    [Route("Api/[controller]")]
    [Authorize]
    public class InsurancesController : Controller
    {
        new ApiResponse Response = new ApiResponse();
        private readonly ILogger<InsurancesController> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        private KE_POSContext DB;
        private KE_PMGWContext DBPMGW;
        private CultureInfo enUS = new CultureInfo("en-US");

        public InsurancesController(KE_POSContext context, KE_PMGWContext PMGWcontext, ILogger<InsurancesController> logger, IHostingEnvironment hostingEnvironment)
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

            int count = 0;
            int totalCount = 0;
            object result = new List<object>();

            StringBuilder SQLStringBuilder = new StringBuilder();
            SQLStringBuilder.Append("EXEC sp_PDC_Report_Insurance '");
            SQLStringBuilder.Append(dateFrom.ToString("yyyyMMdd", enUS));
            SQLStringBuilder.Append("', '");
            SQLStringBuilder.Append(dateTo.ToString("yyyyMMdd", enUS));
            SQLStringBuilder.Append("', '");
            SQLStringBuilder.Append(UserData.Username);
            SQLStringBuilder.Append("', '");
            SQLStringBuilder.Append(Filter.BranchList);
            SQLStringBuilder.Append("'");

            String sqlStr = SQLStringBuilder.ToString();

            List<Insurance> Insurances = await DB.Insurance.FromSql(sqlStr).ToListAsync();

            if (fileType.Equals("excel"))
            {
                return ExportExcelInsurance(Insurances, dateFrom, dateTo);
            }

            totalCount = Insurances.Count();

            Insurances = Insurances.Skip(pagination.From()).Take(pagination.To()).ToList();

            result = Insurances;
            count = Insurances.Count();

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

        private FileStreamResult ExportExcelInsurance(List<Insurance> Items, DateTime DateFrom, DateTime DateTo)
        {
            // Load the Excel Template
            Stream xlsxStream = System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + @"\assets\templates\Insurance.xlsx");

            ExcelEngine excelEngine = new ExcelEngine();

            // Loads or open an existing workbook through Open method of IWorkbooks
            IWorkbook workbook = excelEngine.Excel.Workbooks.Open(xlsxStream);

            workbook.Version = ExcelVersion.Excel2013;

            // Sheet #1
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.Range["C2"].Text = $"{DateFrom.ToString("dd/MM/yyyy")} - {DateTo.ToString("dd/MM/yyyy")}";

            worksheet.ImportData(Items, 4, 1, false);

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, "Application/msexcel", "KE_PDC_Insurance_Report_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".xlsx");
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
