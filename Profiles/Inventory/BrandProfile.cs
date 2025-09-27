using AutoMapper;
using ERP_System_Project.Models.Inventory;
using ERP_System_Project.ViewModels.Inventory;

namespace ERP_System_Project.Profiles.Inventory
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<BrandVM,Brand>().ReverseMap();
        }
    }
}
