using KE_PDC.Models;
using KE_PDC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace KE_PDC
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Authentication
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options => {
                options.LoginPath = new PathString("/Login");
                options.LogoutPath = new PathString("/Logout");
                options.AccessDeniedPath = new PathString("/StatusCode/403");
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = context =>
                    {
                        if (RedirectIsApi(context))
                        {
                            ApiResponse Response = new ApiResponse();
                            Response.Success = false;
                            Response.Errors.Add(new
                            {
                                Code = 401,
                                Message = "Unauthorized"
                            });

                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Response.WriteAsync(JsonConvert.SerializeObject(Response.Render()));
                        }
                        else
                        {
                            context.Response.Redirect(context.RedirectUri);
                        }

                        return Task.FromResult(0);
                    },
                    OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

                        if (RedirectIsApi(context))
                        {
                            ApiResponse Response = new ApiResponse();
                            Response.Success = false;
                            Response.Errors.Add(new
                            {
                                Code = 403,
                                Message = "Unauthorized"
                            });

                            context.Response.ContentType = "application/json";
                            context.Response.WriteAsync(JsonConvert.SerializeObject(Response.Render()));
                        }
                        else
                        {
                            context.Response.Redirect(context.RedirectUri);
                        }

                        return Task.FromResult(0);
                    }
                };
            });

            // Add Localization
            services.Configure<RequestLocalizationOptions>(
                options => {
                    CultureInfo[] supportedCultures = new[]
                    {
                        new CultureInfo("th-TH"),
                        new CultureInfo("en-US"),
                    };
                    options.DefaultRequestCulture = new RequestCulture("en-US", "th-TH");

                    // Formatting numbers, dates, etc.
                    options.SupportedCultures = new[] { new CultureInfo("en-US") };

                    // UI strings that we have localized.
                    options.SupportedUICultures = supportedCultures;
                });

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            // Add Config object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // Setup options with DI
            services.AddOptions();

            // Add Default Connection to DBContext
            services.AddDbContext<KE_POSContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptions => sqlServerOptions.CommandTimeout(15000)
            ));

            // Add PMGW Connection to DBContext PMGW
            services.AddDbContext<KE_PMGWContext>(options => options.UseSqlServer(Configuration.GetConnectionString("PMGWConnection")));

            // Add CMS Connection to DBContext CMS
            services.AddDbContext<KE_CMSContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CMSConnection")));

            // Add RTSP Connection to DBContext RTSP
            services.AddDbContext<KE_RTSPContext>(options => options.UseSqlServer(Configuration.GetConnectionString("RTSPConnection")));


            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IEDIServicesAD, WebApi>();

            // Add Logging
            services.AddLogging();

            // Add session related services.
            services.AddSession();

            // Add framework services.
            services.AddMvc()
                //.AddJsonOptions(options => {
                //    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                //})
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, options => { options.ResourcesPath = "Resources"; })
                .AddDataAnnotationsLocalization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddFile("Logs/KE-PDC-{Date}.txt");

            //if (env.IsDevelopment() || env.EnvironmentName.Equals("UAT"))
            //{
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/StatusCode");

            //    // Add Status Code Pages
            //    app.UseStatusCodePagesWithReExecute("/StatusCode/{0}");

            //    app.Use(async (httpContext, next) =>
            //    {
            //        string url = httpContext.Request.Host.ToUriComponent();
            //        if (url.Equals("th.rnd.kerryexpress.com") || url.Equals("th.ke.rnd.kerrylogistics.com"))
            //        {
            //            httpContext.Response.Redirect("http://th.pos.kerryexpress.com/PDC" + (httpContext.Request.Path.HasValue ? httpContext.Request.Path.Value : string.Empty), true);
            //            return;
            //        }
            //        await next();
            //    });
            //}

            // Add Auth
            app.UseAuthentication();

            // Add static files to the request pipeline
            app.UseStaticFiles();

            // Configure Session.
            app.UseSession();

            // Add Localization
            app.UseRequestLocalization(
                app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value);

            app.UseMvc(routes =>
            {
                routes.MapRoute("areaRoute", "{area:exists}/{controller}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "login",
                    template: "Login",
                    defaults: new { controller = "Users", action = "Login" });

                routes.MapRoute(
                    name: "logout",
                    template: "Logout",
                    defaults: new { controller = "Users", action = "Logout" });

                routes.MapRoute(
                    name: "api",
                    template: "{area:exists}/{controller=Default}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Dashboard}/{action=Index}/{id?}");
            });
        }

        private bool RedirectIsApi(RedirectContext<CookieAuthenticationOptions> context)
        {
            return context.Request.Path.StartsWithSegments(new PathString("/Api")) || (context.Request.Headers.ContainsKey("X-Requested-With") && context.Request.Headers["X-Requested-With"].Equals("XMLHttpRequest"));
        }
    }
}
