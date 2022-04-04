using InvestTeam.AutoBox.Application.Common.Interfaces;
using System;

namespace InvestTeam.AutoBox.Infrastructure.Externals
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
