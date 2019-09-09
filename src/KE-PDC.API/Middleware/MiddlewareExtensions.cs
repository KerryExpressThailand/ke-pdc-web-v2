using Microsoft.AspNetCore.Builder;

namespace KE_PDC.API.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseSecurityHeadersMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SecurityHeadersMiddleware>();
        }
    }
}
