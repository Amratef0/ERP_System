using ERP_System_Project.Models.Authentication;
using ERP_System_Project.Models.ECommerce;
using ERP_System_Project.Models.ECommerece;
using ERP_System_Project.Models.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.CRM
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string CustomerCode { get; set; }

        [Required(ErrorMessage = "Enter First Name")]
        [MaxLength(50)]
        [MinLength(3, ErrorMessage = "First Name at least 3")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter Last Name")]
        [MaxLength(50)]
        [MinLength(3, ErrorMessage = "Last Name at least 3")]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [DateOfBirthValidation(18)]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        [DataType(DataType.DateTime)]
        public DateTime? LastLoginDate { get; set; } = DateTime.Now;


        [DataType(DataType.DateTime)]
        public DateTime? ModifiedDate { get; set; }

        public bool IsActive { get; set; } = true;
        [DataType(DataType.DateTime)]
        public DateTime? DeactivatedAt { get; set; }




        // Foreign key for ApplicationUser (Identity)
        public string? ApplicationUserId { get; set; }
        public int? CustomerTypeId { get; set; }

        // Navigation properties  (remove virtual if not lazy loading!)
        public virtual ApplicationUser? ApplicationUser { get; set; }
        // add Default Value in fluent API
        public virtual CustomerType? CustomerType { get; set; }

        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; } = new HashSet<CustomerAddress>();
        public virtual ICollection<CustomerFavorite> CustomerFavorites { get; set; } = new HashSet<CustomerFavorite>();

        //public virtual ShoppingCart ShoppingCart { get; set; }
        public virtual ICollection<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>(); // one to many or many yo many?????
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<CustomerReview> Reviews { get; set; } = new HashSet<CustomerReview>();
        public virtual ICollection<CustomerWishlist> Wishlists { get; set; } = new HashSet<CustomerWishlist>();


    }
}
