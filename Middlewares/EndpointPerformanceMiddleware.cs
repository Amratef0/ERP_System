using ERP_System_Project.Models.Enums;
using ERP_System_Project.Models.Logs;
using ERP_System_Project.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ERP_System_Project.Middlewares
{
    public class EndpointPerformanceMiddleware
    {
        private readonly RequestDelegate _next;

        public EndpointPerformanceMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context)
        {
            var stopWatch = new Stopwatch();

            stopWatch.Start();

            await _next(context);

            stopWatch.Stop();

            var path = context.Request.Path.Value ?? "";
            var segments = path.Split("/", StringSplitOptions.RemoveEmptyEntries);

            var endpointName = segments.Length >= 2
                ? $"/{segments[0]}/{segments[1]}"  // "/product/edit"
                : path;                            

            var executionTime = stopWatch.ElapsedMilliseconds;
            var executionTimeStatus = executionTime switch
            {
                < 1 => TimeActionStatus.Instant,
                >= 1 and < 100 => TimeActionStatus.Fast,
                >= 100 and < 500 => TimeActionStatus.Moderate,
                >= 500 and < 1000 => TimeActionStatus.Slow,
                >= 1000 => TimeActionStatus.VerySlow,
            };
            var log = new PerformanceLog
            {
                EndPointName = endpointName,
                ElabsedTime = executionTime.ToString(),
                Status = executionTimeStatus
            };

            // Resolve scoped service inside Invoke()
            var _repository = context.RequestServices.GetRequiredService<IRepository<PerformanceLog>>();

            if(await _repository.AnyAsync(l => l.EndPointName.Contains(endpointName)))
            {
                var logRow = await _repository
                    .GetAllAsIQueryable()
                    .FirstOrDefaultAsync(l => l.EndPointName.Contains(endpointName));

                logRow.RequestDate = DateTime.Now;
                logRow.Status = log.Status;
                logRow.ElabsedTime = log.ElabsedTime;
            }
            else
            {
                await _repository.AddAsync(log);
            }
            await _repository.SaveAsync();
        }
    }
}
