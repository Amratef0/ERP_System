using AutoMapper;
using ERP_System_Project.Models.CRM;
using ERP_System_Project.ViewModels.CRM;

namespace ERP_System_Project.Profiles.CRM
{
    public class CustomerTypeProfile : Profile
    {
        public CustomerTypeProfile()
        {
            // Entity -> VM
            CreateMap<CustomerType, CustomerTypeVM>()
                // set CustomerCount from the navigation collection (guard null)
                .ForMember(dest => dest.CustomerCount,
                           opt => opt.MapFrom(src => src.Customers != null ? src.Customers.Count() : 0));

            // VM -> Entity 
            CreateMap<CustomerTypeVM, CustomerType>()
                .ForMember(dest => dest.Customers, opt => opt.Ignore())
                .ReverseMap(); // reverse mapping will reuse the CustomerCount mapping defined above

        }
    }
}