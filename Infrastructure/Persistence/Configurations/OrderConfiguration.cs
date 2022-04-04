namespace InvestTeam.AutoBox.Infrastructure.Persistence.Configurations
{
    using InvestTeam.AutoBox.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class OrderConfiguration : AuditableEntityConfiguration<Order>, IEntityTypeConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Description)
                .IsRequired();

            builder.Property(o => o.UserId)
                .IsRequired();

            builder.HasOne(o => o.Vechicle)
                .WithMany(v => v.Orders)
                .HasForeignKey(o => o.VechicleId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

        }
    }
}
