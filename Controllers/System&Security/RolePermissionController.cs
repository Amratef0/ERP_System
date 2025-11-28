using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ERP_System_Project.Services.Implementation.System_Security;
using ERP_System_Project.ViewModels.User;

public class RolePermissionController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly PermissionService _permissionService;

    public RolePermissionController(RoleManager<IdentityRole> roleManager, PermissionService permissionService)
    {
        _roleManager = roleManager;
        _permissionService = permissionService;
    }

    public IActionResult Manage(string roleId, string module = "")
    {
        ViewBag.Roles = _roleManager.Roles.ToList();
        ViewBag.SelectedModule = module ?? "";

        if (string.IsNullOrEmpty(roleId))
            return View(new PermissionVM());

        var vm = _permissionService.GetPermissions(roleId, module);
        return View(vm);
    }

    [HttpPost]
    public IActionResult SavePermissions(PermissionVM model, string currentModule = "", string actionType = "")
    {
        if (string.IsNullOrEmpty(model?.RoleId))
        {
            return Ok();
        }

        var allPermissions = _permissionService.GetPermissions(model.RoleId, "");

        if (!string.IsNullOrEmpty(actionType))
        {
            foreach (var perm in allPermissions.Permissions)
            {
                if (string.IsNullOrEmpty(currentModule) || perm.Module == currentModule)
                {
                    perm.Allowed = (actionType == "selectAll");
                }
            }

            model.Permissions = allPermissions.Permissions;
            _permissionService.SavePermissions(model);
        }
        else
        {
            _permissionService.SavePermissions(model);
        }

        return RedirectToAction("Manage", new { roleId = model.RoleId, module = currentModule });
    }


}