using System;

namespace InvestTeam.AutoBox.Application.Common
{
    public class LogResult : CommonResult
    {
        public void AddException(Exception ex)
        {
            exceptions.Add(ex);
        }
    }
}
