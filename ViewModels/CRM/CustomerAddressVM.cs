using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.ViewModels.CRM
{
    public class CustomerAddressVM
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(100, ErrorMessage = "Country must be {1} characters or fewer.")]
        public string Country { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required.")]
        [MaxLength(50, ErrorMessage = "City must less than 50 CHar")]
        [MinLength (2, ErrorMessage = "City must be at least 2 characters.")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Street is required.")]

        [MaxLength(100, ErrorMessage = "Street must less than 100 CHar")]
        [MinLength(2, ErrorMessage = "Street must be at least 2 characters.")]
        public string Street { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Building number must be a positive integer .")]
        public int BuildingNumber { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Apartment number must be a positive integer.")]
        public int ApartmentNumber { get; set; }
        public int NumOfShippingOrders{ get; set; }
        public int NumOfBillingOrders { get; set; }

        
    }
}
