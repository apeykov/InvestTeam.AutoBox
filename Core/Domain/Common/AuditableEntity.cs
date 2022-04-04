using System;

namespace InvestTeam.AutoBox.Domain.Common
{
    public abstract class AuditableEntity : BaseEntity
    {
        /// <summary>
        /// Who created the object
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Time stamp of object creation
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Who has changed the object last
        /// </summary>
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// Time stamp of last changes
        /// </summary>
        public DateTime? LastModified { get; set; }
    }
}
