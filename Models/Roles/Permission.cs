using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.Roles
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        public string RoleId { get; set; }
        public string Module { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
