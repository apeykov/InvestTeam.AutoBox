namespace InvestTeam.AutoBox.Application.Exceptions
{
    using System;

    public class NonExistentObjectException : ObjectException
    {
        public NonExistentObjectException(Type objectType, string objectIdentity, Exception innerException = null) :
            //base(defaultMessage, objectType, objectIdentity, innerException)
            base($"Object with identity { objectIdentity } does NOT exists!", objectType, objectIdentity, innerException)
        {
        }

        public NonExistentObjectException(string message, Type objectType, string objectIdentity, Exception innerException = null) :
           base(message, objectType, objectIdentity, innerException)
        {
        }
    }
}
