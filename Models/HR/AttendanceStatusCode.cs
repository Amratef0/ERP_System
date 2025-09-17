using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.HR
{
    public class AttendanceStatusCode
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        [MaxLength(10)]
        [MinLength(1)]
        public string Code { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        // Rest of navigation properties can be added here as needed
    }
}
