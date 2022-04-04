namespace InvestTeam.AutoBox.Infrastructure.Persistence.Configurations
{
    using InvestTeam.AutoBox.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class VechicleConfiguration : IdentityConfiguration<Vechicle>, IEntityTypeConfiguration<Vechicle>
    {
        public override void Configure(EntityTypeBuilder<Vechicle> builder)
        {
            base.Configure(builder);

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Model)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
