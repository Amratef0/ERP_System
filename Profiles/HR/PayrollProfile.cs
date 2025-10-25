using AutoMapper;
using ERP_System_Project.Models.HR;
using ERP_System_Project.ViewModels.HR;

namespace ERP_System_Project.Profiles.HR
{
    public class PayrollProfile : Profile
    {
        public PayrollProfile()
        {
            // PayrollRun mappings
            CreateMap<PayrollRun, PayrollRunVM>()
                .ForMember(dest => dest.EmployeeCount, opt => opt.MapFrom(src => src.PayrollEntries.Count))
                .ForMember(dest => dest.CurrenciesUsed, opt => opt.Ignore())
                .ForMember(dest => dest.CurrencyTotals, opt => opt.Ignore());

            CreateMap<PayrollRun, PayrollRunDetailsVM>()
                .ForMember(dest => dest.Entries, opt => opt.MapFrom(src => src.PayrollEntries))
                .ForMember(dest => dest.CurrencyTotals, opt => opt.Ignore())
                .ForMember(dest => dest.CurrenciesUsed, opt => opt.Ignore());

            // PayrollEntry mappings
            CreateMap<PayrollEntry, PayrollEntryVM>()
                .ForMember(dest => dest.PayrollRunName, opt => opt.MapFrom(src => src.PayrollRun.Name))
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => $"{src.Employee.FirstName} {src.Employee.LastName}"))
                .ForMember(dest => dest.EmployeeEmail, opt => opt.MapFrom(src => src.Employee.WorkEmail))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Employee.Department.Name))
                .ForMember(dest => dest.JobTitleName, opt => opt.MapFrom(src => src.Employee.JobTitle.Name))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.Currency.Code))
                .ForMember(dest => dest.IsLocked, opt => opt.MapFrom(src => src.PayrollRun.IsLocked));

            CreateMap<PayrollEntryVM, PayrollEntry>()
                .ForMember(dest => dest.PayrollRun, opt => opt.Ignore())
                .ForMember(dest => dest.Employee, opt => opt.Ignore())
                .ForMember(dest => dest.Currency, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());
        }
    }
}
