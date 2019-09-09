using KE_PDC.Models;
using KE_PDC.Services;
using KE_PDC.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KE_PDC.Areas.Api.Controllers
{
    [Route("Api/[controller]")]
    [Authorize]
    public class DHLVerifyRTController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<DHLVerifyRTController> _logger;
        new ApiResponse Response = new ApiResponse();
        private KE_POSContext DB;
        private CultureInfo enUS = new CultureInfo("en-US");

        public DHLVerifyRTController(KE_POSContext context, ILogger<DHLVerifyRTController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            DB = context;
        }

        // GET Api/<controller>
        [HttpGet]
        public async Task<ActionResult> Get(BranchMonthlyViewModel Filter)
        {
            Pagination pagination = new Pagination(HttpContext);

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            string FileType = Request.Query["FileType"];
            string Type = Request.Query["Type"];

            if (Type == null)
            {
                Type = "";
            }

            DateTime monthYear = DateTime.ParseExact(Filter.MonthYear, "MM/yyyy", enUS);

            StringBuilder SQLStringBuilder = new StringBuilder();
            SQLStringBuilder.Append("EXEC sp_RPT313_DHLVerifyReport '");
            SQLStringBuilder.Append(UserData.Username);
            SQLStringBuilder.Append("', '");
            SQLStringBuilder.Append(Filter.BranchList);
            SQLStringBuilder.Append("', '");
            SQLStringBuilder.Append(monthYear.Month.ToString());
            SQLStringBuilder.Append("', '");
            SQLStringBuilder.Append(monthYear.Year.ToString());
            SQLStringBuilder.Append("'");

            string sql = SQLStringBuilder.ToString();

            List <DHLVerify> DHLVerify = await DB.DHLVerify.FromSql(sql).ToListAsync();

            if (FileType != null)
            {
                if (FileType.Equals("excel"))
                {
                    //return ExportExcelLINEPayRemittance(Type, LINEPayRemittance);
                }
            }

            int totalCount = DHLVerify.Count;

            DHLVerify = DHLVerify.Skip(pagination.From()).Take(pagination.To()).ToList();

            Response.Success = true;
            Response.Result = DHLVerify;
            Response.ResultInfo = new
            {
                page = pagination.Page,
                perPage = pagination.PerPage,
                count = DHLVerify.Count,
                totalCount = totalCount
            };

            DB.Dispose();

            return Json(Response.Render());
        }

        // POST Api/<controller>/ASK
        [HttpGet("{id}")]
        public async Task<JsonResult> Detail(string id, string MonthYear)
        {
            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DateTime monthYear = DateTime.ParseExact(MonthYear, "MM/yyyy", enUS);

            StringBuilder SQLStringBuilder = new StringBuilder();
            SQLStringBuilder.Append("sp_RPT313_DHLVerifyDetailReport '");
            SQLStringBuilder.Append(UserData.Username);
            SQLStringBuilder.Append("','");
            SQLStringBuilder.Append(id);
            SQLStringBuilder.Append("','");
            SQLStringBuilder.Append(monthYear.Month.ToString());
            SQLStringBuilder.Append("','");
            SQLStringBuilder.Append(monthYear.Year.ToString());
            SQLStringBuilder.Append("'");

            string sql = SQLStringBuilder.ToString();

            List<DailyRevenueDHL> DailyRevenueDHL = await DB.DailyRevenueDHL.FromSql(sql).ToListAsync();

            //if (Details.Count() == 0)
            //{
            //    return ResponseNotFound(id);
            //}

            Response.Success = true;
            Response.Result = DailyRevenueDHL;

            DB.Dispose();

            return Json(Response.Render());
        }

        // POST Api/<controller>/ASK
        [HttpGet("NoneShipment/{id}")]
        public async Task<JsonResult> NoneShipmentDetail(string id, string MonthYear)
        {
            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DateTime monthYear = DateTime.ParseExact(MonthYear, "MM/yyyy", enUS);

            StringBuilder SQLStringBuilder = new StringBuilder();
            SQLStringBuilder.Append("sp_RPT313_DHLVerifyNoneShipmentDetailReport '");
            SQLStringBuilder.Append(UserData.Username);
            SQLStringBuilder.Append("','");
            SQLStringBuilder.Append(id);
            SQLStringBuilder.Append("','");
            SQLStringBuilder.Append(monthYear.Month.ToString());
            SQLStringBuilder.Append("','");
            SQLStringBuilder.Append(monthYear.Year.ToString());
            SQLStringBuilder.Append("'");

            string sql = SQLStringBuilder.ToString();

            List<NoneShipmentDHL> NoneShipmentDHL = await DB.NoneShipmentDHL.FromSql(sql).ToListAsync();
            
            //if (Details.Count() == 0)
            //{
            //    return ResponseNotFound(id);
            //}

            Response.Success = true;
            Response.Result = NoneShipmentDHL;

            DB.Dispose();

            return Json(Response.Render());
        }

        // POST Api/<controller>
        [HttpPost]
        public async Task<JsonResult> Post(DHLVerifyRTUpdateViewModel Item)
        {
            if (!ModelState.IsValid)
            {
                return Json(Response.RenderError(ModelState));
            }

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DateTime monthYear = DateTime.ParseExact(Item.MonthYear, "MM/yyyy", enUS);
            decimal dhlAdjustment = 0;
            if (decimal.TryParse(Item.DHLAdjustment, out dhlAdjustment))
            {
            }

            StringBuilder sqlStrBuilder = new StringBuilder();

            sqlStrBuilder.Append("EXEC sp_RPT313_DHLVerifyUpdate '");

            sqlStrBuilder.Append(UserData.Username);
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(Item.BranchID);
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(monthYear.Month.ToString());
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(monthYear.Year.ToString());
            sqlStrBuilder.Append("', '");
            sqlStrBuilder.Append(dhlAdjustment);
            sqlStrBuilder.Append("'");

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
    }
}
