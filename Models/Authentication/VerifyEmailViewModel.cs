using System.ComponentModel.DataAnnotations;
namespace ERP_System_Project.Models.Authentication
{
    public class VerifyEmailViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}