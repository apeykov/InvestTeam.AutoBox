using InvestTeam.AutoBox.Domain.Enums;
using System;

namespace InvestTeam.AutoBox.Domain.Common
{
    public abstract class BaseEntity
    {
        public virtual int Id { get; protected set; }

        public override string ToString()
        {
            return base.ToString() + " -> Id: " + Id.ToString();
        }

        public ObjectType GetObjectType()
        {
            string fullTypeName = GetType().ToString();
            var lastIndexOfDot = fullTypeName.LastIndexOf(".");
            string typeName = fullTypeName.Substring(lastIndexOfDot + 1);

            object result;

            bool converted = Enum.TryParse(typeof(ObjectType), typeName, out result);

            if (!converted)
            {
                throw new
                    InvalidCastException($"Object type `{typeName}` is not convertable to {typeof(ObjectType)}");
            }

            return (ObjectType)result;
        }
    }
}