
using System.ComponentModel.DataAnnotations;
namespace ERP_System_Project.Models

{
    public class LoginUserViewModel
    {

        [Required]
        [Display(Name = "Username or Email")]
        public string UserNameOrEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }


    }
}