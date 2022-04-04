namespace InvestTeam.AutoBox.Domain.Entities
{
    using InvestTeam.AutoBox.Domain.Common;
    using InvestTeam.AutoBox.Domain.Enums;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class to track object history and changes
    /// </summary>
    public class History : BaseEntity
    {
        public History()
        {
            Errors = new HashSet<Error>();
        }

        /// <summary>
        /// The commandlet which was executed
        /// </summary>
        public string Commandlet { get; set; }

        /// <summary>
        /// The parameters used for the commandlet
        /// </summary>
        public string Parameters { get; set; }

        /// <summary>
        /// the user who executed the command
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// When has the command been executed
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// What was the result of the commandlet
        /// </summary>
        public ServiceResult Result { get; set; }

        /// <summary>
        /// Which object type was accessed
        /// </summary>
        public ObjectType ObjectType { get; set; }

        /// <summary>
        /// Database generated ID of the object
        /// </summary>
        public int ObjectID { get; set; }

        /// <summary>
        /// Domain object identity
        /// </summary>
        public string ObjectIdentity { get; set; }

        /// <summary>
        /// Source of the commandlet
        /// </summary>
        public Source Source { get; set; }

        /// <summary>
        /// Identifier for group of objects that have been modified by the same (one) commandlet execution
        /// </summary>
        public Guid RunID { get; set; }

        public virtual ICollection<Error> Errors { get; set; }

        public virtual ICollection<HistoryDetails> HistoryDetails { get; set; }

        public virtual ICollection<RESTLog> RESTLogs { get; set; }
    }
}
