namespace InvestTeam.AutoBox.Application.Exceptions
{
    using System;

    public class RollbackTransactionException : ApplicationException
    {
        private const string message = "Attempt to rollback not existing transaction";

        public RollbackTransactionException() : base(message)
        {
        }

        public RollbackTransactionException(Exception innerException) : base(message, innerException)
        {
        }
    }
}