using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.Authentication
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}