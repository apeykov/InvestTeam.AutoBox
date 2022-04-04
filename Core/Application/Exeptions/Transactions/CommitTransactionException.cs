namespace InvestTeam.AutoBox.Application.Exceptions
{
    using System;

    public class CommitTransactionException : ApplicationException
    {
        private const string message = "Attempt to commit not existing transaction";

        public CommitTransactionException() : base(message)
        {
        }

        public CommitTransactionException(Exception innerException) : base(message, innerException)
        {
        }
    }
}