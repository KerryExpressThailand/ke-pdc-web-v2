using KE_PDC.Models;
using KE_PDC.Services;
using KE_PDC.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf.Parsing;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KE_PDC.Web.Areas.Api.Controllers.Dashboard
{
    [Produces("application/json")]
    [Route("Api/ParcelShop/CloseShop")]
    public class CloseShopController : Controller
    {
        new ApiResponse Response = new ApiResponse();
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<CloseShopController> _logger;
        private readonly CultureInfo _cultureENInfo = new CultureInfo("en-US");
        private readonly CultureInfo _cultureTHInfo = new CultureInfo("th-TH");
        private KE_POSContext DB;

        public CloseShopController(KE_POSContext context, ILogger<CloseShopController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            DB = context;
        }

        // GET: /Api/Dashboard/CloseShop/Get
        [HttpPost("Get")]
        public async Task<ActionResult> Get(CloseShopViewModel Filter, string Type, string FileType)
        {
            if (!ModelState.IsValid)
            {
                return Json(Response.RenderError(ModelState));
            }

            // Parameter
            Type = Type ?? string.Empty;
            FileType = FileType ?? string.Empty;

            Pagination pagination = new Pagination(HttpContext);

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DateTime dateFrom = DateTime.ParseExact(Filter.DateFrom, "dd/MM/yyyy", new CultureInfo("en-US"));

            string parameter = "'" + dateFrom.ToString("yyyyMMdd", new CultureInfo("en-US")) + "', '" + UserData.Username + "', '" + Filter.BranchList + "'";
            int count = 0;
            int totalCount = 0;
            object result = new List<object>();

            string EXEC = "EXEC sp_PDC_Dashboard_CloseShop_Get ";

            if (Type.Equals("confirm"))
            {
                //EXEC = "EXEC sp_RPT302_DailyRevenueConfirmed ";
            }

            string sql = EXEC + parameter;
            _logger.LogInformation(sql);

            if (Type.Equals("closeshop"))
            {
                IQueryable<CloseShop> CloseShop = DB.CloseShop.FromSql(sql);

                if (FileType.Equals("excel"))
                {
                    //return await ExportExcelVerify(DailyRevenue);
                }

                count = await CloseShop.Skip(pagination.From()).Take(pagination.To()).CountAsync();
                totalCount = CloseShop.Count();
                result = await CloseShop.Skip(pagination.From()).Take(pagination.To()).ToListAsync();
            }
            //else if (type.Equals("confirm"))
            //{
            //    IQueryable<DailyRevenueConfirm> DailyRevenue = DB.DailyRevenueConfirm.FromSql(EXEC + parameter);

            //    if (filetype.Equals("excel"))
            //    {
            //        //return await ExportExcelConfirm(DailyRevenue);
            //    }

            //    count = await DailyRevenue.Skip(pagination.From()).Take(pagination.To()).CountAsync();
            //    totalCount = DailyRevenue.Count();
            //    result = await DailyRevenue.Skip(pagination.From()).Take(pagination.To()).ToListAsync();
            //}

            Response.Success = true;
            Response.Result = result;
            Response.ResultInfo = new
            {
                page = pagination.Page,
                perPage = pagination.PerPage,
                count,
                totalCount
            };

            DB.Dispose();

            return Json(Response.Render());
        }

        // GET: /Api/Dashboard/CloseShop/Export
        [HttpPost("Export")]
        public async Task<ActionResult> Export(CloseShopViewModel Filter)
        {
            if (!ModelState.IsValid)
            {
                return Json(Response.RenderError(ModelState));
            }

            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            DateTime dateFrom = DateTime.ParseExact(Filter.DateFrom, "dd/MM/yyyy", new CultureInfo("en-US"));
            DateTime dateTo = DateTime.ParseExact(Filter.DateFrom, "dd/MM/yyyy", new CultureInfo("en-US"));
            string exec = $"sp_PDC_Dashboard_CloseShopCoverPage_Get '{dateFrom.ToString("yyyyMMdd", new CultureInfo("en-US"))}', '{dateTo.ToString("yyyyMMdd", new CultureInfo("en-US"))}', '{UserData.Username}', '{Filter.BranchList}'";
            _logger.LogInformation(exec);
            List<EOD> edo = await DB.EOD.FromSql(exec).ToListAsync();

            if (edo.Count() <= 0)
            {
                Response.Success = false;
                Response.Errors.Add(new
                {
                    Key = "BranchId",
                    Message = "No data found for " + Filter.BranchList
                });

                HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;

                return Json(Response.Render());
            }

            return Filter.FileType.Equals("excel") ? this.ExportExcel(edo, dateFrom) : this.ExportPDF(edo);
        }

        private FileStreamResult ExportExcel(List<EOD> edo, DateTime dateFrom)
        {
            // Load the Excel Template
            Stream xlsxStream = System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + @"\assets\templates\CloseShopReport.xlsx");

            ExcelEngine excelEngine = new ExcelEngine();

            // Loads or open an existing workbook through Open method of IWorkbooks
            IWorkbook workbook = excelEngine.Excel.Workbooks.Open(xlsxStream);

            workbook.Version = ExcelVersion.Excel2013;

            // Sheet #1
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.Range["E2"].Text = $"{dateFrom.ToString("dd/MM/yyyy")}";

            worksheet.ImportData(edo.Select(o => new
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
                o.rabbitTopUp,
                o.mPayService,
                ServiceDiscount = o.Discount,
                o.Shipment,
                o.Boxes,
                o.DropOffBoxes,
                o.TotalDetailService,

                o.Cash,
                o.Rabbit,
                o.CreditBBL,
                o.CreditSCB,
                o.QRPay,
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

                LastedUpdate = o.LastedUpdate.HasValue ? o.LastedUpdate.Value.ToString("yyyy-MM-dd HH:ss", _cultureENInfo) : "-",
            }), 5, 1, false);

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, "Application/msexcel", "KE_PDC_CloseShop_Report_" + DateTime.Now.ToString("yyyMMdd_HHmmss") + ".xlsx");
        }

        private FileStreamResult ExportPDF(List<EOD> edo)
        {
            // Load the PDF Template
            Stream pdfStream = System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + @"\assets\templates\CloseShop2.pdf");

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, (float)8);
            //PdfFont fontText = new PdfStandardFont(PdfFontFamily.Helvetica, (float)12);
            PdfFont fontTextTHSarabunNew = new PdfTrueTypeFont(System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + $@"\assets\fonts\THSarabunNew\THSarabunNew Bold.ttf"), 15);
            PdfFont fontTextCalibri = new PdfTrueTypeFont(System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + $@"\assets\fonts\calibri\Calibri.ttf"), 13);
            PdfFont fontTextCalibriBold = new PdfTrueTypeFont(System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + $@"\assets\fonts\calibri\Calibri.ttf"), 13, PdfFontStyle.Bold);

            // Load a PDF document.
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(pdfStream);

            //Create a new PDF document.
            PdfDocument pdfDocument = new PdfDocument();

            int numPage = 1;
            PdfPage pdfPage;

            //Set the format for string.
            PdfStringFormat formatAlignRight = new PdfStringFormat(PdfTextAlignment.Right);
            PdfStringFormat formatAlignCenter = new PdfStringFormat(PdfTextAlignment.Center);

            edo.ForEach(e => {
                pdfDocument.ImportPage(loadedDocument, 0);

                pdfPage = pdfDocument.Pages[numPage - 1];

                // Create PDF graphics for the page
                PdfGraphics graphics = pdfPage.Graphics;

                float xPosition = 105;
                float yPosition = (float)85.5;//106.5;
                float gap = (float)20.5;

                #region Header Left
                // Branch Name
                graphics.DrawString(e.BranchName.Replace("KERRY EXPRESS", "Kerry Express"), fontTextTHSarabunNew, PdfBrushes.Black, new PointF(xPosition, yPosition));


                // Date
                yPosition += gap;
                graphics.DrawString(e.Report_Date.ToString("dd-MMMM-yyyy", _cultureTHInfo), fontTextTHSarabunNew, PdfBrushes.Black, new PointF(xPosition, yPosition));


                // Branch ID
                yPosition += gap + (float)1;
                graphics.DrawString(e.BranchID, fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition));


                // Cost Center
                yPosition += gap + (float)1;
                graphics.DrawString(e.ERPID, fontTextTHSarabunNew, PdfBrushes.Black, new PointF(xPosition, yPosition));
                #endregion


                #region Header Right
                // Branch Type
                xPosition = (float)464;
                yPosition = (float)107;
                graphics.DrawString(e.BranchType.Split('-')[0], fontTextCalibri, PdfBrushes.Black, new PointF(xPosition - (float)14.5, yPosition), formatAlignCenter);

                // Total Transfer
                xPosition = (float)555.31;
                yPosition = (float)107;
                graphics.DrawString(e.TotalTransfer.ToString("N"), fontTextCalibriBold, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Shipment
                yPosition += gap;
                graphics.DrawString(e.TotalShipments.ToString("N0"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Boxes
                yPosition += gap;
                graphics.DrawString(e.TotalBoxes.ToString("N0"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);
                #endregion

                #region Detail Service
                xPosition = (float)281.585;
                yPosition = (float)219;
                gap = (float)19;

                // Transport Service
                graphics.DrawString(e.TransportService.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // AM Service
                yPosition += gap-1;
                graphics.DrawString(e.AMService.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // PUP Service
                yPosition += gap - 2;
                graphics.DrawString(e.PUPService.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // SAT Service
                yPosition += gap - 1;
                graphics.DrawString(e.SATService.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // RAS Service
                yPosition += gap - 1;
                graphics.DrawString(e.RASService.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // COD Service
                yPosition += gap - 1;
                graphics.DrawString(e.CODService.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // INSUR Service
                yPosition += gap - 1;
                graphics.DrawString(e.INSURService.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Package Service
                yPosition += gap - 2;
                graphics.DrawString(e.PACKAGEService.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Sale Package Service
                yPosition += gap - 1;
                graphics.DrawString(e.SALEService.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                
                // Discount
                yPosition += gap + 1;
                graphics.DrawString(e.Discount.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Line Top-up Service
                yPosition += gap - 1;
                graphics.DrawString(e.LNTUPService.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);               

                //rabbitTopUp
                yPosition += gap + 2;
                graphics.DrawString(e.rabbitTopUp.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                //mPayService
                yPosition += gap - 1;
                graphics.DrawString(e.mPayService.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Total Shipments
                yPosition += gap;
                graphics.DrawString(e.Shipment.ToString("N0"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Total Boxes
                yPosition += gap + 2;
                graphics.DrawString(e.Boxes.ToString("N0"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Drop-off Boxes
                yPosition += gap + 2;
                graphics.DrawString(e.DropOffBoxes.ToString("N0"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Total Detail Service
                yPosition += (float)34.5;
                graphics.DrawString(e.TotalDetailService.ToString("N"), fontTextCalibriBold, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);
                #endregion

                // Total Freight Revenue
                yPosition += (float)26;
                graphics.DrawString(e.TotalFreightRevenue.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                #region Detail Pay
                xPosition = (float)555.31;
                yPosition = (float)219.5;
                gap = (float)19;

                // Cash
                graphics.DrawString(e.Cash.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Rabbit
                yPosition += gap + 2;
                graphics.DrawString(e.Rabbit.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Credit Card BBL
                yPosition += gap + 2;
                graphics.DrawString(e.CreditBBL.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Credit Card SCB
                yPosition += gap + 2;
                graphics.DrawString(e.CreditSCB.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Credit QR Payment
                yPosition += gap + 2;
                graphics.DrawString(e.QRPay.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // LinePay
                yPosition += gap + 2;
                graphics.DrawString(e.LinePay.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Total Detail Pay
                yPosition += (float)32;
                graphics.DrawString(e.TotalDetailPay.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);
                #endregion

                #region Detail Surcharge
                yPosition = (float)439.5;
                gap = (float)19;

                // Transportation 
                graphics.DrawString(e.Transportation.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // VAS Surcharge
                yPosition += gap + 2;
                graphics.DrawString(e.VASSurcharge.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Discount
                yPosition += gap + 2;
                graphics.DrawString(e.Discount.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Vat
                yPosition += gap + 2;
                graphics.DrawString(e.VAT.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Total Detail Surcharge
                yPosition += (float)29;
                graphics.DrawString(e.TotalDetailSurcharge.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);
                #endregion

                #region BSD Surcharge
                xPosition = (float)281.585;
                yPosition = (float)632.5;
                gap = (float)19;

                // CITY
                graphics.DrawString(e.BSDCity.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // CITYN
                yPosition += gap + 2;
                graphics.DrawString(e.BSDCityn.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // CITYS
                yPosition += gap + 2;
                graphics.DrawString(e.BSDCitys.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Grab
                yPosition += gap + 2;
                graphics.DrawString(e.BSDGrab.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Discount
                yPosition += gap + 2;
                graphics.DrawString(e.BSDDiscount.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Total
                yPosition += gap + (float)12.5;
                graphics.DrawString(e.BSDTotalDetailService.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Total Consignment
                yPosition += gap + 12;
                graphics.DrawString(e.BSDConsignment.ToString("N0"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);
                #endregion

                #region BSD Surcharge
                xPosition = (float)555.31;
                yPosition = (float)632.5;
                gap = (float)19;

                // Cash
                graphics.DrawString(e.BSDCash.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Line Pay
                yPosition += gap + 2;
                graphics.DrawString(e.BSDLinePay.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Total Payment
                yPosition += gap + 2;
                graphics.DrawString(e.BSDTotalPayment.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Line Topup
                yPosition += gap + 2;
                graphics.DrawString(e.BSDLineTopUp.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Dummy
                yPosition += gap + 2;
                graphics.DrawString("", fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Total Payment Cash
                yPosition += gap + (float)12.5;
                graphics.DrawString(e.BSDTotalPaymentCash.ToString("N"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Total Payment Boxes
                yPosition += gap + 12;
                graphics.DrawString(e.BSDBoxes.ToString("N0"), fontTextCalibri, PdfBrushes.Black, new PointF(xPosition, yPosition), formatAlignRight);

                // Lasted Update 
                string closedDate = e.LastedUpdate.HasValue
                ? e.LastedUpdate.Value.ToString("dd MMM yyyy HH:mm:ss", _cultureENInfo)
                : "N/A";

                yPosition += gap + (float)7.8;
                xPosition = pdfDocument.Pages[0].GetClientSize().Width - 24;
                graphics.DrawString($"Closed Date/Time : {closedDate}", font, PdfBrushes.Red, new PointF(xPosition, yPosition), formatAlignRight);
                #endregion


                if (!(e.TotalDetailService.Equals(e.TotalDetailPay) && e.TotalDetailSurcharge.Equals(e.TotalDetailPay)))
                {
                    //watermark text.
                    PdfFont fontTextTHSarabunNewBold = new PdfTrueTypeFont(System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + $@"\assets\fonts\THSarabunNew\THSarabunNew Bold.ttf"), 48);
                    PdfGraphicsState state = graphics.Save();
                    graphics.SetTransparency(0.50f);
                    graphics.RotateTransform(-40);
                    graphics.DrawString("ข้อมูลไม่ถูกต้อง โปรดติดต่อผู้ดูแลระบบ", fontTextTHSarabunNewBold, PdfPens.Red, PdfBrushes.Red, new PointF(-300, 460));
                }

                numPage++;
            });

            //Set properties to paginate the table.
            PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
            layoutFormat.Break = PdfLayoutBreakType.FitElement;
            layoutFormat.Layout = PdfLayoutType.Paginate;
            layoutFormat.PaginateBounds = new RectangleF(20, 20, pdfDocument.Pages[0].GetClientSize().Width - 40, pdfDocument.Pages[0].GetClientSize().Height - 50);

            //Create a Page template that can be used as footer.
            RectangleF bounds = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 50);
            PdfPageTemplateElement footer = new PdfPageTemplateElement(bounds);
            PdfBrush brush = new PdfSolidBrush(Color.Black);

            //Create page number field.
            PdfPageNumberField pageNumber = new PdfPageNumberField(font, brush);

            //Create page count field.
            PdfPageCountField count = new PdfPageCountField(font, brush);

            //Add the fields in composite fields.
            PdfCompositeField compositeField = new PdfCompositeField(font, brush, "Page {0} of {1}", pageNumber, count);

            string printDate = DateTime.Now.ToString("dd MMM yyyy HH:mm:ss", _cultureENInfo);
            PdfCompositeField compositePrintDate = new PdfCompositeField(font, brush, string.Format("Printed from PDC/CloseShop      Printed Date/Time : {0}", printDate));

            compositeField.Bounds = footer.Bounds;

            //Draw the composite field in footer.
            compositeField.Draw(footer.Graphics, new PointF(pdfDocument.Pages[0].GetClientSize().Width - (float)63.5, 30));
            compositePrintDate.Draw(footer.Graphics, new PointF((float)24, 30));

            //Add the footer template at the bottom.
            pdfDocument.Template.Bottom = footer;

            MemoryStream ms = new MemoryStream();
            pdfDocument.Save(ms);
            ms.Position = 0;

            //Close the document
            pdfDocument.Close(true);

            // Close file
            pdfStream.Dispose();

            //Save the document.
            return File(ms, "Application/pdf");
        }
    }
}