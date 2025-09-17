using ERP_System_Project.Models.ECommerce;
using ERP_System_Project.Models.ECommerece;
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

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public DateTime LastLoginDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }

        // Foreign key for ApplicationUser (Identity)
        public string? ApplicationUserId { get; set; }
        public string? CustomerTypeId { get; set; }

        // Navigation properties  (remove virtual if not lazy loading!)
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser? ApplicationUser { get; set; }
        //public virtual CustomerType?  CustomerType{ get; set; }

        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; } = new HashSet<CustomerAddress>();
        public virtual ICollection<CustomerFavorite> CustomerFavorites { get; set; } = new HashSet<CustomerFavorite>();

        //public virtual ICollection<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();
        //public virtual ShoppingCart ShoppingCart { get; set; }
        //public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        //public virtual ICollection<Wishlist> Wishlists { get; set; } = new HashSet<Wishlist>();
        public virtual ICollection<CustomerType> CustomerTypes { get; set; } = new HashSet<CustomerType>(); // one to many or many yo many?????
        public virtual ICollection<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>(); // one to many or many yo many?????
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();




    }
}
