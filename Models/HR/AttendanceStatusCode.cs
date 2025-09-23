using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.HR
{
    public class AttendanceStatusCode
    {
        [Key]
        [Display(Name = "Status ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(30, ErrorMessage = "Name cannot exceed 30 characters.")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
        [Display(Name = "Status Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Code is required.")]
        [MaxLength(10, ErrorMessage = "Code cannot exceed 10 characters.")]
        [MinLength(1, ErrorMessage = "Code must be at least 1 character long.")]
        [Display(Name = "Status Code")]
        public string Code { get; set; }

        [MaxLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Deleted At")]
        public DateOnly? DeletedAt { get; set; }

        // Navigation properties
        [Display(Name = "Attendance Records")]
        public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
    }
}
