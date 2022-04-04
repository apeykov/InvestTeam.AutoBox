using InvestTeam.AutoBox.Domain.Common;
using InvestTeam.AutoBox.Domain.Entities.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace InvestTeam.AutoBox.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Order : AuditableEntity, IIdentifiableEntity, ICloneable
    {
        public const uint OrderSequenceNumberRangeStart = 0000001;
        public const uint OrderSequenceNumberRangeEnd = 9999999;
        private const string orderPrefix = "ITAB_ORDER_";

        public Order()
        {
        }

        /// <summary>
        /// Identity of the order with format "ITAB_ORDER_0000001". Should be unique
        /// </summary>
        public string Identity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(300, MinimumLength = 5)]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int VechicleId { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Vechicle Vechicle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(450)]
        public string UserId { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public long SequenceNumber { get; set; }

        public void Initialize(Func<long> sequenceNumberInitializer)
        {
            SequenceNumber = sequenceNumberInitializer();
            Identity = $"{ orderPrefix }{ SequenceNumber.ToString("D7") }";
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return Identity;
        }
    }
}
