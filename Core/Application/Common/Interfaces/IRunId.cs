namespace InvestTeam.AutoBox.Application.Common.Interfaces
{
    using System;

    public interface IRunId
    {
        Guid RunId { get; }

        void UpdateRunId();
    }
}
