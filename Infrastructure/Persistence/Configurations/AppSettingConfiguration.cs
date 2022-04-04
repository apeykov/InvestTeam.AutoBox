namespace InvestTeam.AutoBox.Infrastructure.Persistence.Configurations
{
    using InvestTeam.AutoBox.Domain.Common;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AppSettingConfiguration : IEntityTypeConfiguration<AppSetting>
    {
        public virtual void Configure(EntityTypeBuilder<AppSetting> builder)
        {
            builder.Ignore(e => e.Id);

            builder.Property(e => e.Key).HasMaxLength(256);
            builder.Property(e => e.Value).HasMaxLength(1024);

            builder.HasKey("Key", "Value");

        }
    }
}
