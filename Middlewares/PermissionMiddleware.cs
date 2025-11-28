using System.Security.Claims;
using ERP_System_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Middlewares
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;

        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, Erpdbcontext db)
        {
            if (context.Request.Path.StartsWithSegments("/lib") ||
                context.Request.Path.StartsWithSegments("/css") ||
                context.Request.Path.StartsWithSegments("/js") ||
                context.Request.Path.StartsWithSegments("/images"))
            {
                await _next(context);
                return;
            }

            if (context.Request.Path.StartsWithSegments("/Account"))
            {
                await _next(context);
                return;
            }

            if (context.Request.Path.StartsWithSegments("/Home"))
            {
                await _next(context);
                return;
            }

            var endpoint = context.GetEndpoint();
            if (endpoint == null)
            {
                await _next(context);
                return;
            }

            string controller = context.Request.RouteValues["controller"]?.ToString() ?? "";
            string action = context.Request.RouteValues["action"]?.ToString() ?? "";

            var publicPages = new List<(string Controller, string Action)>
            {
                ("Market", "Index"),
                
            };

            if (publicPages.Any(p =>
                string.Equals(p.Controller, controller, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(p.Action, action, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }

            if (!context.User.Identity.IsAuthenticated)
            {
                context.Response.Redirect("/Account/Login");
                return;
            }

            if (context.User.IsInRole("Admin"))
            {
                await _next(context);
                return;
            }

            var roleIds = context.User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roleIds.Any())
            {
                context.Response.Redirect("/Home/AccessDenied");
                return;
            }

            var allowed = await db.Permissions
                .AsNoTracking()
                .AnyAsync(p =>
                    roleIds.Contains(p.RoleId) &&
                    p.Controller.ToLower() == controller.ToLower() &&
                    p.Action.ToLower() == action.ToLower()
                );

            if (!allowed)
            {
                context.Response.Redirect("/Home/AccessDenied");
                return;
            }

            await _next(context);
        }
    }
}