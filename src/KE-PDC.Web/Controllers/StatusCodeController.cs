using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KE_PDC.Controllers
{
    public class StatusCodeController : Controller
    {
        // GET: StatusCode
        [HttpGet("StatusCode/{code?}")]
        public ActionResult Index(int code)
        {
            return View("Error", code);
        }
    }
}