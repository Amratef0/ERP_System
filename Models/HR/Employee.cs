using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Models.ValidationAttributes;
using ERP_System_Project.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ERP_System_Project.Models.Enums;
using ERP_System_Project.Models.Authentication;

namespace ERP_System_Project.Models.HR
{
    public class Employee : ISoftDeletable
    {
        [Key]
        [Display(Name = "Employee ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
        [MinLength(2, ErrorMessage = "First name must be at least 2 characters long.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
        [MinLength(2, ErrorMessage = "Last name must be at least 2 characters long.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Date of Birth")]
        public DateOnly DateOfBirth { get; set; }

        [Display(Name = "Gender")]
        public Gender Gender { get; set; }

        [Display(Name = "National ID")]
        public string? NationalId { get; set; }

        [Display(Name = "Passport Number")]
        public string? PassportNumber { get; set; }

        [Required(ErrorMessage = "Hire date is required.")]
        [Display(Name = "Hire Date")]
        public DateOnly HireDate { get; set; }

        [Display(Name = "Termination Date")]
        public DateOnly? TerminationDate { get; set; }

        [MaxLength(255, ErrorMessage = "Work email cannot exceed 255 characters.")]
        [MinLength(5, ErrorMessage = "Work email must be at least 5 characters long.")]
        [Display(Name = "Work Email")]
        public string WorkEmail { get; set; }

        [MaxLength(50, ErrorMessage = "Work phone cannot exceed 50 characters.")]
        [MinLength(5, ErrorMessage = "Work phone must be at least 5 characters long.")]
        [Display(Name = "Work Phone")]
        public string WorkPhone { get; set; }

        [MaxLength(255, ErrorMessage = "Personal email cannot exceed 255 characters.")]
        [MinLength(5, ErrorMessage = "Personal email must be at least 5 characters long.")]
        [Display(Name = "Personal Email")]
        public string? PersonalEmail { get; set; }

        [MaxLength(50, ErrorMessage = "Personal phone cannot exceed 50 characters.")]
        [MinLength(5, ErrorMessage = "Personal phone must be at least 5 characters long.")]
        [Display(Name = "Personal Phone")]
        public string? PersonalPhone { get; set; }

        [MaxLength(255, ErrorMessage = "Emergency contact name cannot exceed 255 characters.")]
        [MinLength(2, ErrorMessage = "Emergency contact name must be at least 2 characters long.")]
        [Display(Name = "Emergency Contact Name")]
        public string? EmergencyContactName { get; set; }

        [MaxLength(50, ErrorMessage = "Emergency contact phone cannot exceed 50 characters.")]
        [MinLength(5, ErrorMessage = "Emergency contact phone must be at least 5 characters long.")]
        [Display(Name = "Emergency Contact Phone")]
        public string? EmergencyContactPhone { get; set; }

        [Required(ErrorMessage = "Base salary is required.")]
        [DecimalPrecisionScale(15, 4)]
        [Display(Name = "Base Salary")]
        public decimal BaseSalary { get; set; }

        [MaxLength(50, ErrorMessage = "Bank account number cannot exceed 50 characters.")]
        [MinLength(1, ErrorMessage = "Bank account number must be at least 1 character long.")]
        [Display(Name = "Bank Account Number")]
        public string? BankAccountNumber { get; set; }

        [MaxLength(50, ErrorMessage = "Bank name cannot exceed 50 characters.")]
        [MinLength(1, ErrorMessage = "Bank name must be at least 1 character long.")]
        [Display(Name = "Bank Name")]
        public string? BankName { get; set; }

        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }

        public string? ImageURL { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Deleted At")]
        public DateOnly? DeletedAt { get; set; }

        // Navigation Properties
        [ForeignKey("ApplicationUser")]
        [Display(Name = "Application User")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }


        [ForeignKey("Branch")]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
        public virtual Branch Branch { get; set; }

        [ForeignKey("Department")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        [ForeignKey("Type")]
        [Display(Name = "Employee Type")]
        public int TypeId { get; set; }
        public virtual EmployeeType Type { get; set; }

        [ForeignKey("JobTitle")]
        [Display(Name = "Job Title")]
        public int JobTitleId { get; set; }
        public virtual JobTitle JobTitle { get; set; }

        [ForeignKey("SalaryCurrence")]
        [Display(Name = "Salary Currency")]
        public int? SalaryCurrencyId { get; set; }
        public virtual Currency? SalaryCurrency { get; set; }

        [ForeignKey("Address")]
        [Display(Name = "Address")]
        public int AddressId { get; set; }
        public virtual Address Address { get; set; }

        [Display(Name = "Inventory Transactions")]
        public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();

        [Display(Name = "Requested Requisitions")]
        public virtual ICollection<InventoryRequisition> RequestedRequisitions { get; set; } = new List<InventoryRequisition>();

        [Display(Name = "Approved Requisitions")]
        public virtual ICollection<InventoryRequisition> ApprovedRequisitions { get; set; } = new List<InventoryRequisition>();

        [Display(Name = "Payroll Entries")]
        public virtual ICollection<PayrollEntry> PayrollEntries { get; set; } = new List<PayrollEntry>();

        [Display(Name = "Leave Requests")]
        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

        [Display(Name = "Approved Team Leave Requests")]
        public virtual ICollection<LeaveRequest> ApprovedTeamLeaveRequests { get; set; } = new List<LeaveRequest>();

        [Display(Name = "Employee Leave Balances")]
        public virtual ICollection<EmployeeLeaveBalance> EmployeeLeaveBalances { get; set; } = new List<EmployeeLeaveBalance>();
    }
}