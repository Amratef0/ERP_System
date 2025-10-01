using ERP_System_Project.Models.HR;

namespace ERP_System_Project.Services.Interfaces.HR
{
    public interface IJobTitleService : IGenericService<JobTitle>
    {
        Task<IEnumerable<JobTitle>> SearchAsync(string name);
    }
}
