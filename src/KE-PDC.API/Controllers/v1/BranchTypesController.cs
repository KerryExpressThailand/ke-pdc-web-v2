using KE_PDC.API.ViewModel;
using KE_PDC.Models;
using KE_PDC.Models.POS;
using KE_PDC.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace KE_PDC.API.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BranchTypesController : ControllerBase
    {
        private readonly ILogger<BranchTypesController> _logger;
        new ApiResponse Response = new ApiResponse();
        private KE_POSContext DB;

        public BranchTypesController(KE_POSContext context, ILogger<BranchTypesController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            DB = context;
        }

        // GET: api/v1/DailyRevenues
        //[HttpGet]
        //public async Task<object> Get([FromQuery]BranchTypeViewModel model)
        //{
        //    IQueryable<Branch> queryable = DB.Branch.AsQueryable();

        //    if (model.SearchBy != null && model.SearchKeyword != null)
        //    {
        //        queryable = queryable.Where(x => model.BranchTypes.Contains(x.BranchType));
        //    }

        //    if (model.OrderBy != null && QueryHelper.PropertyExists<Branch>(model.OrderBy))
        //    {
        //        string orderBy = model.OrderBy;

        //        if (model.OrderDirection.Equals("desc"))
        //        {
        //            queryable = queryable.OrderByPropertyDescending(orderBy);
        //        }
        //        else
        //        {
        //            queryable = queryable.OrderByProperty(orderBy);
        //        }
        //    }

        //    PaginatedList<object> dailyRevenues = await PaginatedList<object>.CreateAsync(queryable.AsNoTracking(), model.Page, model.PerPage);
        //    return new
        //    {
        //        dailyRevenues.CurrentPage,
        //        Data = dailyRevenues.ToList(),
        //        dailyRevenues.From,
        //        dailyRevenues.To,
        //        dailyRevenues.PerPage,
        //        dailyRevenues.TotalPages,
        //        dailyRevenues.Total
        //    };
        //}
    }
}