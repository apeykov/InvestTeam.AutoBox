namespace InvestTeam.AutoBox.SelfService.Web.API.Controllers
{
    using InvestTeam.AutoBox.Application.AppServices;
    using InvestTeam.AutoBox.Application.Common;
    using InvestTeam.AutoBox.Domain.Entities;
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
    public class VechicleController : ControllerBase
    {
        private readonly VechicleHelper vechicleHelper;
        private readonly LoggingService logger;
        public VechicleController(VechicleHelper vechicleHelper, LoggingService logger)
        {
            this.vechicleHelper = vechicleHelper;
            this.logger = logger;
        }

        [Route("GetVechicles")]
        [HttpGet]
        public async Task<IActionResult> GetVechicles([FromQuery] string identity)
        {
            QueryOperationResult<Vechicle> result;
            ObjectResult responseMessage;
            try
            {
                result = await vechicleHelper.GetVechicles(identity);

                if (!result.HasErrors)
                {
                    responseMessage = StatusCode(200, result.Entities);
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
    }
}
