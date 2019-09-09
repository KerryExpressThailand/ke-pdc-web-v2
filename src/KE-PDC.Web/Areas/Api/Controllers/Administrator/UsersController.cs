using KE_PDC.Models;
using KE_PDC.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace KE_PDC.Web.Areas.Api.Controllers.Administrator
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        new ApiResponse Response = new ApiResponse();
        private IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<UsersController> _logger;
        private readonly AppSettings _settings;
        private KE_POSContext DB;
        private CultureInfo enUS = new CultureInfo("en-US");

        public UsersController(KE_POSContext context, IHostingEnvironment env, IOptions<AppSettings> settings, ILogger<UsersController> logger)
        {
            _logger = logger;
            _settings = settings.Value;
            _hostingEnvironment = env;
            DB = context;
        }

        // GET: api/Users
        [HttpGet]
        public object Get()
        {
            Pagination pagination = new Pagination(HttpContext);

            int.TryParse(HttpContext.Request.Query["filters[group]"], out int filterGroup);

            IQueryable<User> iqUser = DB.User
                .Where(u =>
                    string.IsNullOrEmpty(pagination.SearchPhrase)
                    || u.Username.Contains(pagination.SearchPhrase)
                    || u.Name.Contains(pagination.SearchPhrase));

            if(filterGroup > 0)
            {
                iqUser = iqUser.Where(a => a.UserGroupId.Equals(filterGroup));

            }

            #region Order
            if (pagination.OrderRequest.Equals("username"))
            {
                iqUser = pagination.DirectionRequest.Equals("desc")
                    ? iqUser.OrderByDescending(a => a.Username)
                    : iqUser = iqUser.OrderBy(a => a.Username);
            }
            else if (pagination.OrderRequest.Equals("name"))
            {
                iqUser = pagination.DirectionRequest.Equals("desc")
                    ? iqUser.OrderByDescending(a => a.Name)
                    : iqUser.OrderBy(a => a.Name);
            }
            else if (pagination.OrderRequest.Equals("defaultshopid"))
            {
                iqUser = pagination.DirectionRequest.Equals("desc")
                    ? iqUser.OrderByDescending(a => a.DefaultShopId)
                    : iqUser.OrderBy(a => a.DefaultShopId);
            }
            //else if (pagination.OrderRequest.Equals("createdby"))
            //{
            //    iqUser = pagination.DirectionRequest.Equals("desc")
            //        ? iqUser.OrderByDescending(a => a.CreatedBy)
            //        : iqUser.OrderBy(a => a.CreatedBy);
            //}
            //else
            //{
            //    iqUser = pagination.DirectionRequest.Equals("desc")
            //        ? iqUser.OrderByDescending(a => a.CreatedAt)
            //        : iqUser.OrderBy(a => a.CreatedAt);
            //}
            #endregion

            int TotalCount = iqUser.Count();

            List<User> Users = iqUser
                .Skip(pagination.From())
                .Take(pagination.To())
                .ToList();

            Response.Success = true;
            Response.Result = Users;
            Response.ResultInfo = new
            {
                page = pagination.Page,
                perPage = pagination.PerPage,
                count = Users.Count,
                totalCount = TotalCount
            };

            return Response.Render();
        }

        // GET: api/Users/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Users
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
