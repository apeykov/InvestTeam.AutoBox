namespace InvestTeam.AutoBox.Application.Exceptions
{
    using System;

    public abstract class ObjectException : Exception
    {
        public Type ObjectType { get; protected set; }

        public string ObjectIdentity { get; protected set; }

        protected ObjectException(string message, Type objectType, string objectIdentity, Exception innerException) :
           base(message, innerException)
        {
            ObjectType = objectType;
            ObjectIdentity = objectIdentity;
        }

        public override string ToString()
        {
            return $"{ base.Message } [Object Type: { ObjectType }] [Object Identity:  {ObjectIdentity}]";
        }
    }
}
