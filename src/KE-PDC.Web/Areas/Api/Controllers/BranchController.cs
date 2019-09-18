using KE_PDC.Models;
using KE_PDC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KE_PDC.Areas.Api.Controllers
{
    [Area("Api")]
    [Authorize]
    public class BranchController : Controller
    {
        new ApiResponse Response = new ApiResponse();
        private readonly ILogger<BranchController> _logger;
        private KE_POSContext DB;

        public BranchController(KE_POSContext context, ILogger<BranchController> logger)
        {
            _logger = logger;
            DB = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        // GET: /<controller>/Type
        public async Task<object> Type()
        {
            // Auth Data
            var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
            UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

            string sql = $"EXEC sp_PDC_BranchType_Get_Auth'{ UserData.Username }'";

            Response.Success = true;
            Response.Result = await DB.BranchType.FromSql(sql)
                .GroupBy(t => t.TypeGroup, (key, list) => new
                {
                    TypeGroupId = key,
                    Types = list
                })
                .ToListAsync();

            DB.Dispose();

            return Response.Render();
        }

        // POST: /<controller>/Get
        public async Task<object> Get(string type, string region)
        {
            try
            {
                // Auth Data
                var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
                UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

                if (type.Equals("all"))
                {
                    type = string.Empty;
                }
                Response.Success = true;
               

                if (region == null)
                {
                    var data = await DB.BranchList_r.FromSql("EXEC sp_PDC_Branch_Get_Auth @p0,@p1, @p2", parameters: new[] { UserData.Username, type, "" }).ToListAsync();

                    //var data = await DB.BranchList.FromSql("EXEC sp_PDC_Branch_Get '" + UserData.Username + "', '" + type + "'" + "', '" + "'").ToListAsync();
                    Response.Result = data;
                }else
                {
                    var data = await DB.BranchList_r.FromSql("EXEC sp_PDC_Branch_Get_Auth @p0,@p1, @p2", parameters: new[] { UserData.Username, type, region }).ToListAsync();

                    //var data = await DB.BranchList.FromSql("EXEC sp_PDC_Branch_Get '" + UserData.Username + "', '" + type + "'" + "', '" + region + "'").ToListAsync();
                    Response.Result = data;
                }
                DB.Dispose();

                return Response.Render();
            }
            catch(Exception ex)
            {
                string mss = ex.Message.ToString();
                return Response.Render();
            }
            
        }


        public async Task<object> GetBranList(string type, string region)
        {
            try
            {
                // Auth Data
                var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
                UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

                if (type.Equals("all"))
                {
                    type = string.Empty;
                }
                Response.Success = true;

                if (region == null)
                {
                    var data = await DB.BranchList_r.FromSql("EXEC sp_PDC_Branch_Get_Auth @p0,@p1, @p2", parameters: new[] { UserData.Username, type, "" }).ToListAsync();

                    //var data = await DB.BranchList.FromSql("EXEC sp_PDC_Branch_Get '" + UserData.Username + "', '" + type + "'" + "', '" + "'").ToListAsync();
                    Response.Result = data;
                }
                else
                {
                    var data = await DB.BranchList_r.FromSql("EXEC sp_PDC_Branch_Get_Auth @p0,@p1, @p2", parameters: new[] { UserData.Username, type, region }).ToListAsync();

                    //var data = await DB.BranchList.FromSql("EXEC sp_PDC_Branch_Get '" + UserData.Username + "', '" + type + "'" + "', '" + region + "'").ToListAsync();
                    Response.Result = data;
                }
                DB.Dispose();

                return Response.Render();
            }
            catch (Exception ex)
            {
                string mss = ex.Message.ToString();
                return Response.Render();
            }

        }


        public object Discount()
        {
            try
            {
                // Auth Data
                var userData = User.Claims.SingleOrDefault(c => c.Type.Equals("User")).Value;
                UserMapRole UserData = JsonConvert.DeserializeObject<UserMapRole>(userData);

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

                var data = DB.Database.ExecuteSqlCommand(" sp_PDC_Get_Master_Discount @jsonreq, @jsonOutput OUTPUT ", jsonInput, jsonOutput);
                ResultDiscount _Discount = JsonConvert.DeserializeObject<ResultDiscount> (jsonOutput.Value.ToString());

                Response.Success = true;
                //var data = await DB.BranchList.FromSql("EXEC sp_PDC_Branch_Get '" + UserData.Username + "', '" + type + "'" + "', '" + "'").ToListAsync();
                //Response.Result = Discount;
                //DB.Dispose();

                //return Response.Render();

                var model = _Discount.Result.Select(c => new KeyValuePair<string, Object>(c.Discount_Code, c.Discount_Description)).ToList();

                return Json(new { success = true, data = model });
            }
            catch (Exception ex)
            {
                string mss = ex.Message.ToString();
                return Response.Render();
            }

        }
    }
}
