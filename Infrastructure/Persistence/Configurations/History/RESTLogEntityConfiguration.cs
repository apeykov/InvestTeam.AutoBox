namespace InvestTeam.AutoBox.Infrastructure.Persistence.Configurations
{
    using InvestTeam.AutoBox.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class RESTLogEntityConfiguration : IEntityTypeConfiguration<RESTLog>
    {
        public void Configure(EntityTypeBuilder<RESTLog> builder)
        {
            builder.HasOne(rl => rl.History)
                .WithMany(h => h.RESTLogs)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
