using KE_PDC.Models;
using KE_PDC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

namespace KE_PDC.API.Middleware
{
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly AppSettings _settings;
        //private IHostingEnvironment _hostingEnvironment;
        private KE_POSContext DB;

        public SecurityHeadersMiddleware(RequestDelegate next, KE_POSContext db, IOptions<AppSettings> settings, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<SecurityHeadersMiddleware>();
            _settings = settings.Value;
            //_hostingEnvironment = env;

            DB = db;
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInformation("Handling request: " + context.Request.Path);

            ApiResponse Response = new ApiResponse();
            Hasher Hasher = new Hasher(_settings.ApiKey);

            // Test
            string key = Hasher.Make("TestKey"); 

            IHeaderDictionary headers = context.Request.Headers;

            if (headers.ContainsKey("X-Auth-Token-Key") && headers.ContainsKey("X-Auth-App-Id"))
            {
                if (headers["X-Auth-App-Id"].ToString() != null && Hasher.Check("TestKey", headers["X-Auth-Token-Key"]))
                {
                    Response.Success = true;
                }
                else
                {
                    Response.Errors.Add(new
                    {
                        Message = "Unknown X-Auth-Token-Key or X-Auth-App-Id"
                    });
                }
            }
            else
            {
                if (!headers.ContainsKey("X-Auth-App-Id"))
                {
                    Response.Errors.Add(new
                    {
                        Message = "Missing X-Auth-App-Id header"
                    });
                }

                if (!headers.ContainsKey("X-Auth-Token-Key"))
                {
                    Response.Errors.Add(new
                    {
                        Message = "Missing X-Auth-Token-Key header"
                    });
                }
            }

            if (!Response.Success)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(Response.Render())).ConfigureAwait(true);
                return;
            }

            await _next.Invoke(context);

            _logger.LogInformation("Finished handling request.");
        }
    }
}
