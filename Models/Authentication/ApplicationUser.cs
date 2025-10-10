using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.ECommerece;
using ERP_System_Project.Models.HR;
using ERP_System_Project.Models.ValidationAttributes;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public virtual Customer? Customer { get; set; }

        public virtual Employee? Employee { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [DateOfBirthValidation(15)]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public override string UserName
        {
            get => $"{FirstName} {LastName}";
            set => base.UserName = value;
        }
    }
}
