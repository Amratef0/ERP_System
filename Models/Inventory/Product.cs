using ERP_System_Project.Models.CRM;
using ERP_System_Project.Models.ECommerce;
using ERP_System_Project.Models.ValidationAttributes;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.Inventory
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product Name Is Required")]
        [StringLength(100, ErrorMessage = "Category Name Must Be Less Than 100 Characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Product Description Is Required")]
        public string Description { get; set;} = null!;

        [DecimalPrecisionScale(10, 2)]
        public decimal UnitCost { get; set; } = 0;

        [DecimalPrecisionScale(10,2)]
        public decimal StandardPrice { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }

        [ForeignKey("Brand")]
        public int BrandId { get; set; }
        public Brand Brand { get; set; } 
        
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }


        public ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
        public ICollection<ProductInventory> Warehouses { get; set; } = new List<ProductInventory>();
        public ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();
        public ICollection<InventoryRequisitionItem> InventoryRequestedProducts { get; set; } = new List<InventoryRequisitionItem>();
        public ICollection<OrderItem> OrderedItems { get; set; } = new List<OrderItem>();
        public ICollection<OfferProduct> Offers { get; set; } = new List<OfferProduct>();




        public ICollection < CustomerFavorite> CustomerFavorites { get; set; }= new List<CustomerFavorite>();
        public ICollection < CustomerWishlist>  CustomerWishlists{ get; set; }= new List<CustomerWishlist>();
        public ICollection <CustomerReview> CustomerReviews { get; set; } = new List<CustomerReview>();
        

    }
}
