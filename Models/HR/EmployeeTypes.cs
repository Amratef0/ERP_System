using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.HR
{
    public class EmployeeTypes
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        // Rest of navigation properties can be added here as needed
    }
}
