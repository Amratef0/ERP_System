using ERP_System_Project.Models.Logs;
using ERP_System_Project.Services.Interfaces.Log;
using Microsoft.AspNetCore.Mvc;

namespace ERP_System_Project.Controllers.Log
{
    public class PerformanceLogController : Controller
    {
        private readonly IPerformanceLogService _performanceLogService;

        public PerformanceLogController(IPerformanceLogService performanceLogService)
        {
            _performanceLogService = performanceLogService;
        }
        public async Task<IActionResult> Index()
            => View(await _performanceLogService.GetAllLogsAsync());
    }
}
