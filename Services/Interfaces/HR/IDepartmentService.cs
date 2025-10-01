using ERP_System_Project.Models.HR;

namespace ERP_System_Project.Services.Interfaces.HR
{
    public interface IDepartmentService : IGenericService<Department>
    {
        Task<IEnumerable<Department>> SearchAsync(string name);
    }
}
