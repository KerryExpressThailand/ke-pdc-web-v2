using KE_PDC.Models;
using KE_PDC.Services;
using KE_PDC.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KE_PDC.Areas.Api.Controllers
{
    [Route("Api/[controller]")]
    //[Authorize]
    public class MonthlyCommissionRTController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<MonthlyCommissionRTController> _logger;
        new ApiResponse Response = new ApiResponse();
        private KE_POSContext DB;
        private CultureInfo enUS = new CultureInfo("en-US");

        public MonthlyCommissionRTController(KE_POSContext context, ILogger<MonthlyCommissionRTController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            DB = context;
        }

        // POST Api/<controller>
        [HttpPost]
        public async Task<JsonResult> Post(UpdatePRViewModel UpdatePR)
        {
            if (!ModelState.IsValid)
            {
                return Json(Response.RenderError(ModelState));
            }

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DateTime monthYear = DateTime.ParseExact(UpdatePR.MonthYear, "MM/yyyy", enUS);

            StringBuilder SQLStringBuilder = new StringBuilder();

            for(int i = 0; i < UpdatePR.BranchID.Count(); i++)
            {
                SQLStringBuilder.AppendLine($"EXEC sp_RPT312_UpdatePR '{UserData.Username}', '{UpdatePR.BranchID[i]}', '{UpdatePR.PR[i]}', '{monthYear.Month.ToString()}', '{monthYear.Year.ToString()}'");
            }

            string strSQL = SQLStringBuilder.ToString();

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
