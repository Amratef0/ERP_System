using AutoMapper;
using ERP_System_Project.Models.HR;
using ERP_System_Project.ViewModels.HR;

namespace ERP_System_Project.Profiles.HR
{
    public class AttendanceProfile : Profile
    {
        public AttendanceProfile()
        {
            CreateMap<AttendanceRecord, EmployeeAttendanceRecordVM>()
                .ForMember(dest => dest.AttendanceRecordId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.CheckInTime, opt => opt.MapFrom(src => src.CheckInTime))
                .ForMember(dest => dest.CheckOutTime, opt => opt.MapFrom(src => src.CheckOutTime))
                .ForMember(dest => dest.TotalHours, opt => opt.MapFrom(src => src.TotalHours))
                .ForMember(dest => dest.OverTimeHours, opt => opt.MapFrom(src => src.OverTimeHours))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))

                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Employee.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.Employee.FirstName} {src.Employee.LastName}"))
                .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Employee.Branch.Name))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Employee.Department.Code))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Employee.Type.Name))
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.Employee.JobTitle.Name));
        }
    }
}
