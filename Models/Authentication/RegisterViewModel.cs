using ERP_System_Project.Models.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.Authentication
{
    public class RegisterViewModel
    {


        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "First Name cannot exceed 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "Last Name cannot exceed 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }




        [Required(ErrorMessage = "Date of Birth is required")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        //[DateOfBirthValidation(15, ErrorMessage = "You should be at least 15 year.")]
        public DateOnly DateOfBirth { get; set; }




        [Required(ErrorMessage = "Country is required")]
        [Display(Name = "Country")]
        [MaxLength(100)]
        public string Country { get; set; } // Changed from int CountryId to string Country

        [Required(ErrorMessage = "City is required")]
        [Display(Name = "City")]
        [MaxLength(100)]
        public string City { get; set; }

        [Required(ErrorMessage = "Street is required")]
        [Display(Name = "Street")]
        [MaxLength(255)]
        public string Street { get; set; } // Changed from AddressLine1 to Street

        [Required(ErrorMessage = "Building Number is required")]
        [Display(Name = "Building Number")]
        [Range(1, 10000, ErrorMessage = "Building number must be between 1 and 10000")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "Apartment Number is required")]
        [Display(Name = "Apartment Number")]
        [Range(1, 1000, ErrorMessage = "Apartment number must be between 1 and 1000")]
        public int ApartmentNumber { get; set; }







        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


    }

}