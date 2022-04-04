using InvestTeam.AutoBox.Domain.Enums;

namespace InvestTeam.AutoBox.SelfService.Web.API.Models
{
    public class UserDto
    {
        public string Identity { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public Role Role { get; set; }
    }
}
