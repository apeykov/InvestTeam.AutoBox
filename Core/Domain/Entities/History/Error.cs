namespace InvestTeam.AutoBox.Domain.Entities
{
    using InvestTeam.AutoBox.Domain.Common;

    /// <summary>
    /// Error item which is attached to the history
    /// </summary>
    public class Error : BaseEntity
    {
        /// <summary>
        ///  Data store ID of a history item
        /// </summary>
        public int HistoryID { get; set; }

        /// <summary>
        /// Type of the error
        /// </summary>
        public string ErrorType { get; set; }

        /// <summary>
        /// Error Text
        /// </summary>
        public string ErrorText { get; set; }
    }
}
