namespace InvestTeam.AutoBox.Domain.Entities
{
    using InvestTeam.AutoBox.Domain.Common;
    using InvestTeam.AutoBox.Domain.Enums;

    /// <summary>
    /// Rest log information attached to history items. Needs to be created for each request from the rest api
    /// </summary>
    public class RESTLog : BaseEntity
    {
        /// <summary>
        /// Response string of the request (JSON)
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// Request information
        /// </summary>
        public string Request { get; set; }

        /// <summary>
        /// Which kind of http request was executed
        /// </summary>
        public HttpRequestType Type { get; set; }

        /// <summary>
        /// Id of the related history item
        /// </summary>
        public int HistoryID { get; set; }

        public virtual History History { get; set; }
    }
}
