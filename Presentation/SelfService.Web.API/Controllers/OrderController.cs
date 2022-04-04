namespace InvestTeam.AutoBox.SelfService.Web.API.Controllers
{
    using InvestTeam.AutoBox.Application.AppServices;
    using InvestTeam.AutoBox.Application.Common;
    using InvestTeam.AutoBox.Application.Data.DTOs;
    using InvestTeam.AutoBox.Application.Data.DTOs.PhoneEntities;
    using InvestTeam.AutoBox.Domain.Entities;
    using InvestTeam.AutoBox.Domain.Enums;
    using InvestTeam.AutoBox.SelfService.Web.API.Helper;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [RequireHttps]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderController : ControllerBase
    {
        private readonly OrderHelper orderHelper;
        private readonly LoggingService logger;
        public OrderController(OrderHelper orderHelper, LoggingService logger)
        {
            this.orderHelper = orderHelper;
            this.logger = logger;
        }

        [Route("CreateOrder")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO options)
        {
            OperationResult<Order> result;
            ObjectResult responseMessage;
            try
            {
                result = await orderHelper.CreateOrder(options);
                if (!result.HasErrors)
                {
                    responseMessage = StatusCode(200, result.Entities.First());
                }
                else
                {
                    var errorMessage = "Error while create Order";
                    responseMessage = StatusCode(400, errorMessage);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }

            WebApiLogEntryDTO webApiLogEntry = new WebApiLogEntryDTO()
            {
                Endpoint = "CreateOrder",
                HttpVerb = HttpRequestType.POST,
                Request = $"Create new Order with display identity { result.Entities.First().Identity }",
                Response = responseMessage.StatusCode.ToString(),
                RequestParams = options.ToString()
            };
            await logger.RestLog<Order>(webApiLogEntry, result);

            return responseMessage;
        }

        [Route("SearchOrders")]
        [HttpGet]
        public async Task<IActionResult> SearchOrders([FromQuery] string search)
        {
            QueryOperationResult<Order> result;
            ObjectResult responseMessage;
            try
            {
                result = await orderHelper.SearchOrders(search);

                if (!result.HasErrors)
                {
                    responseMessage = StatusCode(200, result.Entities);
                }
                else
                {
                    var errorMessage = "Error while search Order." + result.Exceptions.FirstOrDefault().Message;
                    responseMessage = StatusCode(400, errorMessage);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }

            return responseMessage;
        }

        [Route("GetOrders")]
        [HttpGet]
        public async Task<IActionResult> GetOrder([FromQuery] string identity)
        {
            QueryOperationResult<Order> result;
            ObjectResult responseMessage;
            try
            {
                result = await orderHelper.GetOrders(identity);

                if (!result.HasErrors)
                {
                    responseMessage = StatusCode(200, result.Entities);
                }
                else
                {
                    var errorMessage = "Error while get Order." + result.Exceptions.FirstOrDefault().Message;
                    responseMessage = StatusCode(400, errorMessage);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }

            return responseMessage;
        }

        [Route("RemoveOrder")]
        [HttpDelete]
        public async Task<IActionResult> RemoveOrder([FromBody] string identity)
        {
            OperationResult<Order> result;
            ObjectResult responseMessage;

            try
            {
                result = await orderHelper.RemoveOrder(identity);

                if (!result.HasErrors)
                {
                    responseMessage = StatusCode(200, identity);
                }
                else
                {
                    var errorMessage = "Error while remove Order";
                    responseMessage = StatusCode(400, errorMessage);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }

            WebApiLogEntryDTO webApiLogEntry = new WebApiLogEntryDTO()
            {
                Endpoint = "RemoveOrder",
                HttpVerb = HttpRequestType.DELETE,
                Request = $"Delete Order with identity { identity }",
                Response = responseMessage.StatusCode.ToString(),
                RequestParams = identity
            };
            await logger.RestLog<Order>(webApiLogEntry, result);

            return responseMessage;
        }
    }
}
