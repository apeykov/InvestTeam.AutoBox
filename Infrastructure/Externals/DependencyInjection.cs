namespace InvestTeam.AutoBox.Infrastructure.Externals
{
    using InvestTeam.AutoBox.Application.Common.Interfaces;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddExternalsDI(this IServiceCollection services /*, IConfiguration configuration*/)
        {
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<ICurrentUserService, CurrentUserService>();

            services.AddSingleton<IApplicationConfiguration, AutoBoxConfiguration>();

            return services;
        }
    }
}
