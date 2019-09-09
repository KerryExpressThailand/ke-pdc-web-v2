using KE_PDC.API.Middleware;
using KE_PDC.Models;
using KE_PDC.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace KE_PDC.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Config object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // Setup options with DI
            services.AddOptions();

            // Add Default Connection to DBContext
            services.AddDbContext<KE_POSContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Add PMGW Connection to DBContext PMGW
            //services.AddDbContext<KE_PMGWContext>(options => options.UseSqlServer(Configuration.GetConnectionString("PMGWConnection")));

            // Add CMS Connection to DBContext CMS
            services.AddDbContext<KE_CMSContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CMSConnection")));

            // Add Logging
            services.AddLogging();

            // Add CORS services
            services.AddCors();

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddFile("Logs/KE-PDC-API-{Date}.txt");

            // Shows UseCors with CorsPolicyBuilder.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(builder => builder
                    .WithOrigins(Configuration.GetValue<string>("Origin"))
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            }

            app.UseStatusCodePages(async context =>
            {
                ApiResponse Response = new ApiResponse();

                if (context.HttpContext.Response.StatusCode.Equals((int)HttpStatusCode.NotFound))
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    Response.Errors.Add(new
                    {
                        Message = "No route for that URI"
                    });
                }

                await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(Response.Render()));
            });

            app.UseSecurityHeadersMiddleware();

            app.UseMvc(routes =>
            {
                routes.MapRoute("areaRoute", "{controller=Default}/{action=Index}/{id?}");
            });
        }
    }
}
