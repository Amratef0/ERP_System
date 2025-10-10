using AutoMapper;
using ERP_System_Project.Models.Core;
using ERP_System_Project.Models.HR;
using ERP_System_Project.ViewModels.Core;
using ERP_System_Project.ViewModels.HR;

namespace ERP_System_Project.Profiles.HR
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeVM>()
                .ForMember(dest => dest.Line1, opt => opt.MapFrom(src => src.Address.Line1))
                .ForMember(dest => dest.Line2, opt => opt.MapFrom(src => src.Address.Line2))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.StateProvince, opt => opt.MapFrom(src => src.Address.StateProvince))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.Address.PostalCode))
                .ForMember(dest => dest.AddressType, opt => opt.MapFrom(src => src.Address.AddressType))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.Address.CountryId))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Address.Country.Name))
                .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch.Name))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.SalaryCurrency, opt => opt.MapFrom(src => src.SalaryCurrency != null ? src.SalaryCurrency.Name : string.Empty))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.Name))
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.JobTitle.Name))
                .ForMember(dest => dest.Branches, opt => opt.Ignore())
                .ForMember(dest => dest.Departments, opt => opt.Ignore())
                .ForMember(dest => dest.EmployeeTypes, opt => opt.Ignore())
                .ForMember(dest => dest.JobTitles, opt => opt.Ignore())
                .ForMember(dest => dest.Countries, opt => opt.Ignore())
                .ForMember(dest => dest.Currencies, opt => opt.Ignore());

            CreateMap<EmployeeVM, Employee>()
                .ForPath(dest => dest.Address.Line1, opt => opt.MapFrom(src => src.Line1))
                .ForPath(dest => dest.Address.Line2, opt => opt.MapFrom(src => src.Line2))
                .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
                .ForPath(dest => dest.Address.StateProvince, opt => opt.MapFrom(src => src.StateProvince))
                .ForPath(dest => dest.Address.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
                .ForPath(dest => dest.Address.AddressType, opt => opt.MapFrom(src => src.AddressType))
                .ForPath(dest => dest.Address.CountryId, opt => opt.MapFrom(src => src.CountryId));
        }
    }
}
