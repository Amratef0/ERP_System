
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models
{
    public class EnterTokenViewModel
    {
        [Required(ErrorMessage = "Please enter the token sent to your email.")]
        public string Token { get; set; }
    }
}