using ERP_System_Project.Models.Core;
using System.ComponentModel.DataAnnotations;

namespace ERP_System_Project.ViewModels.HR
{
    public class PublicHolidayCountriesVM
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Holiday Name")]
        public string Name { get; set; }

        [Display(Name = "Date")]
        public DateOnly Date { get; set; }

        [Display(Name = "Country")]
        public int CountryId { get; set; }

        public IEnumerable<Country>? Countries { get; set; }
    }
}
