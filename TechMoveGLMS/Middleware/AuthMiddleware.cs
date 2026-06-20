using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace TechMoveGLMS.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Paths that don't require authentication
            var publicPaths = new[]
            {
                "/Account/Login",
                "/Account/Register",
                "/css/",
                "/js/",
                "/lib/",
                "/favicon.ico"
            };

            var path = context.Request.Path.ToString();
            bool isPublicPath = false;

            foreach (var publicPath in publicPaths)
            {
                if (path.StartsWith(publicPath, System.StringComparison.OrdinalIgnoreCase))
                {
                    isPublicPath = true;
                    break;
                }
            }

            // Check if user is logged in
            var userId = context.Session.GetString("UserId");
            bool isAuthenticated = !string.IsNullOrEmpty(userId);

            // Redirect to login if not authenticated and trying to access protected page
            if (!isAuthenticated && !isPublicPath)
            {
                context.Response.Redirect("/Account/Login?returnUrl=" + path);
                return;
            }

            await _next(context);
        }
    }
}