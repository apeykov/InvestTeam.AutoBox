namespace InvestTeam.AutoBox.Infrastructure.Persistence
{
    using InvestTeam.AutoBox.Application.Common.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        /// <summary>
        /// Confure the IoC container
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dbConnectionString">Temporary parameter which will be dropped when ASP.NET Web API middleware implemented</param>
        /// <returns></returns>
        public static IServiceCollection AddPersistenceDI(this IServiceCollection services, string dbConnectionString /*, IConfiguration configuration*/)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    dbConnectionString,
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)),
                ServiceLifetime.Scoped);

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            return services;
        }
    }
}
