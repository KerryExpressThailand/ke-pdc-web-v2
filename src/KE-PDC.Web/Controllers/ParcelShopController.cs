using KE_PDC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace KE_PDC.Web.Areas
{
    public class ParcelShopController : Controller
    {
        private KE_POSContext DB;

        public ParcelShopController(KE_POSContext context) => DB = context;

        public IActionResult Index() => View();

        // GET: /<controller>/CloseShop
        [Authorize(Roles = "Administrators,ParcelShopCloseShop")]
        public async Task<IActionResult> CloseShop() => await Report(View());

        // GET: /<controller>/ShopDaily
        [Authorize(Roles = "Administrators,ParcelShopShopDaily")]
        public async Task<IActionResult> ShopDaily() => await Report(View(), "BranchList");

        // GET: /<controller>/CloseBSD
        [Authorize(Roles = "Administrators,ParcelShopCloseBSD")]
        public async Task<IActionResult> CloseBSD() => await Report(View());

        private async Task<IActionResult> Report(ViewResult viewResult) => await Report(viewResult, null);

        private async Task<IActionResult> Report(ViewResult viewResult, string with)
        {
            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            var packageIDKeys = new string[] { "PKG03", "PKG04", "PKG18", "PKG05",/*"PKG06", */ "PKG07", "PKG08" };
            //PKG03 = Box S
            //PKG04 = Box M
            //PKG18 = Box M Plus
            //PKG05 = Box L
            //PKG06 = Box XL
            //PKG07 = Box Mini
            //PKG08 = Box Box S-Plus

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

            ViewData["PackageAll"] = await DB.Package
                .Where(p => packageIDKeys.Contains(p.PackageID))
                .OrderBy(p => p.UnitPrice).ToListAsync();

            DB.Dispose();

            return viewResult;
        }
    }
}