using ERP_System_Project.Models.Logs;

namespace ERP_System_Project.Services.Interfaces.Log
{
    public interface IPerformanceLogService
    {
        Task<List<PerformanceLog>> GetAllLogsAsync();
    }
}
