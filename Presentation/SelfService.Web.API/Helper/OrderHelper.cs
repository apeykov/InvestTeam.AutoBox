using InvestTeam.AutoBox.Application.AppServices;
using InvestTeam.AutoBox.Application.Common;
using InvestTeam.AutoBox.Application.Data.DTOs.PhoneEntities;
using InvestTeam.AutoBox.Domain.Entities;
using InvestTeam.AutoBox.Domain.Enums;
using InvestTeam.AutoBox.SelfService.Web.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InvestTeam.AutoBox.SelfService.Web.API.Helper
{
    public class OrderHelper
    {
        private OrderService service;
        private UserManager<User> userMng;
        private readonly IHttpContextAccessor context;

        public OrderHelper(OrderService service, UserManager<User> userMng, IHttpContextAccessor httpContextAccessor)
        {
            this.service = service;
            this.userMng = userMng;
            this.context = httpContextAccessor;
        }

        public async Task<OperationResult<Order>> CreateOrder(OrderDTO orderDTO)
        {
            OperationResult<Order> result = new OperationResult<Order>();

            orderDTO.UserId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                result = await service.AddOrder(orderDTO);

                foreach (Order order in result.Entities)
                    order.Vechicle.Orders = null;
            }
            catch (Exception ex)
            {
                var order = new Order
                {
                    Description = orderDTO.Description,
                    Vechicle = new Vechicle
                    {
                        Identity = orderDTO.Vechicle.Number,
                        Model = orderDTO.Vechicle.Model,
                        Color = (Color)orderDTO.Vechicle.Color
                    }
                };
                result.AddException(ex, order);
            }
            return result;
        }

        public async Task<OperationResult<Order>> UpdateOrder(OrderDTO orderDTO)
        {
            OperationResult<Order> result = new OperationResult<Order>();

            try
            {
                result = await service.SetOrder(orderDTO);

                foreach (Order order in result.Entities)
                    order.Vechicle.Orders = null;
            }
            catch (Exception ex)
            {
                var order = new Order
                {
                    Description = orderDTO.Description,
                    Vechicle = new Vechicle
                    {
                        Identity = orderDTO.Vechicle.Number,
                        Model = orderDTO.Vechicle.Model,
                        Color = (Color)orderDTO.Vechicle.Color
                    }
                };
                result.AddException(ex, order);
            }
            return result;
        }

        public async Task<OperationResult<Order>> RemoveOrder(string identity)
        {
            OperationResult<Order> result = new OperationResult<Order>();

            try
            {
                result = await service.RemoveOrder(identity);
            }
            catch (Exception ex)
            {
                var order = new Order { Identity = identity };
                result.AddException(ex, order);
            }
            return result;
        }

        public async Task<QueryOperationResult<Order>> SearchOrders(string search)
        {
            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = await userMng.FindByIdAsync(userId);
            bool isManager = await userMng.IsInRoleAsync(user, Enum.GetName(typeof(Role), Role.Manager));

            QueryOperationResult<Order> result = new QueryOperationResult<Order>();

            if (string.IsNullOrEmpty(search))
                if (isManager)
                    result = service.GetOrders(o => true);
                else
                    result = service.GetOrders(o => o.UserId == userId);
            else
                if (isManager)
                result = service.GetOrders(o => o.Identity.Contains(search) || o.Description.Contains(search) || o.Vechicle.Identity.Contains(search) || o.Vechicle.Model.Contains(search));
            else
                result = service.GetOrders(o => o.Identity.Contains(search) || o.Description.Contains(search) || o.Vechicle.Identity.Contains(search) || o.Vechicle.Model.Contains(search) && o.UserId == userId);

            foreach (Order order in result.Entities)
                order.Vechicle.Orders = null;

            return await Task.FromResult(result);
        }

        public async Task<QueryOperationResult<Order>> GetOrders(string identity)
        {
            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = await userMng.FindByIdAsync(userId);
            bool isManager = await userMng.IsInRoleAsync(user, Enum.GetName(typeof(Role), Role.Manager));

            QueryOperationResult<Order> result = new QueryOperationResult<Order>();

            if (string.IsNullOrEmpty(identity))
                if (isManager)
                    result = service.GetOrders(o => true);
                else
                    result = service.GetOrders(o => o.UserId == userId);
            else
                if (isManager)
                result = service.GetOrders(o => o.Identity == identity);
            else
                result = service.GetOrders(o => o.Identity == identity && o.UserId == userId);

            foreach (Order order in result.Entities)
                order.Vechicle.Orders = null;

            return await Task.FromResult(result);
        }
    }
}
