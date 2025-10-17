using AutoMapper;
using ERP_System_Project.Models.HR;
using ERP_System_Project.ViewModels.HR;

namespace ERP_System_Project.Profiles.HR
{
    public class EmployeeLeaveBalanceProfile : Profile
    {
        public EmployeeLeaveBalanceProfile()
        {
            CreateMap<EmployeeLeaveBalance, EmployeeLeaveBalanceVM>()
                .ForMember(dest => dest.EmployeeName, 
                    opt => opt.MapFrom(src => src.Employee.FirstName + " " + src.Employee.LastName))
                .ForMember(dest => dest.LeaveTypeName, 
                    opt => opt.MapFrom(src => src.LeaveType.Name))
                .ForMember(dest => dest.BranchName, 
                    opt => opt.MapFrom(src => src.Employee.Branch != null ? src.Employee.Branch.Name : null))
                .ForMember(dest => dest.DepartmentName, 
                    opt => opt.MapFrom(src => src.Employee.Department != null ? src.Employee.Department.Name : null))
                .ForMember(dest => dest.JobTitleName, 
                    opt => opt.MapFrom(src => src.Employee.JobTitle != null ? src.Employee.JobTitle.Name : null))
                .ForMember(dest => dest.EmployeeTypeName, 
                    opt => opt.MapFrom(src => src.Employee.Type != null ? src.Employee.Type.Name : null))
                .ReverseMap()
                .ForMember(dest => dest.Employee, opt => opt.Ignore())
                .ForMember(dest => dest.LeaveType, opt => opt.Ignore());
        }
    }
}
