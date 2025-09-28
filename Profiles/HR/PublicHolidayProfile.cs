using AutoMapper;
using ERP_System_Project.Models.HR;
using ERP_System_Project.ViewModels.HR;

namespace ERP_System_Project.Profiles.HR
{
    public class PublicHolidayProfile : Profile
    {
        public PublicHolidayProfile()
        {
            CreateMap<PublicHolidayCountriesVM, PublicHoliday>().ReverseMap();
        }
    }
}
