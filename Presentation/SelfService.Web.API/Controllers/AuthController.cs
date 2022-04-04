using InvestTeam.AutoBox.SelfService.Web.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace InvestTeam.AutoBox.SelfService.Web.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [RequireHttps]
    public class AuthController : ControllerBase
    {
        private SignInManager<User> SignInManager;
        private UserManager<User> UserManager;
        private IConfiguration Configuration;

        public AuthController(SignInManager<User> signMgr,
                UserManager<User> usrMgr,
                IConfiguration config)
        {
            SignInManager = signMgr;
            UserManager = usrMgr;
            Configuration = config;
        }

        [Route("SignIn")]
        [HttpPost]
        public async Task<object> SignIn([FromBody] SignInCredentialsDto creds)
        {
            User user = await UserManager.FindByEmailAsync(creds.Email);
            SignInResult result = await SignInManager.CheckPasswordSignInAsync(user, creds.Password, true);

            if (result.Succeeded)
            {
                SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
                {
                    Subject = (await SignInManager.CreateUserPrincipalAsync(user)).Identities.First(),
                    Expires = DateTime.Now.AddMinutes(int.Parse(Configuration["BearerTokens:ExpiryMins"])),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["BearerTokens:Key"])), SecurityAlgorithms.HmacSha256Signature)
                };
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                SecurityToken secToken = new JwtSecurityTokenHandler().CreateToken(descriptor);
                return new { success = true, token = handler.WriteToken(secToken) };
            }
            return new { success = false };
        }
    }
}
