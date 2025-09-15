using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.Models.HR
{
    public class LeaveRequestStatusCode
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        public string Code { get; set; }

        // Rest of navigation properties can be added here as needed
    }
}
