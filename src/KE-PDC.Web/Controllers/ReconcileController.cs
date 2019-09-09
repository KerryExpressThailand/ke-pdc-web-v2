using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KE_PDC.Helper;
using KE_PDC.Models;
using KE_PDC.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using static KE_PDC.Models.SharedModel;

namespace KE_PDC.Web.Controllers
{
    public class ReconcileController : Controller
    {

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<ReconcileController> _logger;
        private KE_POSContext DB;
        private readonly IEDIServicesAD _api;
        private List<BillPayModel> _listOfRawData = new List<BillPayModel>();
        public ReconcileController(KE_POSContext context, ILogger<ReconcileController> logger, IHostingEnvironment hostingEnvironment, IEDIServicesAD api)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            DB = context;
            _api = api;
        }

        [HttpPost]
        public async Task<IActionResult> Savefile(IFormFile file)
        {
            file = Request.Form.Files[0];
            string folderName = "FileExcel";
            //var WebRootPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            //var PathWithFolderName = System.IO.Path.Combine(WebRootPath, "FileExcel");
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);               

                using (var stream = new FileStream(fullPath, FileMode.Create))                {
                   

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
                    int startRow = 6;

                    for (int i = startRow; i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) break;
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
                            _util.IsEmpty(_items.First().A).Trim().ToUpper().Equals("NO.") ||
                            _util.IsEmpty(_items.First().B).Trim().ToUpper().Equals("PAY.TIME") ||
                            _util.IsEmpty(_items.First().C).Trim().ToUpper().Equals("CUSTOMER NO.") ||
                            _util.IsEmpty(_items.First().D).Trim().ToUpper().Equals("CUSTOMER NAME.") ||
                            _util.IsEmpty(_items.First().E).Trim().ToUpper().Equals("PAY.DATE") ||
                            _util.IsEmpty(_items.First().F).Trim().ToUpper().Equals("REFERENCE NO.") ||
                            _util.IsEmpty(_items.First().G).Trim().ToUpper().Equals("FR BR.") ||
                            _util.IsEmpty(_items.First().H).Trim().ToUpper().Equals("AMOUNT") ||
                            _util.IsEmpty(_items.First().I).Trim().ToUpper().Equals("BY.") ||
                            _util.IsEmpty(_items.First().J).Trim().ToUpper().Equals("CHQ.NO") ||
                            _util.IsEmpty(_items.First().K).Trim().ToUpper().Equals("BC.") ||
                            _util.IsEmpty(_items.First().L).Trim().ToUpper().Equals("RC.")                          
                           )
                        {
                            
                            foreach (LineOfBillPayModel _line in _items)
                            {
                                _listOfRawData.Add(new BillPayModel
                                {
                                    Id = Guid.NewGuid().ToString("N"),
                                    CustomerCode = _util.IsEmpty(_line.E),
                                    CustomerName = _util.IsEmpty(_line.D),
                                    TransactionDate = DateTime.Now,
                                    PayDate = _util.ToDate(_line.B, "dd/MM/yyyy", "th-TH"),
                                    PayTime = _util.ToTime(_line.C),
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
                            // get only import data is valid ref2
                            //_items = _items.Where(i => i.F != null && i.F.Trim().Length == 11 && referenceMaster.Any(r => r.reference_no.Equals(i.F.Trim().Substring(8)))).ToList();

                            // fetch validate data line by line to model
                            foreach (LineOfBillPayModel _line in _items)
                            {
                                // increate item count for calculate percentage to display in progress bar

                                // mapping line item and convert to correct format to bill payment model
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
                    }
                }
               
            }


            SaveBill(_listOfRawData);
           // GetBatch GetBatch = DB.GetBatch.FromSql("EXEC sp_PDC_Reconcile_Get_Batch_Master").FirstOrDefault();
           // var GetBatch1 = DB.Database.ExecuteSqlCommand("EXECUTE sp_PDC_Reconcile_Get_Batch_Master");




            ModelState.AddModelError("sucess", "Username/Password wrong!");
            return View();
        }


        public IActionResult SaveBill(List<BillPayModel> model)
        {
            try
            {
                if (model.Count == 0)
                {
                    return Json(
                        new ResponseStatusModel()
                        {
                            status = new StatusModel { code = 400, desc = "Bad request, Please try again." }
                        }
                    );
                }
                string json = JsonConvert.SerializeObject(model);
                GetBatch batct_id = DB.GetBatch.FromSql("sp_PDC_Reconcile_Get_Batch_Master").FirstOrDefault();
                SqlParameter[] sqlParameters = { new SqlParameter("@json", SqlDbType.NVarChar) { Value = json }
                                                 ,new SqlParameter("@batch_id" ,SqlDbType.BigInt) { Value = batct_id.Batch_Id}

                };


                //return entities.FromSql(commandText, parameters);
                //int rows = DB.ExecuteSP("sp_PDC_Reconcile_Insert_Transaction", sqlParameters);
                return null;
            }
            catch
            { 
                return null;
            }
         }

        public ActionResult OnPostImport()
        {
            IFormFile file = Request.Form.Files[0];
            string folderName = "Upload";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            StringBuilder sb = new StringBuilder();
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
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
                    IRow headerRow = sheet.GetRow(0); //Get Header Row
                    int cellCount = headerRow.LastCellNum;
                    sb.Append("<table class='table'><tr>");
                    for (int j = 0; j < cellCount; j++)
                    {
                        NPOI.SS.UserModel.ICell cell = headerRow.GetCell(j);
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                        sb.Append("<th>" + cell.ToString() + "</th>");
                    }
                    sb.Append("</tr>");
                    sb.AppendLine("<tr>");
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                                sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
                        }
                        sb.AppendLine("</tr>");
                    }
                    sb.Append("</table>");
                }
            }
            return this.Content(sb.ToString());
        }
    }
}