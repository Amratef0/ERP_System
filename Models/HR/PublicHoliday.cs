using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_System_Project.Models.HR
{
    public class PublicHoliday : ISoftDeletable
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Holiday Name")]
        public string Name { get; set; }

        [Display(Name = "Date")]
        public DateOnly Date { get; set; }

        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Deleted At")]
        public DateOnly? DeletedAt { get; set; }

        // Navigation Properties
        [ForeignKey("Country")]
        [Display(Name = "Country")]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
    }
}
