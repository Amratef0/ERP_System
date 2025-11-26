namespace ERP_System_Project.Models.Authentication
{
    using System.ComponentModel.DataAnnotations;
    using ERP_System_Project.Models.ValidationAttributes;

    public class EditProfileViewModel
    {
        [Required]
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

       
    }

}
