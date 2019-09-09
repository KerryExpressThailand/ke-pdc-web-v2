using KE_PDC.Services;
using KE_PDC.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace KE_PDC.Areas.Api.Controllers.Administrator
{
    [Produces("application/json")]
    [Route("Api/Administrator/[controller]")]
    [Authorize]
    public class MenusController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<MenusController> _logger;
        new ApiResponse Response = new ApiResponse();

        public MenusController(ILogger<MenusController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Api/Administrator/Menus
        [HttpGet]
        public async Task<object> GetAsync()
        {
            List<Menu> menu;
            Stream fs = System.IO.File.OpenRead($@"{_hostingEnvironment.ContentRootPath}\menus.json");
            using (StreamReader reader = new StreamReader(fs))
            {
                menu = JsonConvert.DeserializeObject<List<Menu>>(await reader.ReadToEndAsync());
                Response.Result = menu;
            }
            fs.Dispose();

            Response.Success = true;

            return Response.Render();
        }

        // GET: Api/Administrator/Menu/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: Api/Administrator/Menus
        [HttpPost]
        public object Post([FromBody]List<MenuViewModel> menus)
        {
            //menu.Add(new Menu
            //{
            //    Id = Guid.NewGuid(),
            //    Controller = "a",
            //    Action = "a",
            //    Url = "a",
            //    LinkText = "a",
            //    Icon = "a",
            //    Children = new List<List<Menu>> { new List<Menu> { } }
            //});

            if(menus == null)
            {
                Response.Success = false;
                Response.Messages = new List<object> { "Menu can't Nullable" };
            }
            else
            {
                // Upload original file
                using (FileStream fs = System.IO.File.Create($@"{_hostingEnvironment.ContentRootPath}\menus.json"))
                {
                    try
                    {
                        Response.Success = true;
                        Response.Result = menus;
                        Response.Messages = new List<object> { "Success" };

                        var fsWriter = new StreamWriter(fs);
                        fsWriter.Write(JsonConvert.SerializeObject(menus));
                        fsWriter.Dispose();
                    }
                    catch (IOException e)
                    {
                        Response.Success = false;
                        Response.Messages = new List<object> { e.Message };
                    }
                }
            }

            return Response.Render();
        }

        // PUT: Api/Administrator/Menus/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE: Api/Administrator/Menus/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
