using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.Enums;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Models.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.ViewModels.HR
{
    public class EmployeeVM
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

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

        [Display(Name = "Hire Date")]
        public DateOnly HireDate { get; set; }

        [Display(Name = "Termination Date")]
        public DateOnly? TerminationDate { get; set; }

        [Display(Name = "Work Email")]
        public string WorkEmail { get; set; }

        [Display(Name = "Work Phone")]
        public string WorkPhone { get; set; }

        [Display(Name = "Personal Email")]
        public string? PersonalEmail { get; set; }

        [Display(Name = "Personal Phone")]
        public string? PersonalPhone { get; set; }

        [Display(Name = "Emergency Contact Name")]
        public string? EmergencyContactName { get; set; }

        [Display(Name = "Emergency Contact Phone")]
        public string? EmergencyContactPhone { get; set; }

        [DecimalPrecisionScale(15, 4)]
        [Display(Name = "Base Salary")]
        public decimal BaseSalary { get; set; }

        [Display(Name = "Bank Account Number")]
        public string? BankAccountNumber { get; set; }

        [Display(Name = "Bank Name")]
        public string? BankName { get; set; }

        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }

        public IFormFile? Image { get; set; }

        public string? ImageURL { get; set; }

        public IFormFile? NewImage { get; set; }

        public bool RemoveImage { get; set; }

        [Display(Name = "Line 1")]
        public string Line1 { get; set; }

        [Display(Name = "Line 2")]
        public string? Line2 { get; set; }

        public string City { get; set; }

        [Display(Name = "State Province")]
        public string? StateProvince { get; set; }

        [Display(Name = "Postal Code")]
        public string? PostalCode { get; set; }

        [Display(Name = "Address Type")]
        public string? AddressType { get; set; }

        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Display(Name = "Country")]
        public string? Country { get; set; }

        [Display(Name = "Application User")]
        public string ApplicationUserId { get; set; }

        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Display(Name = "Branch")]
        public string? Branch { get; set; }

        [Display(Name = "Employee Type")]
        public int TypeId { get; set; }

        [Display(Name = "Employee Type")]
        public string? Type { get; set; }

        [Display(Name = "Job Title")]
        public int JobTitleId { get; set; }

        [Display(Name = "Job Title")]
        public string? JobTitle { get; set; }

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Display(Name = "Department")]
        public string? Department { get; set; }

        [Display(Name = "Salary Currency")]
        public int? SalaryCurrencyId { get; set; }

        [Display(Name = "Salary Currency")]
        public string? SalaryCurrency { get; set; }

        public IEnumerable<Country> Countries { get; set; } = new List<Country>();

        public IEnumerable<Branch> Branches { get; set; } = new List<Branch>();

        public IEnumerable<EmployeeType> EmployeeTypes { get; set; } = new List<EmployeeType>();

        public IEnumerable<JobTitle> JobTitles { get; set; } = new List<JobTitle>();

        public IEnumerable<Department> Departments { get; set; } = new List<Department>();

        public IEnumerable<Currency> Currencies { get; set; } = new List<Currency>();
    }
}
