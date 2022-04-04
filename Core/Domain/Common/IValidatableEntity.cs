using System;

namespace InvestTeam.AutoBox.Domain.Common
{
    public class EntityValidationResult
    {
        public EntityValidationResult()
        {
            IsValid = true;
            Reason = String.Empty;
        }

        public EntityValidationResult(bool isValid, string reason)
        {
            IsValid = isValid;
            Reason = reason;
        }

        public bool IsValid { get; }

        public string Reason { get; }
    }

    public interface IValidatableEntity
    {
        EntityValidationResult Validate();
    }
}
