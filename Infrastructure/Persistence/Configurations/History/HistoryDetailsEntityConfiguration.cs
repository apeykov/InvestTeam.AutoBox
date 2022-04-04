namespace InvestTeam.AutoBox.Infrastructure.Persistence.Configurations.History
{
    using InvestTeam.AutoBox.Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class HistoryDetailsEntityConfiguration : IEntityTypeConfiguration<HistoryDetails>
    {
        public void Configure(EntityTypeBuilder<HistoryDetails> builder)
        {
            builder.HasOne(hd => hd.History)
                .WithMany(h => h.HistoryDetails)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
