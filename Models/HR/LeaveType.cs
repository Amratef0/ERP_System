using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.HR
{
    public class LeaveType
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Leave Type Name is required.")]
        [MaxLength(100, ErrorMessage = "Leave Type Name cannot exceed 100 characters.")]
        [MinLength(2, ErrorMessage = "Leave Type Name must be at least 2 characters.")]
        [Display(Name = "Leave Type Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Leave Type Code is required.")]
        [MaxLength(50, ErrorMessage = "Leave Type Code cannot exceed 50 characters.")]
        [MinLength(1, ErrorMessage = "Leave Type Code must be at least 1 character.")]
        [Display(Name = "Leave Type Code")]
        public string Code { get; set; }

        [Display(Name = "Maximum Days Per Year")]
        public int? MaxDaysPerYear { get; set; }

        [Display(Name = "Is Paid Leave")]
        public bool IsPaid { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        // Navigation properties
        [Display(Name = "Leave Requests")]
        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

        [Display(Name = "Employee Leave Balances")]
        public ICollection<EmployeeLeaveBalance> EmployeeLeaveBalances { get; set; } = new List<EmployeeLeaveBalance>();
    }
}
