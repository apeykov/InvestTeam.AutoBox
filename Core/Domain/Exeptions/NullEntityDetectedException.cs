using System;

namespace InvestTeam.AutoBox.Domain.Exceptions
{
    public class NullEntityDetectedException : ApplicationException
    {
        public NullEntityDetectedException(Type entityType)
        {
            EntityType = entityType;
        }

        Type EntityType { get; }

        public override string Message => $"Null entity detected. { EntityType }";
    }
}
