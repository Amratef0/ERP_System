using ERP_System_Project.Repository.Implementation;
using ERP_System_Project.Repository.Interfaces;
using ERP_System_Project.Services.Implementation;
using ERP_System_Project.Services.Interfaces;
using ERP_System_Project.Settings;
using ERP_System_Project.UOW;

namespace ERP_System_Project.Extensions
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddDataSevices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            services.AddTransient<IEmailService,EmailService>();

            return services;
        }
    }
}
