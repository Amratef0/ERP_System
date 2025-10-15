using ERP_System_Project.Models.HR;
using ERP_System_Project.ViewModels.HR;

namespace ERP_System_Project.Services.Interfaces.HR
{
    public interface IEmployeeService : IGenericService<Employee>
    {
        Task<bool> CreateAsync(EmployeeVM model);

        Task<bool> UpdateAsync(EmployeeVM employee);

        Task<bool> IsExistAsync(int id);

        Task<Employee> GetByIdWithDetailsAsync(int id);

        Task<IEnumerable<EmployeeIndexVM>> GetAllAsync();

        Task<IEnumerable<EmployeeIndexVM>> SearchAsync(string? name, int? branchId, int? departmentId, int? employeeTypeId, int? jobTitleId);

        /// <summary>
        /// Gets employee by ApplicationUserId
        /// </summary>
        Task<Employee?> GetByApplicationUserIdAsync(string applicationUserId);
    }
}
