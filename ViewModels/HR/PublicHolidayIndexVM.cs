using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.HR;

namespace ERP_System_Project.ViewModels.HR
{
    public class PublicHolidayIndexVM
    {
        public IEnumerable<PublicHoliday> PublicHolidays { get; set; }
        public IEnumerable<Country> Countries { get; set; }
    }
}
