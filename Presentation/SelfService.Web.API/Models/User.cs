using Microsoft.AspNetCore.Identity;

namespace InvestTeam.AutoBox.SelfService.Web.API.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
    }
}
