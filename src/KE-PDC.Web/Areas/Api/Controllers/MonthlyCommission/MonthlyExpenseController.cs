using KE_PDC.Models;
using KE_PDC.Models.POS;
using KE_PDC.Services;
using KE_PDC.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
    public class MonthlyExpenseController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<MonthlyExpenseController> _logger;
        new ApiResponse Response = new ApiResponse();
        private KE_POSContext DB;
        private CultureInfo enUS = new CultureInfo("en-US");

        public MonthlyExpenseController(KE_POSContext context, ILogger<MonthlyExpenseController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            DB = context;
        }

        // GET Api/<controller>
        [Route("Get")]
        public async Task<ActionResult> Get(BranchMonthlyViewModel Filter, String FileType)
        {
            Pagination pagination = new Pagination(HttpContext);

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            // Ref Page
            string type = Request.Query["Type"];
            string VerifyID = "";

            switch(type)
            {
                case "acc":
                    VerifyID = "EXPENSE_VERIFY_FOR_ACC";
                    break;
            }

            DateTime monthYear = DateTime.ParseExact(Filter.MonthYear, "MM/yyyy", enUS);

            string exec = $"sp_RPT300_FN_MonthlyExpenseSync '{UserData.Username}', '{monthYear.Month.ToString()}', '{monthYear.Year.ToString()}'";
            _logger.LogInformation(exec);
            await DB.Database.ExecuteSqlCommandAsync(exec);

            exec = $"EXEC sp_RPT310_GetMonthlyExpense '{UserData.Username}', '{Filter.BranchList}', '{monthYear.Month.ToString()}', '{monthYear.Year.ToString()}', '{VerifyID}'";
            _logger.LogInformation(exec);
            List<MonthlyExpense> MonthlyExpense = await DB.MonthlyExpense
                .FromSql(exec).ToListAsync();

            if(FileType != null)
            {
                if (FileType.Equals("excel"))
                {
                    return ExportExcelMonthlyExpense(MonthlyExpense);
                }
            }

            int totalCount = MonthlyExpense.Count();

            MonthlyExpense = MonthlyExpense.Skip(pagination.From()).Take(pagination.To()).ToList();

            Response.Success = true;
            Response.Result = MonthlyExpense;
            Response.ResultInfo = new
            {
                page = pagination.Page,
                perPage = pagination.PerPage,
                count = MonthlyExpense.Count(),
                totalCount = totalCount
            };

            DB.Dispose();

            return Json(Response.Render());
        }

        // GET Api/<controller>/ASK
        [HttpGet("{id}")]
        public async Task<JsonResult> Detail(string id, string MonthYear)
        {
            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DateTime monthYear = DateTime.ParseExact(MonthYear, "MM/yyyy", enUS);

            string exec = $"EXEC sp_PDC_Report_MonthlyCommissionSummary_Get '{UserData.Username}', '{id}', '{monthYear.Month.ToString()}', '{monthYear.Year.ToString()}', 0, ''";
            _logger.LogInformation(exec);
            List<MonthlySummaryCommission> MonthlyCommissionFC = await DB.MonthlySummaryCommission.FromSql(exec).ToListAsync();

            if (MonthlyCommissionFC.Count() > 0)
            {
                exec = $"EXEC sp_RPT310_GetMonthlyExpenseDetail '{UserData.Username}', '{ id }', '{monthYear.Month.ToString()}', '{monthYear.Year.ToString()}'";
                _logger.LogInformation(exec);
                MonthlyCommissionFC.FirstOrDefault().MonthlyExpenseDetail = await DB.MonthlyExpenseDetail
                    .FromSql(exec)
                    .ToListAsync();
            }

            //if (Details.Count() == 0)
            //{
            //    return ResponseNotFound(id);
            //}

            Response.Success = true;
            Response.Result = MonthlyCommissionFC;

            DB.Dispose();

            return Json(Response.Render());
        }

        // GET Api/<controller>/2017/2/KVIL/3/name.pdf
        [HttpGet("{year}/{month}/{branch}/{category}/{name}")]
        public ActionResult Download(int year, int month, string branch, int category, string name)
        {
            string path = $@"{_hostingEnvironment.ContentRootPath}\..\Upload\MonthlyExpense\{year}\{month}\{branch}\{category}\{name}";
            if (System.IO.File.Exists(path))
            {
                System.IO.FileStream file = System.IO.File.OpenRead(path);
                return File(file, "Application/octet-stream");
            }

            HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;

            return View("~/Views/Shared/Error.cshtml");
        }

        // POST Api/<controller>
        [HttpPost]
        public async Task<JsonResult> Post(MonthlyExpenseUpdateViewModel MonthlyExpenseUpdate)
        {
            if (!ModelState.IsValid)
            {
                return Json(Response.RenderError(ModelState));
            }

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            IEnumerable<IGrouping<string, IFormFile>> File = Request.Form.Files.GroupBy(f => f.Name);

            DateTime monthYear = DateTime.ParseExact(MonthlyExpenseUpdate.MonthYear, "MM/yyyy", enUS);
            string userID = UserData.Username;
            string branchID = MonthlyExpenseUpdate.BranchID;
            string month = monthYear.Month.ToString();
            string year = monthYear.Year.ToString();

            StringBuilder SQLStringBuilder = new StringBuilder();

            #region Management Fee
            if(!string.IsNullOrEmpty(MonthlyExpenseUpdate.ManagementFee))
            {
                decimal managementFee = 0;
                if (decimal.TryParse(MonthlyExpenseUpdate.ManagementFee, out managementFee))
                {
                }

                SQLStringBuilder.AppendLine(GenSQLString(
                    userID // @u_id
                    , branchID // @branch_id
                    , month // @month
                    , year // @year
                    , "MANAGEMENT_FEE" // @item_desc
                    , MonthlyExpenseUpdate.ManagementFeeID[0] // @item_id
                    , 1 // @category_id
                    , 1 // @item_amount
                    , managementFee // @item_expense
                    , null // @remark
                    , null // @attachment
                    , "false"
                ));
            }
            #endregion

            #region Service Fee - IT
            if(MonthlyExpenseUpdate.ServiceFeeITExpense != null)
            {
                for (int i = 0; i < MonthlyExpenseUpdate.ServiceFeeITExpense.Count(); i++)
                {
                    if (MonthlyExpenseUpdate.ServiceFeeITItem[i] == null && MonthlyExpenseUpdate.ServiceFeeITID[i] == null)
                    {
                        continue;
                    }

                    string item_desc = MonthlyExpenseUpdate.ServiceFeeITItem[i];
                    string itemID = string.IsNullOrEmpty(MonthlyExpenseUpdate.ServiceFeeITID[i]) ? null : MonthlyExpenseUpdate.ServiceFeeITID[i];
                    int category_id = 2;
                    int amount = 1;
                    decimal expense = 0;
                    string remark = string.IsNullOrEmpty(MonthlyExpenseUpdate.ServiceFeeITRemark[i]) ? null : MonthlyExpenseUpdate.ServiceFeeITRemark[i];
                    string attach = null;
                    string trash = MonthlyExpenseUpdate.ServiceFeeITTrash[i].ToString().ToLower();

                    IGrouping<string, IFormFile> file = File.SingleOrDefault(f => f.Key.Equals("ServiceFeeITAttach"));

                    if (file != null)
                    {
                        IFormFile attachFile = File.SingleOrDefault(f => f.Key.Equals("ServiceFeeITAttach")).ToArray()[i];
                        if (attachFile.Length > 0) attach = UploadFile(attachFile, year, month, branchID, category_id);
                    }

                    if (decimal.TryParse(MonthlyExpenseUpdate.ServiceFeeITExpense[i], out expense))
                    {
                    }

                    SQLStringBuilder.AppendLine(GenSQLString(
                        userID, branchID, month, year, item_desc, itemID, category_id, amount, expense, remark, attach, trash
                    ));
                }
            }
            #endregion

            #region Service Fee - Supply
            if(MonthlyExpenseUpdate.ServiceFeeSupplyExpense != null)
            {
                for (int i = 0; i < MonthlyExpenseUpdate.ServiceFeeSupplyExpense.Count(); i++)
                {
                    if (MonthlyExpenseUpdate.ServiceFeeSupplyItem[i] == null && MonthlyExpenseUpdate.ServiceFeeSupplyID[i] == null)
                    {
                        continue;
                    }

                    string item_desc = MonthlyExpenseUpdate.ServiceFeeSupplyItem[i];
                    string itemID = string.IsNullOrEmpty(MonthlyExpenseUpdate.ServiceFeeSupplyID[i]) ? null : MonthlyExpenseUpdate.ServiceFeeSupplyID[i];
                    int category_id = 3;
                    int amount = 1;
                    decimal expense;
                    string remark = string.IsNullOrEmpty(MonthlyExpenseUpdate.ServiceFeeSupplyRemark[i]) ? null : MonthlyExpenseUpdate.ServiceFeeSupplyRemark[i];
                    string attach = null;
                    string trash = MonthlyExpenseUpdate.ServiceFeeSupplyTrash[i].ToString().ToLower();

                    IGrouping<string, IFormFile> file = File.SingleOrDefault(f => f.Key.Equals("ServiceFeeSupplyAttach"));

                    if (file != null)
                    {
                        IFormFile attachFile = File.SingleOrDefault(f => f.Key.Equals("ServiceFeeSupplyAttach")).ToArray()[i];
                        if (attachFile.Length > 0) attach = UploadFile(attachFile, year, month, branchID, category_id);
                    }

                    if (decimal.TryParse(MonthlyExpenseUpdate.ServiceFeeSupplyExpense[i], out expense))
                    {
                    }

                    SQLStringBuilder.AppendLine(GenSQLString(
                        userID, branchID, month, year, item_desc, itemID, category_id, amount, expense, remark, attach, trash
                    ));
                }
            }
            #endregion

            #region Service Fee - Facility
            if(MonthlyExpenseUpdate.ServiceFeeFacilityExpense!= null)
            {
                for (int i = 0; i < MonthlyExpenseUpdate.ServiceFeeFacilityExpense.Count(); i++)
                {
                    if (MonthlyExpenseUpdate.ServiceFeeFacilityItem[i] == null && MonthlyExpenseUpdate.ServiceFeeFacilityID[i] == null)
                    {
                        continue;
                    }

                    string item_desc = MonthlyExpenseUpdate.ServiceFeeFacilityItem[i];
                    string itemID = string.IsNullOrEmpty(MonthlyExpenseUpdate.ServiceFeeFacilityID[i]) ? null : MonthlyExpenseUpdate.ServiceFeeFacilityID[i];
                    int category_id = 4;
                    int amount = 1;
                    decimal expense = 0;
                    string remark = string.IsNullOrEmpty(MonthlyExpenseUpdate.ServiceFeeFacilityRemark[i]) ? null : MonthlyExpenseUpdate.ServiceFeeFacilityRemark[i];
                    string attach = null;
                    string trash = "false"; // MonthlyExpenseUpdate.ServiceFeeFacilityTrash[i].ToString().ToLower();

                    IGrouping<string, IFormFile> file = File.SingleOrDefault(f => f.Key.Equals("ServiceFeeFacilityAttach"));

                    if (file != null)
                    {
                        IFormFile attachFile = File.SingleOrDefault(f => f.Key.Equals("ServiceFeeFacilityAttach")).ToArray()[i];
                        if (attachFile.Length > 0) attach = UploadFile(attachFile, year, month, branchID, category_id);
                    }

                    if (decimal.TryParse(MonthlyExpenseUpdate.ServiceFeeFacilityExpense[i], out expense))
                    {
                    }

                    SQLStringBuilder.AppendLine(GenSQLString(
                        userID, branchID, month, year, item_desc, itemID, category_id, amount, expense, remark, attach, trash
                    ));
                }
            }
            #endregion

            #region Sales Package
            if(MonthlyExpenseUpdate.SalesPackageAmount != null)
            {
                for (int i = 0; i < MonthlyExpenseUpdate.SalesPackageAmount.Count(); i++)
                {
                    if (string.IsNullOrEmpty(MonthlyExpenseUpdate.SalesPackageAmount[i]) && MonthlyExpenseUpdate.SalesPackageID[i] == null)
                    {
                        continue;
                    }

                    string item_desc = MonthlyExpenseUpdate.SalesPackageItem[i];
                    string itemID = string.IsNullOrEmpty(MonthlyExpenseUpdate.SalesPackageID[i]) ? null : MonthlyExpenseUpdate.SalesPackageID[i];
                    int category_id = 5;
                    int amount = 0;
                    decimal expense;
                    string remark = string.IsNullOrEmpty(MonthlyExpenseUpdate.SalesPackageRemark[i]) ? null : MonthlyExpenseUpdate.SalesPackageRemark[i];
                    string attach = null;
                    string trash = "false"; // MonthlyExpenseUpdate.SalesPackageTrash[i].ToString().ToLower();

                    IGrouping<string, IFormFile> file = File.SingleOrDefault(f => f.Key.Equals("SalesPackageAttach"));
                    
                    if (file != null)
                    {
                        IFormFile attachFile = File.SingleOrDefault(f => f.Key.Equals("SalesPackageAttach")).ToArray()[i];
                        if (attachFile.Length > 0) attach = UploadFile(attachFile, year, month, branchID, category_id);
                    }

                    if (int.TryParse(MonthlyExpenseUpdate.SalesPackageAmount[i], out amount))
                    {
                    }

                    if (decimal.TryParse(MonthlyExpenseUpdate.SalesPackageExpense[i], out expense))
                    {
                    }

                    SQLStringBuilder.AppendLine(GenSQLString(
                        userID, branchID, month, year, item_desc, itemID, category_id, amount, expense, remark, attach, trash
                    ));
                }
            }
            #endregion

            #region Adjustment
            if (!string.IsNullOrEmpty(MonthlyExpenseUpdate.Adjustment))
            {
                decimal adjustment = 0;
                if (decimal.TryParse(MonthlyExpenseUpdate.Adjustment, out adjustment))
                {
                }

                SQLStringBuilder.AppendLine(GenSQLString(
                    userID // @u_id
                    , branchID // @branch_id
                    , month // @month
                    , year // @year
                    , "ADJUSTMENT" // @item_desc
                    , MonthlyExpenseUpdate.AdjustmentID[0] // @item_id
                    , 6 // @category_id
                    , 1 // @item_amount
                    , adjustment // @item_expense
                    , string.IsNullOrEmpty(MonthlyExpenseUpdate.AdjustmentRemark[0]) ? null : MonthlyExpenseUpdate.AdjustmentRemark[0] // @remark
                    , null // @attachment
                    , "false"
                ));
            }
            #endregion

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

        // POST Api/<controller>/Verify
        [HttpPost("Verify")]
        public async Task<JsonResult> Verify(MonthlyExpenseVerifyViewModel MonthlyExpenseVerify)
        {
            if (!ModelState.IsValid)
            {
                return Json(Response.RenderError(ModelState));
            }

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DateTime monthYear = DateTime.ParseExact(MonthlyExpenseVerify.MonthYear, "MM/yyyy", enUS);
            StringBuilder SQLStringBuilder = new StringBuilder();

            string[] branchLists = MonthlyExpenseVerify.BranchID.Split(',');

            foreach (var branch in branchLists)
            {
                if (MonthlyExpenseVerify.Type.Equals("acc"))
                {
                    SQLStringBuilder.AppendLine($"EXEC sp_RPT310_SaveMonthlyExpenseAccountVerify '{UserData.Username}', '{branch}', {monthYear.Month.ToString()}, {monthYear.Year.ToString()}");
                }
                else if (MonthlyExpenseVerify.Type.Equals("ops"))
                {
                    SQLStringBuilder.AppendLine($"EXEC sp_RPT310_SaveMonthlyExpenseOpsAdminVerify '{UserData.Username}', '{branch}', {monthYear.Month.ToString()}, {monthYear.Year.ToString()}");
                }
                else if (MonthlyExpenseVerify.Type.Equals("rt"))
                {
                    SQLStringBuilder.AppendLine($"EXEC sp_RPT310_SaveMonthlyExpenseRetailVerify '{UserData.Username}', '{branch}', {monthYear.Month.ToString()}, {monthYear.Year.ToString()}");
                }
                else if (MonthlyExpenseVerify.Type.Equals("rtsupply"))
                {
                    SQLStringBuilder.AppendLine($"EXEC sp_RPT310_SaveMonthlyExpenseRetailSupplyVerify '{UserData.Username}', '{branch}', {monthYear.Month.ToString()}, {monthYear.Year.ToString()}");
                }
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

        private ActionResult ExportExcelMonthlyExpense(List<MonthlyExpense> List)
        {
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;

            // Creating new workbook
            IWorkbook workbook = application.Workbooks.Create(new[] { "MonthlyExpense" });
            IWorksheet worksheet = workbook.Worksheets[0];

            workbook.Version = ExcelVersion.Excel2013;

            worksheet.Range["A1"].Text = "BranchID";
            worksheet.Range["B1"].Text = "ManagementFee";
            worksheet.Range["C1"].Text = "ServiceFeeIT";
            worksheet.Range["D1"].Text = "ServiceFeeSupply";
            worksheet.Range["E1"].Text = "Facility";
            worksheet.Range["F1"].Text = "SalesPackage";
            worksheet.Range["G1"].Text = "Adjustment";
            worksheet.Range["H1"].Text = "AccountVerifyDate";
            worksheet.Range["I1"].Text = "AccountVerifyBy";
            worksheet.Range["J1"].Text = "OpsVerifyDate";
            worksheet.Range["K1"].Text = "OpsVerifyBy";
            worksheet.Range["L1"].Text = "RetailVerifyDate";
            worksheet.Range["M1"].Text = "RetailVerifyBy";

            // Header Style
            IStyle headerStyle = AddHeaderStyle(workbook);

            worksheet.Rows[0].CellStyle = headerStyle;

            worksheet.ImportData(List.Select(l => new {
                l.BranchID,
                ManagementFee = l.ManagementFee ?? 0,
                ServiceFeeIT = l.ServiceFeeIT ?? 0,
                ServiceFeeSupply = l.ServiceFeeSupply ?? 0,
                Facility = l.Facility ?? 0,
                SalesPackage = l.SalesPackage ?? 0,
                Adjustment = l.Adjustment ?? 0,
                l.FeeManagementVerifyBy,
                l.FeeManagementVerifyDate,
                l.FeeItVerifyBy,
                l.FeeItVerifyDate,
                l.FeeSupplyVerifyBy,
                l.FeeSupplyVerifyDate,
                l.FeeFacilityVerifyBy,
                l.FeeFacilityVerifyDate,
                l.SalesPackageVerifyBy,
                l.SalesPackageVerifyDate,
                l.FcConfirmBy,
                l.FcConfirmDate,
            }), 2, 1, false);

            for (int i = 1; i < 7; i++)
            {
                worksheet.Columns[i].CellStyle.NumberFormat = "#,##0.00";
            }

            worksheet.Rows[1].FreezePanes();

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, "Application/msexcel", "KE_PDC_MonthlyExpense_Report_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".xlsx");
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

        private string GenSQLString(string userID, string branchID, string month, string year, string item_desc, string item_id, int category_id, int item_amount, decimal item_expense, string remark, string attachmentName, string trash)
        {
            StringBuilder SQLStringBuilder = new StringBuilder();
            SQLStringBuilder.Append("EXEC sp_RPT310_SaveMonthlyExpenseUpdate '");
            SQLStringBuilder.Append(userID); // @u_id
            SQLStringBuilder.Append("', '");
            SQLStringBuilder.Append(branchID == null ? "" : branchID.Replace("'", "''")); // @branch_id
            SQLStringBuilder.Append("', '");
            SQLStringBuilder.Append(month); // @month
            SQLStringBuilder.Append("', '");
            SQLStringBuilder.Append(year); // @year
            SQLStringBuilder.Append("', '");
            SQLStringBuilder.Append(item_id); // @item_id
            SQLStringBuilder.Append("', N'");
            SQLStringBuilder.Append(item_desc == null ? "" : item_desc.Replace("'", "''")); // @item_desc
            SQLStringBuilder.Append("', '");
            SQLStringBuilder.Append(category_id); // @category_id
            SQLStringBuilder.Append("', '");
            SQLStringBuilder.Append(item_amount); // @item_amount
            SQLStringBuilder.Append("', '");
            SQLStringBuilder.Append(item_expense); // @item_expense
            SQLStringBuilder.Append("', N'");
            SQLStringBuilder.Append(remark == null ? "" : remark.Replace("'", "''")); // @remark
            SQLStringBuilder.Append("', N'");
            SQLStringBuilder.Append(attachmentName == null ? "" : attachmentName.Replace("'", "''")); // @attachment
            SQLStringBuilder.Append("', '");
            SQLStringBuilder.Append(trash); // @trash
            SQLStringBuilder.Append("'");

            return SQLStringBuilder.ToString();
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

        private string UploadFile(IFormFile attachFile, string year, string month, string branchID, int category_id)
        {
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + attachFile.FileName;
            string destination = $@"{_hostingEnvironment.ContentRootPath}\..\Upload\MonthlyExpense\{year}\{month}\{branchID}\{category_id}\";

            // Check Directory Exist
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            //if (System.IO.File.Exists(FullPathOriginal))
            //{
            //    System.IO.File.Delete(FullPathOriginal);
            //}

            // Upload original file
            using (FileStream fs = System.IO.File.Create(destination + filename))
            {
                attachFile.CopyTo(fs);
                fs.Flush();
                fs.Dispose();
            }

            return filename;
        }
    }
}
