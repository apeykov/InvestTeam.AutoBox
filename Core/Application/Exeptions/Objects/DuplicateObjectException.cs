namespace InvestTeam.AutoBox.Application.Exceptions
{
    using System;

    public class DuplicateObjectException : ObjectException
    {
        protected const string defaultMessage = "Object with the same identity already exists!";
        protected string collisionObjectIdentity;

        public DuplicateObjectException(Type objectType, string collisionObjectIdentity, Exception innerException = null) :
            base(defaultMessage, objectType, collisionObjectIdentity, innerException)
        {
            this.collisionObjectIdentity = collisionObjectIdentity;
        }

        public DuplicateObjectException(string message, Type objectType, string objectIdentity, Exception innerException = null) :
           base(message, objectType, objectIdentity, innerException)
        {
        }

        public override string ToString()
        {
            return $"{ defaultMessage }: [ { GetType() }, Collision Identity: { collisionObjectIdentity } ]";
        }
    }
}
