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

            // Claims provide role NAMES, but permissions table stores Role IDs. Map names -> IDs.
            var roleNames = context.User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            if (!roleNames.Any())
            {
                context.Response.Redirect("/Home/AccessDenied");
                return;
            }

            var roleIds = await db.Roles
                .Where(r => roleNames.Contains(r.Name))
                .Select(r => r.Id)
                .ToListAsync();

            if (!roleIds.Any())
            {
                context.Response.Redirect("/Home/AccessDenied");
                return;
            }

            // Bypass for self-service employee actions (do not require explicit permission rows)
            var selfServiceEmployeeActions = new HashSet<(string Controller,string Action)>(new [] {
                ("Employee","Profile"),
                ("Employee","MyLeaveRequests"),
                ("Employee","RequestLeave"),
                ("Employee","CancelLeaveRequest"),
                ("Employee","CalculateLeaveDays"),
                ("Employee","GetAvailableBalance")
            }, new TupleIgnoreCaseComparer());

            if (selfServiceEmployeeActions.Contains((controller, action)) && context.User.IsInRole("Employee"))
            {
                await _next(context);
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

// Case-insensitive comparer for (controller, action) tuple
internal sealed class TupleIgnoreCaseComparer : IEqualityComparer<(string Controller,string Action)>
{
    public bool Equals((string Controller, string Action) x, (string Controller, string Action) y)
        => string.Equals(x.Controller, y.Controller, StringComparison.OrdinalIgnoreCase)
           && string.Equals(x.Action, y.Action, StringComparison.OrdinalIgnoreCase);
    public int GetHashCode((string Controller, string Action) obj)
        => HashCode.Combine(obj.Controller?.ToLowerInvariant(), obj.Action?.ToLowerInvariant());
}
}