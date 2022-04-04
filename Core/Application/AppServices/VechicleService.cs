namespace InvestTeam.AutoBox.Application.AppServices
{
    using AutoMapper;
    using InvestTeam.AutoBox.Application.Common;
    using InvestTeam.AutoBox.Application.Common.Interfaces;
    using InvestTeam.AutoBox.Domain.Entities;
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// 
    /// </summary>
    public class VechicleService : InfrastructureService<Vechicle>
    {
        private readonly IMapper mapper;

        public VechicleService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            this.mapper = mapper;
        }

        public virtual QueryOperationResult<Vechicle> GetVechicles(Expression<Func<Vechicle, bool>> filter)
        {
            return base.GetComponents(filter);
        }
    }
}
