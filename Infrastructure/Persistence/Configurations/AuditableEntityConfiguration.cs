namespace InvestTeam.AutoBox.Infrastructure.Persistence.Configurations
{
    using InvestTeam.AutoBox.Domain.Common;
    using InvestTeam.AutoBox.Domain.Entities.Common;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public abstract class AuditableEntityConfiguration<TEntity> : IdentityConfiguration<TEntity>
        where TEntity : AuditableEntity, IIdentifiableEntity
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(e => e.Created)
               .IsRequired();

            builder.Property(e => e.LastModifiedBy)
                .HasMaxLength(256);

            base.Configure(builder);
        }
    }
}
