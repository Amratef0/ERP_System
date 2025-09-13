using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models
{
    public class Branch
    {
        [Key]
        public int Branch_Id { get; set; }

        [Required(ErrorMessage = "Branch Name Is Required")]
        [StringLength(100, ErrorMessage = "Branch Name Must be less than 100 characters")]
        public string Branch_Name { get; set; } = null!;

        [Required(ErrorMessage = "Branch Code Is Required")]
        [StringLength(10, ErrorMessage = "Branch Code Must be less than 10 characters")]
        public string Branch_Code { get; set;} = null!;

        [StringLength(50, ErrorMessage = "Phone Number Must be less than 50 Number")]
        [Phone(ErrorMessage = "Invalid Phone Format")]
        public string? Phone_Number { get; set; }

        [StringLength(50, ErrorMessage = "Email Must be less than 30 characters")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string? Email { get; set; }
        public bool Is_Main_Branch { get; set; } = false;
        public bool Is_Active { get; set;} = true;

        [ForeignKey("Address")]
        public int Address_Id { get; set; }
        public Address Address { get; set; }

        public ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();

    }
}
