using ERP_System_Project.Models.Core;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.ViewModels.Core
{
    public class BranchVM
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Branch Name")]
        public string Name { get; set; } = null!;

        [Display(Name = "Branch Code")]
        public string Code { get; set; } = null!;

        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Display(Name = "Is Main Branch?")]
        public bool IsMainBranch { get; set; } = false;

        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Address Line")]
        public string Line1 { get; set; } = null!;

        [Display(Name = "City")]
        public string City { get; set; } = null!;

        [Display(Name = "State/Province")]
        public string? StateProvince { get; set; }

        [Display(Name = "Postal Code")]
        public string? PostalCode { get; set; }

        [Display(Name = "Address Type")]
        public string? AddressType { get; set; }

        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Display(Name = "Country")]
        public string? Country { get; set; }

        public IEnumerable<Country>? Countries { get; set; }
    }
}
