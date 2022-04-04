namespace InvestTeam.AutoBox.Domain.Exceptions
{
    using InvestTeam.AutoBox.Domain.Common;
    using System;

    public class EntityNotFoundException : ApplicationException
    {
        private readonly BaseEntity entity;

        public EntityNotFoundException(BaseEntity entity)
        {
            this.entity = entity;
        }

        public override string Message => "Entity not found: " + entity.ToString();
    }
}
