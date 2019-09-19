using KE_PDC.Helper;
using KE_PDC.Models;
using KE_PDC.Services;
using KE_PDC.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KE_PDC.Models.SharedModel;

namespace KE_PDC.Web.Controllers
{
    [Authorize]
    public class DailyRevenueController : Controller
    {
        private KE_POSContext DB;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<ReconcileController> _logger;
        private readonly IEDIServicesAD _api;
        new readonly ApiResponse Response = new ApiResponse();
        private List<BillPayModel> _listOfRawData = new List<BillPayModel>();
        private List<NoBillPayModel> _listOfRawData_nobill = new List<NoBillPayModel>();
        public DailyRevenueController(KE_POSContext context, ILogger<ReconcileController> logger, IHostingEnvironment hostingEnvironment, IEDIServicesAD api)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            DB = context;
            _api = api;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> BillPayment()
        {
            return await Report(View());
        }

        public async Task<IActionResult> Cards()
        {
            return await Report(View());
        }

        public async Task<IActionResult> LinePay()
        {
            return await Report(View());
        }

        public async Task<IActionResult> QRPay()
        {
            return await Report(View());
        }

        private async Task<IActionResult> Report(ViewResult viewResult)
        {
            return await Report(viewResult, null);
        }

        private async Task<IActionResult> Report(ViewResult viewResult, string with)
        {
            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            switch (with)
            {
                case "BranchList":
                    ViewData["BranchList"] = await DB.BranchList.FromSql("EXEC sp_RPT309_GetUserShopListByProfileId '" + UserData.Username + "', '" + null + "', 0").ToListAsync();
                    break;
                case "BranchListWithERP":
                    ViewData["BranchList"] = await DB.BranchList.FromSql("EXEC sp_RPT309_GetUserShopListByProfileId '" + UserData.Username + "', '" + null + "', 1").ToListAsync();
                    break;
                case "UserProfile":
                    ViewData["UserProfile"] = await DB.UserProfile.ToListAsync();
                    break;
                case "MonthlyExpenseUpdateRT":
                    ViewData["UserProfile"] = await DB.UserProfile.ToListAsync();
                    break;
                case "DC-SHOP":
                    ViewData["BranchList"] = await DB.BranchList.FromSql("EXEC sp_RPT301_GetBranchList '" + UserData.Username + "', 'DC-SHOP'").ToListAsync();
                    break;
            }

                  List<Package> PackageAll = await DB.Package.OrderBy(p => p.UnitPrice).ToListAsync();

            ViewData["PackageAll"] = PackageAll;

            var searchPackage = new List<string> { "PKG03", "PKG04", "PKG05",/*"PKG06",*/"PKG07", "PKG08", "PKG18" };

            ViewData["PackageList"] = PackageAll
                .Where(p => searchPackage.Contains(p.PackageID))
                .ToList();

            ViewData["SupplyList"] = PackageAll
                .Where(p => (new[] { 4, 5 }).Contains(p.PackageType.Value))
                .OrderBy(p => p.UnitPrice)
                .ToList();

            DB.Dispose();

            return viewResult;
        }

        [HttpPost]
        public object GetDataBill([FromBody]BillDataViewModel model)
        {
            try
            {
                List<BranchIdList> _items = new List<BranchIdList>();
                foreach (var branch in model.BranchIdList)
                {
                    BranchIdList _b = new BranchIdList
                    {
                        BranchId = branch
                    };
                    _items.Add(_b);
                }
                ReqReconcileData _line = new ReqReconcileData
                {
                    DateFrom = DateTime.ParseExact(model.DateFrom, "dd/MM/yyyy", new CultureInfo("en-US")).ToString("yyyyMMdd"),
                    DateTo = DateTime.ParseExact(model.DateTo, "dd/MM/yyyy", new CultureInfo("en-US")).ToString("yyyyMMdd"),
                    //DateTo = DateTime.Parse(model.DateTo, new CultureInfo("en-US")).ToString("yyyyMMdd"),
                    MatchStatus = model.MatchStatus,
                    BranchIdList = _items

                };

                string json = JsonConvert.SerializeObject(_line);
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


                DB.Database.ExecuteSqlCommand(" sp_PDC_Reconcile_Report_DailyRevenue_Bill_Payment_Verify_Detail @jsonreq, @jsonOutput OUTPUT ", jsonInput, jsonOutput);
                //JObject dd = JObject.Parse("[" + jsonOutput.Value.ToString() + "]");
                ReconcileBillPaymentData RevenueData = JsonConvert.DeserializeObject<ReconcileBillPaymentData>(jsonOutput.Value.ToString());
                //0 Unmatch, 1 Match, 2 Not found data



                List<ReconcileBillPayment> rc = new List<ReconcileBillPayment>();
                foreach (var item in RevenueData.Result)
                {
                    ReconcileBillPayment bp = new ReconcileBillPayment
                    {
                        ID = item.BranchID + "|" + item.ReportDate.ToString("yyyyMMdd"),
                        ERP_ID = item.ERP_ID,
                        BranchType = item.BranchType,
                        BranchID = item.BranchID,
                        ReportDate = item.ReportDate,
                        EOD = item.EOD,
                        DailyRevenue = item.DailyRevenue,
                        Transfer = item.Transfer,
                        Variance = item.Variance,
                        CheckCloseShop = item.CheckCloseShop,
                        ReconcileMatch = item.ReconcileMatch,
                        IsAdjust = item.IsAdjust


                    };
                    rc.Add(bp);
                }
                string data = JsonConvert.SerializeObject(rc);
                Response.Success = true;
                Response.Result = rc;
                Response.ResultInfo = new
                {
                    page = model.Page,
                    perPage = model.PerPage,
                    count = rc.Count(),
                    totalCount = rc.Count()
                };


                return Response.Render();
                //return Ok(new { data = RevenueData });
            }
            catch (Exception ex)
            {
                return Response.Render();
            }
           

        }
      
        [HttpPost]
        public ActionResult Confirm(string[] Branches, string Verifydate, string RemittanceDate)
        {
            try
            {
                DateTime verifydate = DateTime.ParseExact(Verifydate, "dd/MM/yyyy", new CultureInfo("en-US"));
                DateTime remittanceDate = DateTime.ParseExact(RemittanceDate, "dd/MM/yyyy", new CultureInfo("en-US"));
                var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
                UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);
                List<ConfirmModel> cf = new List<ConfirmModel>();
                foreach (var item in Branches)
                {
                    var words = item.Split('|');
                    ConfirmModel bp = new ConfirmModel
                    {
                        BranchId = words[0].ToString(),
                        ReportDate = words[1],
                        UserId = UserData.Username,
                        VerifyDate = verifydate.ToString("yyyyMMdd"),
                        RemittanceDate = remittanceDate.ToString("yyyyMMdd")
                    };
                    cf.Add(bp);
                }
                ConfirmListsModel cm = new ConfirmListsModel
                {
                    ConfirmLists = cf
                };
                string json = JsonConvert.SerializeObject(cm);
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


                DB.Database.ExecuteSqlCommand(" sp_PDC_Reconcile_Bill_Payment_DailyRevenue_Confirm  @jsonreq, @jsonOutput OUTPUT ", jsonInput, jsonOutput);
                string status = JObject.Parse(jsonOutput.Value.ToString())["StatusCode"].ToString();
                if (status == "1")
                {
                    return Json(new { success = true, message = "" });
                }
                else
                {
                    return Json(new { success = false, message = "Cannot Confirm" });
                }
            }
            catch (Exception ex)
            {
                string mss = ex.Message.ToString();
                return Json(new { success = false, message = mss });
            }    
            

        }
        [HttpPost]
        public JsonResult EditAdjust(DailyRevenueDetailViewModel adjustment)
        {

            if (!ModelState.IsValid)
            {
                return Json(Response.RenderError(ModelState));
            }
            try
            {
                var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
                UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

                DateTime verifydate = DateTime.ParseExact(adjustment.VerifyDate, "dd/MM/yyyy", new CultureInfo("en-US"));
                DateTime remittanceDate = DateTime.ParseExact(adjustment.RemittanceDate, "dd/MM/yyyy", new CultureInfo("en-US"));
                AdjustBill aj = new AdjustBill
                {
                    BranchId = adjustment.Branch,
                    UserId = UserData.Username,
                    ReportDate = adjustment.ReportDate.ToString("yyyyMMdd"),
                    BonusCommission = 0,
                    AdjCreditCard = adjustment.AdjCreditCard,
                    AdjustmentOther = adjustment.Other,
                    ReturnCharge = adjustment.Return,
                    Suspense = adjustment.Suspensse,
                    WithHoldingTax = adjustment.WithHoldingTax,
                    Promotion = adjustment.Promotion,
                    BankCharge = adjustment.BankCharge,
                    AdjLinePay = adjustment.AdjLinePay,
                    Project = adjustment.Project,
                    BonusCommissionRemark = "",
                    AdjCreditCardRemark = adjustment.AdjCreditCardRemark,
                    AdjustmentOtherRemark = adjustment.OtherRemark,
                    ReturnChargeRemark = adjustment.ReturnRemark,
                    SuspenseRemark = adjustment.SuspensseRemark,
                    WithHoldingTaxRemark = adjustment.WithHoldingTaxRemark,
                    PromotionRemark = adjustment.PromotionRemark,
                    BankChargeRemark = adjustment.BankChargeRemark,
                    AdjLinePayRemark = adjustment.AdjLinePayRemark,
                    VerifyDate = verifydate.ToString("yyyyMMdd"),
                    ProjectRemark = adjustment.ProjectRemark,
                    RemittanceDate = remittanceDate.ToString("yyyyMMdd"),

                };
                AdjustListsModel AdjustLists = new AdjustListsModel
                {
                    AdjustLists = aj
                };
                string json = JsonConvert.SerializeObject(AdjustLists);
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

                DB.Database.ExecuteSqlCommand(" sp_PDC_Reconcile_Bill_Payment_DailyRevenue_Adjust  @jsonreq, @jsonOutput OUTPUT ", jsonInput, jsonOutput);

                Response.Success = true;

                string status = JObject.Parse(jsonOutput.Value.ToString())["StatusCode"].ToString();
                if (status == "1")
                {
                    return Json(new { success = true, message = "" });
                }
                else
                {
                    return Json(new { success = false, message = "Cannot Edit" });
                }
            }
            catch (Exception ex)
            {
                string mss = ex.Message.ToString();
                return Json(new { success = false, message = mss });
            }
            // Auth Data

        }
        [HttpPost]
        public ActionResult Bill([FromBody]BranchesBillDateRangeViewModel model)
        {
           
            List<BranchIdList> _items = new List<BranchIdList>();
            foreach (var branch in model.BranchIdList)
            {
                BranchIdList _b = new BranchIdList
                {
                    BranchId = branch
                };
                _items.Add(_b);
            }
            ReqReconcile _line = new ReqReconcile
            {
                DateFrom = DateTime.ParseExact(model.DateFrom, "dd/MM/yyyy", new CultureInfo("en-US")).ToString("yyyyMMdd"),
                DateTo = DateTime.ParseExact(model.DateTo, "dd/MM/yyyy", new CultureInfo("en-US")).ToString("yyyyMMdd"),
                //DateTo = DateTime.Parse(model.DateTo, new CultureInfo("en-US")).ToString("yyyyMMdd"),
                BranchIdList = _items

            };
         
            string json = JsonConvert.SerializeObject(_line);
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


            DB.Database.ExecuteSqlCommand(" sp_PDC_Reconcile_Report_DailyRevenue_Bill_Payment_Verify_Summary @jsonreq, @jsonOutput OUTPUT ", jsonInput, jsonOutput);
            //JObject dd = JObject.Parse("[" + jsonOutput.Value.ToString() + "]");
            SumMatching RevenueData = JsonConvert.DeserializeObject<SumMatching>(jsonOutput.Value.ToString());
            //0 Unmatch, 1 Match, 2 Not found data


            return Ok(new { data = RevenueData });

        }


        [HttpPost]
        public ActionResult BankCharge(string id, string branch, string reportDate,string variance)
        {
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            DateTime rdate = DateTime.ParseExact(reportDate, "yyyy-MM-ddTHH:mm:ss", new CultureInfo("en-US"));

            decimal x = decimal.Parse(variance);
            decimal value = x * -1;
            if (id == null)
            {
                return null;
            }
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);
            List<BankCharge> bk = new List<BankCharge>();
            BankCharge sd = new BankCharge
            {
                BranchId = branch,
                UserId = UserData.Username,
                ReportDate = rdate.ToString("yyyyMMdd"),
                Variance = value
            };
            bk.Add(sd);

            BankChargeList _line = new BankChargeList
            {
              AdjustLists = bk
            };

            string json = JsonConvert.SerializeObject(_line);
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


            DB.Database.ExecuteSqlCommand(" sp_PDC_Reconcile_Bill_Payment_DailyRevenue_BankCharge @jsonreq, @jsonOutput OUTPUT ", jsonInput, jsonOutput);
            //JObject dd = JObject.Parse("[" + jsonOutput.Value.ToString() + "]");
            SumMatching RevenueData = JsonConvert.DeserializeObject<SumMatching>(jsonOutput.Value.ToString());
            //0 Unmatch, 1 Match, 2 Not found data


            return Json(new { success = true, message = "" });

        }

        [HttpPost]
        public ActionResult Shortage(string id, string branch, string reportDate, string variance)
        {
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            DateTime rdate = DateTime.ParseExact(reportDate, "yyyy-MM-ddTHH:mm:ss", new CultureInfo("en-US"));

            decimal x = decimal.Parse(variance);
            decimal value = x * -1;
            if (id == null)
            {
                return null;
            }
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);
            List<BankCharge> bk = new List<BankCharge>();
            BankCharge sd = new BankCharge
            {
                BranchId = branch,
                UserId = UserData.Username,
                ReportDate = rdate.ToString("yyyyMMdd"),
                Variance = value
            };
            bk.Add(sd);

            BankChargeList _line = new BankChargeList
            {
                AdjustLists = bk
            };

            string json = JsonConvert.SerializeObject(_line);
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


            DB.Database.ExecuteSqlCommand(" sp_PDC_Reconcile_Bill_Payment_DailyRevenue_Shortage @jsonreq, @jsonOutput OUTPUT ", jsonInput, jsonOutput);
            //JObject dd = JObject.Parse("[" + jsonOutput.Value.ToString() + "]");
            SumMatching RevenueData = JsonConvert.DeserializeObject<SumMatching>(jsonOutput.Value.ToString());
            //0 Unmatch, 1 Match, 2 Not found data


            return Json(new { success = true, message = "" });

        }
        public ActionResult OnPostImport()
        {
            IFormFile file = Request.Form.Files[0];
            string folderName = "FileExcel";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            var date = DateTime.Now.ToString("yyyyMMddHHmmss");
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, string.Format($"ImportBillPay_{date}{sFileExtension}"));

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }
                    IRow headerRow = sheet.GetRow(7); //Get Header Row
                    int cellCount = headerRow.LastCellNum;

                    for (int j = 0; j < cellCount; j++)
                    {
                        NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;

                    }

                  
                    List<LineOfBillPayModel> _items  = new List<LineOfBillPayModel>();

                    //BillPayModel bp = new BillPayModel();
                    int startRow = 6;
                    List<HeaderBillPayModel> header = new List<HeaderBillPayModel>();
                    for (int i = startRow; i < 7; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.Count != cellCount) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                        ICell _a = row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _b = row.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _c = row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _d = row.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _e = row.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _f = row.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _g = row.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _h = row.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _i = row.GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _j = row.GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _k = row.GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _l = row.GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK);

                        HeaderBillPayModel _line = new HeaderBillPayModel
                        {
                            A = _a.ToString(),
                            B = _b.ToString(),
                            C = _c.ToString(),
                            D = _d.ToString(),
                            E = _e.ToString(),
                            F = _f.ToString(),
                            G = _g.ToString(),
                            H = _h.ToString(),
                            I = _i.ToString(),
                            J = _j.ToString(),
                            K = _k.ToString(),
                            L = _l.ToString()
                        };

                        // add line model to list of bill payment model object
                        header.Add(_line);
                    }
                    for (int i = startRow; i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        string cellA = row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        if (cellA.Equals("No.")) continue;
                        if (row.Cells.Count != cellCount ) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                        ICell _a = row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _b = row.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _c = row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _d = row.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _e = row.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _f = row.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _g = row.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _h = row.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _i = row.GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _j = row.GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _k = row.GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _l = row.GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK);

                        LineOfBillPayModel _line = new LineOfBillPayModel
                        {
                            A = _a.ToString(),
                            B = _b.ToString(),
                            C = _c.ToString(),
                            D = _d.ToString(),
                            E = _e.ToString(),
                            F = _f.ToString(),
                            G = _g.ToString(),
                            H = _h.ToString(),
                            I = _i.ToString(),
                            J = _j.ToString(),
                            K = _k.ToString(),
                            L = _l.ToString()
                        };

                        // add line model to list of bill payment model object
                        _items.Add(_line);


                    }

                    UtilHelper _util = new UtilHelper();

                    // check data in list of line items
                    if (_items.Any())
                    {
                        // check data format for realtime (report type) file.
                        if (
                            _util.IsEmpty(header.First().A).Trim().ToUpper().Equals("NO.") &&
                            _util.IsEmpty(header.First().B).Trim().ToUpper().Equals("PAY.TIME") &&
                            _util.IsEmpty(header.First().C).Trim().ToUpper().Equals("CUSTOMER NO.") &&
                            _util.IsEmpty(header.First().D).Trim().ToUpper().Equals("CUSTOMER NAME") &&
                            _util.IsEmpty(header.First().E).Trim().ToUpper().Equals("PAY.DATE") &&
                            _util.IsEmpty(header.First().F).Trim().ToUpper().Equals("REFERENCE NO.") &&
                            _util.IsEmpty(header.First().G).Trim().ToUpper().Equals("FR BR.") &&
                            _util.IsEmpty(header.First().H).Trim().ToUpper().Equals("AMOUNT") &&
                            _util.IsEmpty(header.First().I).Trim().ToUpper().Equals("BY") &&
                            _util.IsEmpty(header.First().J).Trim().ToUpper().Equals("CHQ.NO.") &&
                            _util.IsEmpty(header.First().K).Trim().ToUpper().Equals("BC.") &&
                            _util.IsEmpty(header.First().L).Trim().ToUpper().Equals("RC.") 
                           )
                        {
                        

                            foreach (LineOfBillPayModel _line in _items)
                            {   
                                _listOfRawData.Add(new BillPayModel
                                {
                                    Id = Guid.NewGuid().ToString("N"),
                                    CustomerCode = _util.IsEmpty(_line.C),
                                    CustomerName = _util.IsEmpty(_line.D),
                                    TransactionDate = DateTime.Now,
                                    PayDate = _util.ToDate(_line.E, "dd/MM/yyyy"),
                                    PayTime = _util.ToTime(_line.B),
                                    PayBy = _util.IsEmpty(_line.I),
                                    ReferenceNo = _util.IsEmpty(_line.F),
                                    FrBr = _util.IsEmpty(_line.G),
                                    Amount = _util.ToMoney(_line.H),
                                    ChqNo = _util.IsEmpty(_line.J),
                                    Bc = _util.IsEmpty(_line.K),
                                    Rc = _util.IsEmpty(_line.L),
                                });
                            }
                        }
                        else // else is data format for history (report type) file.
                        {
                            
                            return Json(new { success = false, message = "Cannot" });
                        }
                    }
                }

            }
            string addtran = InsertbillTran(_listOfRawData);
            if (addtran == "fail")
            {
                return Json(new { success = false, message = "" });
            }
            else
            {

                CheckRecode _line = new CheckRecode
                {
                    TotalRecord = _listOfRawData.Count,
                    BatchId = addtran
                };
                string json = JsonConvert.SerializeObject(_line);

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

                DB.Database.ExecuteSqlCommand(" sp_PDC_Reconcile_Get_Batch_Transaction_Data @jsonreq, @jsonOutput OUTPUT ", jsonInput, jsonOutput);
                string _recode = JObject.Parse(jsonOutput.Value.ToString())["StatusCode"].ToString();
                if (_recode == "1")
                {
                    return Json(new { success = true, message = "" });
                }
                else
                {
                    return Json(new { success = false, message = "" });
                }
            }
        }

        public ActionResult OnPostImportNobill()
        {
            IFormFile file = Request.Form.Files[0];
            string folderName = "FileExcel";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            var date = DateTime.Now.ToString("yyyyMMddHHmmss");
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, string.Format($"ImportNoBillPay_{date}{sFileExtension}"));

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }
                    IRow headerRow = sheet.GetRow(7); //Get Header Row
                    int cellCount = headerRow.LastCellNum;

                    for (int j = 0; j < cellCount; j++)
                    {
                        NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;

                    }

                    List<LineOfBillPayModel> _items = new List<LineOfBillPayModel>();
                    //BillPayModel bp = new BillPayModel();
                    int startRow = 0;

                    for (int i = startRow; i <= sheet.LastRowNum; i++) //Read Excel File
                    {

                        IRow row = sheet.GetRow(i);
                        if (row == null || row.Count() == 0) break;
                        ICell _a = row.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _b = row.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _c = row.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        ICell _d = row.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        if (string.IsNullOrEmpty(_a.ToString()) 
                            || string.IsNullOrEmpty(_b.ToString() )
                            || string.IsNullOrEmpty(_c.ToString() )
                            || string.IsNullOrEmpty(_d.ToString() ))
                             continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                        LineOfBillPayModel _line = new LineOfBillPayModel
                        {
                            A = _a.ToString(),
                            B = _b.ToString(),
                            C = _c.ToString(),
                            D = _d.ToString()
                        };

                        // add line model to list of bill payment model object
                        _items.Add(_line);


                    }

                    UtilHelper _util = new UtilHelper();

                    // check data in list of line items
                    if (_items.Any())
                    {
                        // check data format for realtime (report type) file.
                        if (
                            _util.IsEmpty((_items.First().A).Replace("\n", " ")).Trim().ToUpper().Equals("CUSTOMER NAME") &&
                            _util.IsEmpty((_items.First().B).Replace("\n", " ")).Trim().ToUpper().Equals("CUSTOMER NO.") &&                                                                                                                                                                                                                                                                                                                                                                            
                            _util.IsEmpty(_items.First().C).Trim().ToUpper().Equals("PAY.DATE") &&
                            _util.IsEmpty(_items.First().D).Trim().ToUpper().Equals("AMOUNT") 
                           )
                        {


                            foreach (LineOfBillPayModel _line in _items)
                            {
                            
                                _listOfRawData_nobill.Add(new NoBillPayModel
                                {

                                    Id = Guid.NewGuid().ToString("N"),
                                    CustomerName = _util.IsEmpty(_line.A),
                                    CustomerCode = _util.IsEmpty(_line.B),                            
                                    PayDate = _util.ToDate(_line.C, "dd/MM/yyyy"),
                                    Amount = _util.ToMoney(_line.D)

                                });
                            }
                        }
                        else // else is data format for history (report type) file.
                        {

                            return Json(new { success = false, message = "Cannot" });
                        }
                    }
                }

            }
            string addtran = InsertNobillTran(_listOfRawData_nobill);
            if (addtran == "fail")
            {
                return Json(new { success = false, message = "" });
            }
            else
            {

                CheckRecode _line = new CheckRecode
                {
                    TotalRecord = _listOfRawData_nobill.Count - 1,
                    BatchId = addtran
                };
                string json = JsonConvert.SerializeObject(_line);

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

                DB.Database.ExecuteSqlCommand(" sp_PDC_Reconcile_Get_Batch_No_Bill_Transaction_Data @jsonreq, @jsonOutput OUTPUT ", jsonInput, jsonOutput);
                string _recode = JObject.Parse(jsonOutput.Value.ToString())["StatusCode"].ToString();
                if (_recode == "1")
                {
                    return Json(new { success = true, message = "" });
                }
                else
                {
                    return Json(new { success = false, message = "" });
                }
            }
        }

       
        public string InsertbillTran(List<BillPayModel> Model)
        {
            try
            {
                SqlParameter jsonInput = new SqlParameter()
                {
                    ParameterName = "@jsonreq",
                    SqlDbType = SqlDbType.NVarChar,
                    SqlValue = "",
                    Size = int.MaxValue

                };

                SqlParameter jsonOutput = new SqlParameter()
                {
                    ParameterName = "@jsonOutput",
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Output,
                    Size = int.MaxValue
                };

                var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
                UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

                var batch = DB.Database.ExecuteSqlCommand(" sp_PDC_Reconcile_Get_Batch_Master @jsonreq, @jsonOutput OUTPUT ", jsonInput, jsonOutput);

                int _r = (int) Math.Ceiling(Model.Count() / 50.0);
                List<BillPay> _itemsBillpay = new List<BillPay>();
                for (int i = 0; i < _r; i++)
                {
                    _itemsBillpay.Clear();
                    foreach (var item in Model.Skip(i*50).Take(50).ToList())
                    {
                        BillPay _line = new BillPay
                        {
                            Id = item.Id,
                            CustomerCode = item.CustomerCode,
                            CustomerName = item.CustomerName,
                            TransactionDate = item.TransactionDate,
                            PayDate = item.PayDate,
                            PayTime = item.PayTime,
                            PayBy = item.PayBy,
                            ReferenceNo = item.ReferenceNo,
                            FrBr = item.FrBr,
                            Amount = item.Amount,
                            ChqNo = item.ChqNo,
                            Bc = item.Bc,
                            Rc = item.Rc,
                            BatchId = JObject.Parse(jsonOutput.Value.ToString())["batch_id"].ToString(),
                            CreatedBy = UserData.Username
                        };
                        _itemsBillpay.Add(_line);
                    }
                    //if (i == 0) _itemsBillpay.RemoveAt(0);

                    string status = SaveBill(_itemsBillpay);
                    if (status != "1")
                    {
                        return "fail";
                    }
                }
                return JObject.Parse(jsonOutput.Value.ToString())["batch_id"].ToString();
            }
            catch (Exception ex)
            {
                string mss = ex.Message.ToString();
                return null;
            }
           
        }

        public string InsertNobillTran(List<NoBillPayModel> Model)
        {
            try
            {
                SqlParameter jsonInput = new SqlParameter()
                {
                    ParameterName = "@jsonreq",
                    SqlDbType = SqlDbType.NVarChar,
                    SqlValue = "",
                    Size = int.MaxValue

                };

                SqlParameter jsonOutput = new SqlParameter()
                {
                    ParameterName = "@jsonOutput",
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Output,
                    Size = int.MaxValue
                };

                var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
                UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

                var batch = DB.Database.ExecuteSqlCommand(" sp_PDC_Reconcile_Get_Batch_Master @jsonreq, @jsonOutput OUTPUT ", jsonInput, jsonOutput);

                int _r = (int)Math.Ceiling(Model.Count() / 50.0);
                List<NoBill> _itemsBillpay = new List<NoBill>();
                for (int i = 0; i < _r; i++)
                {
                    _itemsBillpay.Clear();
                    foreach (var item in Model.Skip(i * 50).Take(50).ToList())
                    {
                        NoBill _line = new NoBill
                        {
                            Id = item.Id,
                            CustomerCode = item.CustomerCode,
                            CustomerName = item.CustomerName,
                            PayDate = item.PayDate,
                            Amount = item.Amount,
                            BatchId = JObject.Parse(jsonOutput.Value.ToString())["batch_id"].ToString(),
                            CreatedBy = UserData.Username
                        };
                        _itemsBillpay.Add(_line);
                    }
                    if (i == 0) _itemsBillpay.RemoveAt(0);

                    string status = SaveNoBill(_itemsBillpay);
                    if (status != "1")
                    {
                        return "fail";
                    }
                }
                return JObject.Parse(jsonOutput.Value.ToString())["batch_id"].ToString();
            }
            catch (Exception ex)
            {
                string mss = ex.Message.ToString();
                return null;
            }

        }

        public string SaveBill(List<BillPay> model)
         {
            try
            {
                if (model.Count == 0)
                {
                    return null;
                }
                string json = JsonConvert.SerializeObject(model);

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


                DB.Database.ExecuteSqlCommand(" sp_PDC_Reconcile_Insert_Bill_Payment_Transaction @jsonreq, @jsonOutput OUTPUT ", jsonInput, jsonOutput);
                string status = JObject.Parse(jsonOutput.Value.ToString())["StatusCode"].ToString();
                //string status = "1";
                return status;


            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
        }


        public string SaveNoBill(List<NoBill> model)
        {
            try
            {
                if (model.Count == 0)
                {
                    return null;
                }
                string json = JsonConvert.SerializeObject(model);

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


                DB.Database.ExecuteSqlCommand(" sp_PDC_Reconcile_Insert_No_Bill_Transaction @jsonreq, @jsonOutput OUTPUT ", jsonInput, jsonOutput);
                string status = JObject.Parse(jsonOutput.Value.ToString())["StatusCode"].ToString();
                //string status = "1";
                return status;


            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }


        [HttpPost]
        public ActionResult Rollback(string id, string branch, string reportDate)
        {
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            DateTime rdate = DateTime.ParseExact(reportDate, "yyyy-MM-ddTHH:mm:ss", new CultureInfo("en-US"));

            if (id == null)
            {
                return null;
            }
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);
            List<Rollback> bk = new List<Rollback>();
            Rollback sd = new Rollback
            {
                BranchId = branch,
                UserId = UserData.Username,
                ReportDate = rdate.ToString("yyyyMMdd")
            };
            bk.Add(sd);

            RollbackList _line = new RollbackList
            {
                AdjustLists = bk
            };

            string json = JsonConvert.SerializeObject(_line);
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


            DB.Database.ExecuteSqlCommand(" sp_PDC_Reconcile_Rollback_Adjust @jsonreq, @jsonOutput OUTPUT ", jsonInput, jsonOutput);
            //JObject dd = JObject.Parse("[" + jsonOutput.Value.ToString() + "]");
            SumMatching RevenueData = JsonConvert.DeserializeObject<SumMatching>(jsonOutput.Value.ToString());
            //0 Unmatch, 1 Match, 2 Not found data


            return Json(new { success = true, message = "" });

        }
    }
}