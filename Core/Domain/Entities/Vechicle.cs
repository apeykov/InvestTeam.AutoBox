using InvestTeam.AutoBox.Domain.Common;
using InvestTeam.AutoBox.Domain.Entities.Common;
using InvestTeam.AutoBox.Domain.Enums;
using System;
using System.Collections.Generic;

namespace InvestTeam.AutoBox.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Vechicle : BaseEntity, IIdentifiableEntity, ICloneable
    {
        public Vechicle()
        {
            Orders = new HashSet<Order>();
        }

        /// <summary>
        /// Identity or reg.number of the vechicle. Should be unique
        /// </summary>
        public string Identity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<Order> Orders { get; set; }

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
