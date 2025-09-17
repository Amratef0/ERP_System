using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Models.ValidationAttributes;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ERP_System_Project.Models.HR
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(2)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(2)]
        public string LastName { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public char? Gender { get; set; }

        public string? NationalId { get; set; }

        public string? PassportNumber { get; set; }

        [Required]
        public DateOnly HireDate { get; set; }

        public DateOnly? TerminationDate { get; set; }

        [MaxLength(255)]
        [MinLength(5)]
        public string? WorkEmail { get; set; }

        [MaxLength(50)]
        [MinLength(5)]
        public string? WorkPhone { get; set; }

        [MaxLength(255)]
        [MinLength(5)]
        public string? PersonalEmail { get; set; }

        [MaxLength(50)]
        [MinLength(5)]
        public string? PersonalPhone { get; set; }

        [MaxLength(255)]
        [MinLength(2)]
        public string? EmergencyContactName { get; set; }

        [MaxLength(50)]
        [MinLength(5)]
        public string? EmergencyContactPhone { get; set; }

        [MaxLength(100)]
        [MinLength(2)]
        public string? EmergencyContactRelation { get; set; }

        [Required]
        [DecimalPrecisionScale(15, 4)]
        public decimal BaseSalary { get; set; }

        [MaxLength(50)]
        [MinLength(1)]
        public string? BankAccountNumber { get; set; }

        [MaxLength(50)]
        [MinLength(1)]
        public string? BankName { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        // Navigation Properties

        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        [ForeignKey("Type")]
        public int TypeId { get; set; }
        public EmployeeType Type { get; set; }

        [ForeignKey("JobTitle")]
        public int JobTitleId { get; set; }
        public JobTitle JobTitle { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        [ForeignKey("Manager")]
        public int? ManagerId { get; set; }
        public Employee? Manager { get; set; }

        [ForeignKey("SalaryCurrence")]
        public int? SalaryCurrencyId { get; set; }
        public Currency? SalaryCurrency { get; set; }

        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        public Address? Address { get; set; }

        public ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();
        public ICollection<InventoryRequisition> RequestedRequisitions { get; set; } = new List<InventoryRequisition>();
        public ICollection<InventoryRequisition> ApprovedRequisitions { get; set; } = new List<InventoryRequisition>();
    }
}
