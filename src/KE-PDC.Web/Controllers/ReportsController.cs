using Microsoft.AspNetCore.Mvc;
using KE_PDC.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KE_PDC.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private KE_POSContext DB;

        public ReportsController(KE_POSContext context)
        {
            DB = context;
        }

        #region Acc Version 1.0.4
        // GET: /<controller>/Index
        //public IActionResult Index()
        //{
        //    return await Report(1200, View());
        //}

        // GET: /<controller>/DailyRevenueVerify
        public async Task<IActionResult> DailyRevenueVerify()
        {
            return await Report(1201, View());
        }

        // GET: /<controller>/DailyRevenueConfirm
        public async Task<IActionResult> DailyRevenueConfirm()
        {
            return await Report(1202, View());
        }

        // GET: /<controller>/ShopDailyRevenue
        public async Task<IActionResult> ShopDailyRevenue()
        {
            return await Report(1203, View("Index"));
        }

        // GET: /<controller>/Receipt
        public async Task<IActionResult> Receipt()
        {
            return await Report(1204, View("Index"));
        }

        // GET: /<controller>/TaxInvoice
        public async Task<IActionResult> TaxInvoice()
        {
            return await Report(1205, View("Index"));
        }

        // GET: /<controller>/Commission
        public async Task<IActionResult> Commission()
        {
            return await Report(1206, View("Index"));
        }

        // GET: /<controller>/LinePay
        public async Task<IActionResult> LinePay()
        {
            return await Report(1207, View("Index"));
        }

        // GET: /<controller>/LINETopUpRemittance
        public async Task<IActionResult> LINETopUpRemittance()
        {
            return await Report(1208, View("Index"));
        }
        public async Task<IActionResult> MPayTopUpRemittance()
        {
            return await Report(1209, View());
        }

        public async Task<IActionResult> RabbitTopUpRemittance()
        {
            return await Report(1210, View());
        }
        #endregion

        #region FC Version 2.0.0
        // GET: /<controller>/DailyRevenueConfirmFC
        public async Task<IActionResult> DailyRevenueConfirmFC()
        {
            return await Report(1209, View(), "BranchList");
        }

        // GET: /<controller>/SummaryCommissionFC
        public async Task<IActionResult> SummaryCommissionFC()
        {
            return await Report(1210, View(), "BranchList");
        }

        // GET: /<controller>/SummaryCommissionRT
        public async Task<IActionResult> SummaryCommissionRT()
        {
            return await Report(1211, View(), "UserProfile");
        }

        // GET: /<controller>/MonthlyExpenseUpdateRT
        public async Task<IActionResult> MonthlyExpenseUpdateRT()
        {
            return await Report(1212, View(), "MonthlyExpenseUpdateRT");
        }

        // GET: /<controller>/MonthlyExpenseVerifyACC
        public async Task<IActionResult> MonthlyExpenseVerifyACC()
        {
            return await Report(1213, View("MonthlyExpenseVerify"), "BranchListWithERP");
        }

        // GET: /<controller>/MonthlyExpenseVerifyOps
        public async Task<IActionResult> MonthlyExpenseVerifyOps()
        {
            return await Report(1214, View("MonthlyExpenseVerify"), "BranchListWithERP");
        }

        // GET: /<controller>/MonthlyExpenseVerifyRT
        public async Task<IActionResult> MonthlyExpenseVerifyRT()
        {
            return await Report(1215, View("monthlyexpenseverify"), "BranchListWithERP");
        }

        // GET: /<controller>/DHLVerifyRT
        public async Task<IActionResult> DHLVerifyRT()
        {
            return await Report(1216, View(), "UserProfile");
        }

        // GET: /<controller>/DCTopUpVerify
        public async Task<IActionResult> DCTopUpVerify()
        {
            return await Report(1217, View(), "DC-SHOP");
        }

        // GET: /<controller>/MonthlyExpenseVerifyRTSupply
        public async Task<IActionResult> MonthlyExpenseVerifyRTSupply()
        {
            return await Report(1218, View("monthlyexpenseverify"), "BranchListWithERP");
        }

        // GET: /<controller>/DailyCOD
        public async Task<IActionResult> DailyCOD()
        {
            return await Report(1219, View(), "BranchListWithERP");
        }

        // GET: /<controller>/DownloadTUDReport
        public async Task<IActionResult> DownloadTUDReport()
        {
            return await Report(1220, View(), "BranchListWithERP");
        }

        // GET: /<controller>/SummaryDropOffBox
        public async Task<IActionResult> SummaryDropOffBox()
        {
            return await Report(1221, View(), "BranchListWithERP");
        }

        // GET: /<controller>/StockOrder
        public async Task<IActionResult> StockOrder()
        {
            return await Report(1222, View(), "BranchListWithERP");
        }
        public async Task<IActionResult> DiscountReport()
        {
            return await Report(1223, View());
        }
        #endregion

        // GET: /<controller>/Insurance
        public async Task<IActionResult> Insurance()
        {
            return await Report(1205, View("Insurance"));
        }

        private async Task<IActionResult> Report(int id, ViewResult viewResult)
        {
            return await Report(id, viewResult, null);
        }

        private async Task<IActionResult> Report(int id, ViewResult viewResult, string with)
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
    }
}
