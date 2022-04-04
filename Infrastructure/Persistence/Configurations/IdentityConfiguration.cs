namespace InvestTeam.AutoBox.Infrastructure.Persistence.Configurations
{
    using InvestTeam.AutoBox.Domain.Entities.Common;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public abstract class IdentityConfiguration<TEntity> where TEntity : class, IIdentifiableEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(e => e.Identity)
                .IsRequired()
                .HasMaxLength(256);

            builder.HasIndex(e => e.Identity)
              .IsUnique();
        }
    }
}
