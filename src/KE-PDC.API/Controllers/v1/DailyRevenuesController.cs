using KE_PDC.API.ViewModel;
using KE_PDC.Models;
using KE_PDC.Models.POS;
using KE_PDC.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KE_PDC.API.v1
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class DailyRevenuesController : Controller
    {
        private readonly ILogger<DailyRevenuesController> _logger;
        new ApiResponse Response = new ApiResponse();
        private KE_POSContext DB;

        public DailyRevenuesController(KE_POSContext context, ILogger<DailyRevenuesController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            DB = context;
        }

        // GET: api/v1/DailyRevenues
        [HttpPost]
        public async Task<object> Get([FromBody]BranchesDateRangeViewModel model)
        {
            IQueryable<DailyRevenue> queryable = DB.DailyRevenue.AsQueryable();

            var orderBy = model.OrderBy;

            if (model.Branches != null && model.Branches.Count() > 0)
            {
                queryable = queryable.Where(x => model.Branches.Contains(x.BranchId));
            }

            if (QueryHelper.PropertyExists<DailyRevenue>(orderBy))
            {
                if(model.OrderDirection.Equals("desc"))
                {
                    queryable = queryable.OrderByPropertyDescending(orderBy);
                }
                else
                {
                    queryable = queryable.OrderByProperty(orderBy);
                }
            }

            //var dbSetx = dbSet.Join(DB.Branch, d => d.BranchId, b => b.BranchId, (d, b) => new {
            //    b.ErpId,
            //    d
            //});

            PaginatedList<object> dailyRevenues = await PaginatedList<object>.CreateAsync(queryable.AsNoTracking(), model.Page, model.PerPage);
            return new {
                dailyRevenues.CurrentPage,
                Data = dailyRevenues.ToList(),
                dailyRevenues.From,
                dailyRevenues.To,
                dailyRevenues.PerPage,
                dailyRevenues.TotalPages,
                dailyRevenues.Total
            };
        }

        // GET: api/v1/DailyRevenues/BranchId?ReportDate=yyyy-MM-dd
        [HttpGet("{branchId}", Name = "Get")]
        public async Task<object> Get(string branchId, DateTime reportDate)
        {
            DailyRevenue dailyRevenue = await DB.DailyRevenue.Where(x => x.BranchId.Equals(branchId) && x.ReportDate.Equals(reportDate)).FirstOrDefaultAsync();
            if(dailyRevenue == null)
            {
                return NotFound();
            }

            return dailyRevenue;
        }

        // PUT: api/v1/DailyRevenues/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
    }
}
