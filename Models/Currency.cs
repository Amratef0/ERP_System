using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.Marshalling;

namespace ERP_System_Project.Models
{
    public class Currency
    {
        [Key]
        public int Currency_Id { get; set; }

        [Required(ErrorMessage = "Currency Code Is Required")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency Code Must be 3 characters")]
        public string Currency_Code { get; set; } = null!;

        [Required(ErrorMessage = "Currency Name Is Required")]
        [StringLength(30, ErrorMessage = "Currency Name must be less than 30 characters")]
        public string Currency_Name { get; set;} = null!;

        [Required(ErrorMessage = "Symbol Is Required")]
        [StringLength(5, ErrorMessage = "Symbol must be less than 5 characters")]
        public string Symbol { get; set;} = null!;
        public bool Is_Active { get; set; } = true;
    }
}
