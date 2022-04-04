namespace InvestTeam.AutoBox.Application
{
    using InvestTeam.AutoBox.Application.AppServices;
    using InvestTeam.AutoBox.Application.Common;
    using InvestTeam.AutoBox.Application.Common.Interfaces;
    using InvestTeam.AutoBox.Application.Data;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services)
        {
            services.AddScoped(typeof(IDataRepository<>), typeof(DataRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<LoggingService>();
            services.AddScoped<IRunId, RunIdService>();
            services.AddScoped<AppSettingsStore>();

            services.AddScoped<OrderService>();
            services.AddScoped<VechicleService>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
