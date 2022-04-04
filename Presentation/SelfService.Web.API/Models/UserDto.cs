using InvestTeam.AutoBox.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvestTeam.AutoBox.SelfService.Web.API.Models
{
    public class UserDto
    {
        public string Identity { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public Role Role { get; set; }
    }
}
