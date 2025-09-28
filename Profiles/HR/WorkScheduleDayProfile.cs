using AutoMapper;
using ERP_System_Project.Models.HR;
using ERP_System_Project.ViewModels.HR;

namespace ERP_System_Project.Profiles.HR
{
    public class WorkScheduleDayProfile : Profile
    {
        public WorkScheduleDayProfile()
        {
            CreateMap<WorkScheduleDayVM, WorkScheduleDay>().ReverseMap();
        }
    }
}
