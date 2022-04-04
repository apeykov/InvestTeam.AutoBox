namespace InvestTeam.AutoBox.Infrastructure.Persistence
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    /// <summary>
    /// Design time factory.
    /// Used by the Microsoft.EntityFrameworkCore.Design package and 
    /// `dotnet ef migrations add` tool -> CLI command for EF Core Migrations management
    /// https://docs.microsoft.com/en-gb/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli
    /// </summary>
    internal class ApplicationDbContextDesignTimeFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            if (args.Length > 0)
            {
                string connectionString = args[0];

                optionsBuilder.UseSqlServer(connectionString, builder =>
                    builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            }
            else
            {
                optionsBuilder.UseSqlServer(builder =>
                    builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            }

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
