using KE_PDC.Models;
using KE_PDC.Models.POS;
using KE_PDC.Services;
using KE_PDC.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace KE_PDC.Web.Areas.Api.Controllers.DailyRevenue
{
    [Route("Api/DailyRevenue/[controller]")]
    [ApiController]
    public class ReconcileController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<ReconcileController> _logger;
        new readonly ApiResponse Response = new ApiResponse();
        private readonly KE_POSContext DB;
        private readonly CultureInfo enUS = new CultureInfo("en-US");

        public ReconcileController(KE_POSContext context, ILogger<ReconcileController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            DB = context;
        }

        [HttpPost("BillPayment")]
        public object BillPayment(BranchesBillDateRangeViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Response.RenderError(ModelState);
                }
                List<BranchIdList> _items = new List<BranchIdList>();
                foreach (var branch in model.BranchIdList)
                {
                    BranchIdList _b = new BranchIdList
                    {
                        BranchId = branch
                    };
                    _items.Add(_b);
                }
                ReqReconcile _line = new ReqReconcile
                {
                    DateFrom = DateTime.ParseExact(model.DateFrom, "dd/MM/yyyy", new CultureInfo("en-US")).ToString("yyyyMMdd"),
                    DateTo = DateTime.ParseExact(model.DateTo, "dd/MM/yyyy", new CultureInfo("en-US")).ToString("yyyyMMdd"),
                    //DateTo = DateTime.Parse(model.DateTo, new CultureInfo("en-US")).ToString("yyyyMMdd"),
                    BranchIdList = _items

                };

                string json = JsonConvert.SerializeObject(_line);
                SqlParameter jsonInput = new SqlParameter()
                {
                    ParameterName = "@jsonreq",
                    SqlDbType = SqlDbType.NVarChar,
                    SqlValue = json,
                    Size = int.MaxValue

                };

                SqlParameter jsonOutput = new SqlParameter()
                {
                    ParameterName = "@jsonOutput",
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Output,
                    Size = int.MaxValue
                };

                List<DailyRevenueReconcileBillPayment> dailyRevenueReconcile = DB.DailyRevenueReconcileBillPayment
                  .FromSql(
                      "EXEC sp_PDC_Report_DailyRevenueReconcileBillPayment_Get {0}, {1}, {2}, {3}, {4}"                    
                  ).ToList();
                DB.Database.ExecuteSqlCommand(" sp_PDC_Reconcile_Report_DailyRevenue_Bill_Payment_Verify_Summary @jsonreq, @jsonOutput OUTPUT ", jsonInput, jsonOutput);
                //JObject dd = JObject.Parse("[" + jsonOutput.Value.ToString() + "]");
                List<SumMatching> RevenueData = JsonConvert.DeserializeObject<List<SumMatching>>("[" + jsonOutput.Value.ToString() + "]");
                //0 Unmatch, 1 Match, 2 Not found data


                Response.Success = true;
                Response.Result = RevenueData;
               
                //Response.ResultInfo = new
                //{
                //    page = model.Page,
                //    perPage = model.PerPage,
                //    count = _line
                //};

                DB.Dispose();

                return Response.Render();
            }
            catch(Exception ex)
            {
               return Response.RenderError(ModelState);
            }
            
        }

        [HttpPost("Cards")]
        public object Cards(BranchesDateRangeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Response.RenderError(ModelState);
            }

            List<DailyRevenueReconcileCards> dailyRevenueReconcile = DB.DailyRevenueReconcileCards
                .FromSql(
                    "EXEC sp_PDC_Report_DailyRevenueReconcileCards_Get {0}, {1}, {2}, {3}, {4}, {}",
                    model.DateFrom.ToShortDateString(),
                    model.DateTo.ToShortDateString(),
                    JsonConvert.SerializeObject(model.BranchIdList),
                    model.Page,
                    model.PerPage,
                    model.Filter
                ).ToList();

            Response.Success = true;
            Response.Result = dailyRevenueReconcile;
            Response.ResultInfo = new
            {
                page = model.Page,
                perPage = model.PerPage,
                count = dailyRevenueReconcile.Count(),
                totalCount = DB.DailyRevenue.Join(DB.Branch, d => d.BranchId, b => b.BranchId, (d, b) => new { d = d })
                    .Where(
                        x => model.BranchIdList.Contains(x.d.BranchId) && x.d.ReportDate >= model.DateFrom && x.d.ReportDate <= model.DateTo
                    ).Count()
            };

            DB.Dispose();

            return Response.Render();
        }

        [HttpPost("LinePay")]
        public object LinePay(BranchesDateRangeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Response.RenderError(ModelState);
            }

            List<DailyRevenueReconcileLinePay> dailyRevenueReconcile = DB.DailyRevenueReconcileLinePay
                .FromSql(
                    "EXEC sp_PDC_Report_DailyRevenueReconcileLinePay_Get {0}, {1}, {2}, {3}, {4}",
                    model.DateFrom.ToShortDateString(),
                    model.DateTo.ToShortDateString(),
                    JsonConvert.SerializeObject(model.BranchIdList),
                    model.Page,
                    model.PerPage
                ).ToList();

            Response.Success = true;
            Response.Result = dailyRevenueReconcile;
            Response.ResultInfo = new
            {
                page = model.Page,
                perPage = model.PerPage,
                count = dailyRevenueReconcile.Count(),
                totalCount = DB.DailyRevenue.Join(DB.Branch, d => d.BranchId, b => b.BranchId, (d, b) => new { d = d })
                    .Where(
                        x => model.BranchIdList.Contains(x.d.BranchId) && x.d.ReportDate >= model.DateFrom && x.d.ReportDate <= model.DateTo
                    ).Count()
            };

            DB.Dispose();

            return Response.Render();
        }

        [HttpPost("QrPayment")]
        public object QrPayment(BranchesDateRangeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Response.RenderError(ModelState);
            }

            List<DailyRevenueReconcileQrPayment> dailyRevenueReconcile = DB.DailyRevenueReconcileQrPayment
                .FromSql(
                    "EXEC sp_PDC_Report_DailyRevenueReconcileQrPayment_Get {0}, {1}, {2}, {3}, {4}",
                    model.DateFrom.ToShortDateString(),
                    model.DateTo.ToShortDateString(),
                    JsonConvert.SerializeObject(model.BranchIdList),
                    model.Page,
                    model.PerPage
                ).ToList();

            Response.Success = true;
            Response.Result = dailyRevenueReconcile;
            Response.ResultInfo = new
            {
                page = model.Page,
                perPage = model.PerPage,
                count = dailyRevenueReconcile.Count(),
                totalCount = DB.DailyRevenue.Join(DB.Branch, d => d.BranchId, b => b.BranchId, (d, b) => new { d = d })
                    .Where(
                        x => model.BranchIdList.Contains(x.d.BranchId) && x.d.ReportDate >= model.DateFrom && x.d.ReportDate <= model.DateTo
                    ).Count()
            };

            DB.Dispose();

            return Response.Render();
        }
    }
}