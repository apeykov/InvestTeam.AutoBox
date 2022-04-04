namespace InvestTeam.AutoBox.Application.Common
{
    using System;

    public class IdentityValidationResult<TEntity>
    {
        private TEntity entity;

        public IdentityValidationResult(TEntity entity, Exception exception = null)
        {
            this.entity = entity;
            ValidationException = exception;
        }

        public bool IsValid
        {
            get => ValidationException == null;
        }

        public Exception ValidationException { get; }

        public Type ValidationExceptionType
        {
            get
            {
                return ValidationException?.GetType();
            }
        }

        public TEntity Entity
        {
            get => entity;
        }
    }
}
