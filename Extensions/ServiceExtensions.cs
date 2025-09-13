using ERP_System_Project.Repository.Implementations;
using ERP_System_Project.Repository.Interfaces;
using ERP_System_Project.UOF;

namespace ERP_System_Project.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDataSevices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddBuisnessSevices(this IServiceCollection services)
        {
            throw new NotImplementedException();
        }
    }
}
