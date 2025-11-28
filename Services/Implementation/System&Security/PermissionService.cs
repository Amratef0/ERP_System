using ERP_System_Project.Models;
using ERP_System_Project.Models.Roles;
using ERP_System_Project.Services.Implementation.System_Security;
using ERP_System_Project.ViewModels.User;
using Microsoft.AspNetCore.Identity;

public class PermissionService
{
    private readonly Erpdbcontext _db;
    private readonly ReflectionService _reflection;
    private readonly RoleManager<IdentityRole> _roleManager;

    public PermissionService(Erpdbcontext db, ReflectionService reflection, RoleManager<IdentityRole> roleManager)
    {
        _db = db;
        _reflection = reflection;
        _roleManager = roleManager;
    }

    public PermissionVM GetPermissions(string roleId, string module = "")
    {
        if (string.IsNullOrEmpty(roleId)) return new PermissionVM();

        var all = _reflection.GetAllControllersAndActions();

        if (!string.IsNullOrEmpty(module))
            all = all.Where(x => x.Module == module).ToList();

        var roleName = _roleManager.FindByIdAsync(roleId).Result?.Name ?? "";
        var saved = _db.Permissions.Where(p => p.RoleId == roleId).ToList();

        return new PermissionVM
        {
            RoleId = roleId,
            Permissions = all.Select(x => new PermissionItem
            {
                Module = x.Module,
                Controller = x.Controller,
                Action = x.Action,
                Allowed = saved.Any(s => s.Controller == x.Controller && s.Action == x.Action)
            }).ToList()
        };
    }

    public void SavePermissions(PermissionVM model)
    {
        if (model == null || string.IsNullOrEmpty(model.RoleId) || model.Permissions == null)
            throw new ArgumentException("Invalid permission model.");

        var oldPermissions = _db.Permissions.Where(p => p.RoleId == model.RoleId).ToList();
        _db.Permissions.RemoveRange(oldPermissions);

        foreach (var p in model.Permissions)
        {
            if (p.Allowed)
            {
                _db.Permissions.Add(new Permission
                {
                    RoleId = model.RoleId,
                    Module = p.Module,
                    Controller = p.Controller,
                    Action = p.Action
                });
            }
        }

        _db.SaveChanges();
    }
}