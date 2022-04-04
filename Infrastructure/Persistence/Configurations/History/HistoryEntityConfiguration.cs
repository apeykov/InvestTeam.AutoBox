namespace InvestTeam.AutoBox.Infrastructure.Persistence.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class HistoryEntityConfiguration : IEntityTypeConfiguration<InvestTeam.AutoBox.Domain.Entities.History>
    {
        public void Configure(EntityTypeBuilder<InvestTeam.AutoBox.Domain.Entities.History> builder)
        {
            builder.Property(history => history.Commandlet)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(history => history.Parameters)
               .IsRequired()
               .HasMaxLength(1024);

            builder.Property(history => history.ObjectIdentity)
               .HasMaxLength(256);

            builder.Property(history => history.User)
               .IsRequired()
               .HasMaxLength(256);
        }
    }
}
