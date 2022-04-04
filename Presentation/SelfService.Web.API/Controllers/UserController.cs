namespace InvestTeam.AutoBox.SelfService.Web.API.Controllers
{
    using InvestTeam.AutoBox.Application.AppServices;
    using InvestTeam.AutoBox.Application.Common;
    using InvestTeam.AutoBox.SelfService.Web.API.Helper;
    using InvestTeam.AutoBox.SelfService.Web.API.Models;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Manager")]
    public class UserController : ControllerBase
    {
        private readonly UserHelper userHelper;
        private readonly LoggingService logger;
        public UserController(UserHelper userHelper, LoggingService logger)
        {
            this.userHelper = userHelper;
            this.logger = logger;
        }


        [Route("CreateUser")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDto dto)
        {
            OperationResult<User> result;
            ObjectResult responseMessage;
            try
            {
                result = await userHelper.CreateUser(dto);
                if (!result.HasErrors)
                {
                    responseMessage = StatusCode(200, result.Entities.First());
                }
                else
                {
                    var errorMessage = "Error while create User" + result.Exceptions.FirstOrDefault();
                    responseMessage = StatusCode(400, errorMessage);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }

            return responseMessage;
        }

        [Route("UpdateUser")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto dto)
        {
            OperationResult<User> result;
            ObjectResult responseMessage;
            try
            {
                result = await userHelper.UpdateUser(dto);
                if (!result.HasErrors)
                {
                    responseMessage = StatusCode(200, result.Entities.First());
                }
                else
                {
                    var errorMessage = "Error while update User" + result.Exceptions.FirstOrDefault();
                    responseMessage = StatusCode(400, errorMessage);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }

            return responseMessage;
        }

        [Route("GetUser")]
        [HttpGet]
        public async Task<IActionResult> GetUser([FromQuery] string identity)
        {
            OperationResult<UserDto> result;
            ObjectResult responseMessage = null;
            try
            {
                result = await userHelper.GetUser(identity);

                if (!result.HasErrors)
                {
                    responseMessage = StatusCode(200, result.Entities.FirstOrDefault());
                }
                else
                {
                    var errorMessage = "Error while get User." + result.Exceptions.FirstOrDefault().Message;
                    responseMessage = StatusCode(400, errorMessage);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }

            return responseMessage;
        }

        [Route("RemoveUser")]
        [HttpDelete]
        public async Task<IActionResult> RemoveUser([FromQuery] string id)
        {
            OperationResult<User> result;
            ObjectResult responseMessage;
            try
            {
                result = await userHelper.RemoveUser(id);
                if (!result.HasErrors)
                {
                    responseMessage = StatusCode(200, result.Entities.First());
                }
                else
                {
                    var errorMessage = "Error while delete User." + result.Exceptions.FirstOrDefault();
                    responseMessage = StatusCode(400, errorMessage);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }

            return responseMessage;
        }
    }
}
