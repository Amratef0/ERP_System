using AutoMapper;
using ERP_System_Project.Models.Core;
using ERP_System_Project.ViewModels.Core;

namespace ERP_System_Project.Profiles.Core
{
    public class BranchProfile : Profile
    {
        public BranchProfile()
        {
            CreateMap<Branch, BranchVM>()
                .ForMember(dest => dest.Line1, opt => opt.MapFrom(src => src.Address.Line1))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.StateProvince, opt => opt.MapFrom(src => src.Address.StateProvince))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.Address.PostalCode))
                .ForMember(dest => dest.AddressType, opt => opt.MapFrom(src => src.Address.AddressType))
                .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.Address.CountryId));

            CreateMap<BranchVM, Branch>()
                .ForPath(dest => dest.Address.Line1, opt => opt.MapFrom(src => src.Line1))
                .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
                .ForPath(dest => dest.Address.StateProvince, opt => opt.MapFrom(src => src.StateProvince))
                .ForPath(dest => dest.Address.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
                .ForPath(dest => dest.Address.AddressType, opt => opt.MapFrom(src => src.AddressType))
                .ForPath(dest => dest.Address.CountryId, opt => opt.MapFrom(src => src.CountryId));
        }
    }
}
