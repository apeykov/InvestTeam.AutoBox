using System;
using System.Text;

namespace InvestTeam.AutoBox.Domain.Exceptions
{
    public class EntityRemovalException : ApplicationException
    {
        public EntityRemovalException(Type entityType, string entityIdentity, string reason) : base(reason)
        {
            EntityType = entityType;
            EntityIdentity = entityIdentity;
        }

        public Type EntityType { get; }

        public string EntityIdentity { get; }

        public override string Message
        {
            get
            {
                var errMessaage = new StringBuilder($"Entity { EntityIdentity } of type { EntityType } is not eligible for removal.");

                if (base.Message != null)
                {
                    errMessaage.AppendLine(base.Message);
                }

                return errMessaage.ToString();
            }
        }

        public override string ToString()
        {
            return Message;
        }
    }
}
