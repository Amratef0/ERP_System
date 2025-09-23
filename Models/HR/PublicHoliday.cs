using ERP_System_Project.Models.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.HR
{
    public class PublicHoliday
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Holiday name is required.")]
        [MaxLength(100, ErrorMessage = "Holiday name cannot exceed 100 characters.")]
        [MinLength(2, ErrorMessage = "Holiday name must be at least 2 characters.")]
        [Display(Name = "Holiday Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please select a date for the holiday.")]
        [Display(Name = "Date")]
        public DateOnly Date { get; set; }

        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Deleted At")]
        public DateOnly? DeletedAt { get; set; }

        // Navigation Properties
        [Required(ErrorMessage = "Please select a country.")]
        [ForeignKey("Country")]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        public Country Country { get; set; }
    }
}
