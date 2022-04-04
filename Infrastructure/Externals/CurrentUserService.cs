namespace InvestTeam.AutoBox.Infrastructure.Externals
{
    using InvestTeam.AutoBox.Application.Common.Interfaces;
    using System;

    public class CurrentUserService : ICurrentUserService
    {
        public string UserId => Environment.UserName;
    }
}
