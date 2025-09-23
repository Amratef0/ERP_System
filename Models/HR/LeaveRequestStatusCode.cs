using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.HR
{
    public class LeaveRequestStatusCode
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Status Name is required.")]
        [MaxLength(100, ErrorMessage = "Status Name cannot exceed 100 characters.")]
        [MinLength(2, ErrorMessage = "Status Name must be at least 2 characters.")]
        [Display(Name = "Status Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Status Code is required.")]
        [MaxLength(50, ErrorMessage = "Status Code cannot exceed 50 characters.")]
        [MinLength(1, ErrorMessage = "Status Code must be at least 1 character.")]
        [Display(Name = "Status Code")]
        public string Code { get; set; }

        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Deleted At")]
        public DateOnly? DeletedAt { get; set; }

        // Navigation properties
        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
    }
}
