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
    public class BranchesController : ControllerBase
    {
        private readonly ILogger<BranchesController> _logger;
        new readonly ApiResponse Response = new ApiResponse();
        private KE_POSContext DB;

        public BranchesController(KE_POSContext context, ILogger<BranchesController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            DB = context;
        }

        // GET: api/v1/DailyRevenues
        [HttpGet]
        public async Task<object> Get([FromQuery]BranchViewModel model)
        {
            IQueryable<Branch> queryable = DB.Branch.AsQueryable();

            if (model.BranchTypes != null && model.BranchTypes.Count() > 0)
            {
                queryable = queryable.Where(x => model.BranchTypes.Contains(x.BranchType));
            }

            if (model.OrderBy != null && QueryHelper.PropertyExists<Branch>(model.OrderBy))
            {
                string orderBy = model.OrderBy;

                if (model.OrderDirection.Equals("desc"))
                {
                    queryable = queryable.OrderByPropertyDescending(orderBy);
                }
                else
                {
                    queryable = queryable.OrderByProperty(orderBy);
                }
            }

            PaginatedList<object> dailyRevenues = await PaginatedList<object>.CreateAsync(queryable.AsNoTracking(), model.Page, model.PerPage);
            return new
            {
                dailyRevenues.CurrentPage,
                Data = dailyRevenues.ToList(),
                dailyRevenues.From,
                dailyRevenues.To,
                dailyRevenues.PerPage,
                dailyRevenues.TotalPages,
                dailyRevenues.Total
            };
        }
    }
}