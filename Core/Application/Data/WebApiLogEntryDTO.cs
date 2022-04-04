namespace InvestTeam.AutoBox.Application.Data.DTOs
{
    using InvestTeam.AutoBox.Domain.Enums;

    public class WebApiLogEntryDTO
    {
        public string Endpoint { get; set; }

        public string Request { get; set; }

        public string RequestParams { get; set; }

        public string Response { get; set; }

        public HttpRequestType HttpVerb { get; set; }
    }
}
