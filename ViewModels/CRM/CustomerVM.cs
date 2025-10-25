using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.ViewModels.CRM
{
    public class CustomerVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter First Name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "First Name must be between 3 and 50 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter Last Name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Last Name must be between 3 and 50 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        [Required(ErrorMessage = "Enter Email")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Enter a valid phone number")]
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Enter Date of Birth")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateOnly DateOfBirth { get; set; }

        [DataType(DataType.DateTime)]

        public DateTime? LastLoginDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime? DeactivatedAt { get; set; }

        // Foreign keys
        public string? ApplicationUserId { get; set; }

        [Display(Name = "Customer Type")]
        public int? CustomerTypeId { get; set; }

        public string? CustomerTypeName { get; set; }

        [Display(Name = "Address")]
        public string? MainAddress { get; set; }
        public int NumOfOrders { get; set; }



    }
}
