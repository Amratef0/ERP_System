using ERP_System_Project.Models.Logs;
using ERP_System_Project.Services.Interfaces.Log;
using ERP_System_Project.UOW;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.Services.Implementation.Log
{
    public class PerformanceLogService : IPerformanceLogService
    {
        private readonly IUnitOfWork _uow;

        public PerformanceLogService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<List<PerformanceLog>> GetAllLogsAsync()
            => await _uow.PerformanceLogs
            .GetAllAsIQueryable()
            .OrderByDescending(pl => pl.ElabsedTime)
            .ToListAsync();
    }
}
