using InvestTeam.AutoBox.Application.Common.Interfaces;
using InvestTeam.AutoBox.Domain.Enums;

namespace InvestTeam.AutoBox.SelfService.Web.API
{
    /// <summary>
    /// `Command source` concept is used for DB logging purposes.
    /// </summary>
    public class WebAPICommandSource : ISource
    {
        public Source Source => Source.RESTAPI;
    }
}
