using System.ComponentModel.DataAnnotations;

namespace InvestTeam.AutoBox.SelfService.Web.API.Models
{
    public class SignInCredentialsDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
