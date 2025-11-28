using System.Collections.Generic;

namespace ERP_System_Project.ViewModels.User
{
    public class PermissionVM
    {
        public string RoleId { get; set; }
        public List<PermissionItem> Permissions { get; set; } = new();
    }

    public class PermissionItem
    {
        public string Module { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool Allowed { get; set; }
    }




}
