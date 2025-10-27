using System.Collections.Generic;

namespace ERP_System_Project.ViewModels.User
{
    public class UserViewModel
    {
        public string Id { get; set; } = string.Empty; // IdentityUser.Id is string
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int? EmployeeId { get; set; }
        public string? Job { get; set; }

        public IEnumerable<string> Roles { get; set; } = new List<string>();


        public List<string> AllRoles { get; set; } = new();
        public List<string> SelectedRoles { get; set; } = new();

    }
}
