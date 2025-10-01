using ERP_System_Project.Models.HR;

namespace ERP_System_Project.Services.Interfaces.HR
{
    public interface IEmployeeTypeCodeService : IGenericService<AttendanceStatusCode>
    {
        Task<IEnumerable<AttendanceStatusCode>> SearchAsync(string name);
    }
}
