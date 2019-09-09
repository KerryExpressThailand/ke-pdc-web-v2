using KE_PDC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KE_PDC.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class AdministratorsController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<AdministratorsController> _logger;
        private KE_POSContext DB;

        public AdministratorsController(KE_POSContext context, ILogger<AdministratorsController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            DB = context;
        }

        // GET: /<controller>/
        [Authorize(Roles = "Administrators,AdministratorsIndex")]
        public IActionResult Index()
        {
            return View();
        }

        // GET: /<controller>/Users
        [Authorize(Roles = "Administrators,AdministratorsUsers")]
        public IActionResult Users()
        {
            return View();
        }

        // GET: /<controller>/Menus
        [Authorize(Roles = "Administrators,AdministratorsMenus")]
        public async Task<IActionResult> Menus()
        {
            List<Menu> navigations;
            Stream fs = System.IO.File.OpenRead($@"{_hostingEnvironment.ContentRootPath}\menus.json");
            using (StreamReader reader = new StreamReader(fs))
            {
                navigations = JsonConvert.DeserializeObject<List<Menu>>(await reader.ReadToEndAsync());
                ViewData["Navigation"] = navigations;
            }
            fs.Dispose();

            return View();
        }

        // POST: /<controller>/Menus
        [HttpPost]
        [Authorize(Roles = "Administrators,AdministratorsMenus")]
        //[ValidateAntiForgeryToken]
        public IActionResult SaveMenus(string navigations)
        {
            // Upload original file
            using (FileStream fs = System.IO.File.Create($@"{_hostingEnvironment.ContentRootPath}\menus.json"))
            {
                var fsWriter = new StreamWriter(fs);
                fsWriter.Write(JsonConvert.SerializeObject(JsonConvert.DeserializeObject<Menu>(navigations)));
                fsWriter.Dispose();
            }

            return RedirectToAction("Menus");
        }
    }
}
