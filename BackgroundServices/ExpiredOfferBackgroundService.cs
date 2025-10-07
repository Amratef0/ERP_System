using ERP_System_Project.Models.ECommerce;
using ERP_System_Project.Models.Logs;
using ERP_System_Project.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERP_System_Project.BackgroundServices
{
    public class ExpiredOfferBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ExpiredOfferBackgroundService> _logger;

        public ExpiredOfferBackgroundService(IServiceProvider serviceProvider, ILogger<ExpiredOfferBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;
                var nextRun = now.Date.AddDays(1); 
                var delay = nextRun - now;

                try
                {
                    // Wait until 12 AM UTC
                    await Task.Delay(delay, stoppingToken);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var _repository = scope.ServiceProvider.GetRequiredService<IRepository<Offer>>();

                        await _repository
                            .GetAllAsIQueryable()
                            .Where(o => o.EndDate <= DateTime.Now)
                            .ExecuteDeleteAsync(stoppingToken);
                    }
                }
                catch (TaskCanceledException)
                {
                    _logger.LogInformation("Expired Offer BackgroundService stopping gracefully.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while Checking offers expired date");
                }
            }
        }
    }
}
