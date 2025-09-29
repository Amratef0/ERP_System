using ERP_System_Project.Models.HR;
using ERP_System_Project.ViewModels.HR;
using System.Linq.Expressions;

namespace ERP_System_Project.Services.Interfaces.HR
{
    public interface IWorkScheduleService : IGenericService<WorkSchedule>
    {
        Task<ICollection<WorkScheduleDay>?> GetScheduleDaysByIdAsync(int workScheduleId);
        Task<WorkScheduleDay?> GetScheduleDayByIdAsync(int workScheduleId, int dayId);
        Task<bool> SoftDelete(int id);
    }
}
