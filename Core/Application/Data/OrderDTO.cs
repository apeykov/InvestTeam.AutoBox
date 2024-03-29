﻿namespace InvestTeam.AutoBox.Application.Data.DTOs.PhoneEntities
{
    using AutoMapper;
    using InvestTeam.AutoBox.Application.Common.Mappings;
    using InvestTeam.AutoBox.Domain.Attributes;
    using InvestTeam.AutoBox.Domain.Entities;
    using System.ComponentModel.DataAnnotations;

    public class OrderDTO : IMapFrom<Order>
    {
        [Filter]
        public string Identity { get; set; }

        [Filter]
        public string Description { get; set; }

        [Filter]
        [Required]
        public VechicleDTO Vechicle { get; set; }

        public string UserId { get; set; }

        public void TransferUpdatedData(Order order)
        {
            if (!string.IsNullOrEmpty(Description))
            {
                order.Description = Description;
            }

            if (Vechicle != null)
            {
                Vechicle.TransferUpdatedData(order.Vechicle);
            }
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<OrderDTO, Order>();
        }
    }
}
