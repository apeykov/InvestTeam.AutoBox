namespace InvestTeam.AutoBox.Infrastructure.Persistence.Configurations
{
    using InvestTeam.AutoBox.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ErrorEntityConfiguration : IEntityTypeConfiguration<Error>
    {
        public void Configure(EntityTypeBuilder<Error> builder)
        {
            builder.Property(err => err.ErrorType)
                .IsRequired()
                .HasMaxLength(1024);
        }
    }
}
