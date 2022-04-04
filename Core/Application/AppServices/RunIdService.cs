namespace InvestTeam.AutoBox.Application.AppServices
{
    using InvestTeam.AutoBox.Application.Common.Interfaces;
    using System;

    internal class RunIdService : IRunId
    {
        private Guid runId;

        public RunIdService()
        {
            runId = Guid.NewGuid();
        }

        public Guid RunId
        {
            get
            {
                return runId;
            }
        }

        public void UpdateRunId()
        {
            runId = Guid.NewGuid();
        }
    }
}