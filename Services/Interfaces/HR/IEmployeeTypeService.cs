using ERP_System_Project.Models.HR;

namespace ERP_System_Project.Services.Interfaces.HR
{
    public interface IEmployeeTypeService : IGenericService<EmployeeType>
    {
        Task<IEnumerable<EmployeeType>> SearchAsync(string name);
    }
}
