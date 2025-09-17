using ERP_System_Project.Models.ECommerece;
using ERP_System_Project.Models.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.ECommerce
{
    public class OrderStatusCode
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Code Is Required")]
        [StringLength(50, ErrorMessage = "Code Must Be Less Than 50 Characters")]
        public string Code { get; set; } = null!;

        [Required(ErrorMessage = "Name Is Required")]
        [StringLength(100, ErrorMessage = "Name Must Be Less Than 100 Characters")]
        public string Name { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Category Name Must Be Less Than 500 Characters")]
        public string? Description { get; set; }

        [DecimalPrecisionScale(19,4)]
        public decimal Cost { get; set; }
        public bool IsActive { get; set; } = true;


        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
