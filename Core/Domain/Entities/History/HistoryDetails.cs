namespace InvestTeam.AutoBox.Domain.Entities
{
    using InvestTeam.AutoBox.Domain.Common;

    /// <summary>
    /// Additional details for all (successful / failed) history (log) items
    /// </summary>
    public class HistoryDetails : BaseEntity
    {
        public string Information { get; set; }

        public string Warning { get; set; }

        public int HistoryId { get; set; }

        public virtual History History { get; set; }
    }
}
