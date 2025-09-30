using ERP_System_Project.Models.HR;

namespace ERP_System_Project.Services.Interfaces.HR
{
    public interface ILeaveTypeService : IGenericService<LeaveType>
    {
        Task<IEnumerable<LeaveType>> SearchAsync(string name);
    }
}
