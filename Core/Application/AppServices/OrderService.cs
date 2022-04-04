namespace InvestTeam.AutoBox.Application.AppServices
{
    using AutoMapper;
    using InvestTeam.AutoBox.Application.Common;
    using InvestTeam.AutoBox.Application.Common.Interfaces;
    using InvestTeam.AutoBox.Application.Data.DTOs.PhoneEntities;
    using InvestTeam.AutoBox.Domain.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class OrderService : InfrastructureService<Order>
    {
        private readonly IMapper mapper;
        private readonly IDataRepository<Order> orders;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            this.mapper = mapper;
            orders = unitOfWork.Repository<Order>();
        }

        public virtual async Task<OperationResult<Order>> AddOrder(OrderDTO orderInputData)
        {
            Order order = mapper.Map<Order>(orderInputData);

            unitOfWork.BeginTransaction();

            order.Initialize(() => GetNextSequenceNumber());

            Vechicle vechicle = unitOfWork.Repository<Vechicle>()
               .GetOne(v => v.Identity == orderInputData.Vechicle.Number);

            if (vechicle == null)
            {
                vechicle = mapper.Map<Vechicle>(orderInputData.Vechicle);
            }

            order.Vechicle = vechicle;

            var addToLocalStoreResult = await base.AddComponent(order);

            unitOfWork.CommitTransaction();

            return addToLocalStoreResult;
        }

        /// <summary>
        /// 
        /// </summary>       
        public virtual async Task<OperationResult<Order>> SetOrder(OrderDTO orderInputData)
        {
            var result = new OperationResult<Order>();

            IDataRepository<Order> orders = unitOfWork.Repository<Order>();

            Order order = orders.FindBy(u => u.Identity == orderInputData.Identity).FirstOrDefault<Order>();

            if (order == null)
            {
                var appEx = new ApplicationException($"Order with Identity: `{ orderInputData.Identity }` not exist.");

                result.AddException(appEx, null);

                return result;
            }

            orderInputData.TransferUpdatedData(order);

            return await base.SetComponent(order);
        }

        public virtual async Task<OperationResult<Order>> RemoveOrder(string identity)
        {
            return await base.RemoveComponent(identity);
        }

        public virtual QueryOperationResult<Order> GetOrders(Expression<Func<Order, bool>> filter)
        {
            return base.GetComponents(filter, o => o.Vechicle);
        }

        private long GetNextSequenceNumber()
        {
            long nextSequenceNumber = Order.OrderSequenceNumberRangeStart;

            List<Order> order = orders
                .FindBy(c => c.SequenceNumber >= Order.OrderSequenceNumberRangeStart
                    && c.SequenceNumber <= Order.OrderSequenceNumberRangeEnd)
                .ToList();

            if (order.Count() == 0)
            {
                return nextSequenceNumber;
            }
            else
            {
                nextSequenceNumber = order.Max(c => c.SequenceNumber) + 1;
            }

            if (nextSequenceNumber > Order.OrderSequenceNumberRangeEnd)
            {
                throw new InvalidOperationException("Could NOT assign SequenceNumber outside of the range " +
                    $"[{ Order.OrderSequenceNumberRangeStart }; { Order.OrderSequenceNumberRangeEnd }]");
            }

            return nextSequenceNumber;
        }

    }

}
