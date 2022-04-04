namespace InvestTeam.AutoBox.Application.Common.Interfaces
{
    using Microsoft.Extensions.Configuration;

    public interface IApplicationConfiguration
    {
        IConfigurationRoot Configuration { get; }

        string DbConnectionString { get; }

        string DbConnectionStringPasswordExcluded { get; }

    }
}
