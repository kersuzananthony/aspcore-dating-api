using System.ComponentModel.DataAnnotations;

namespace DatingAPI.Controllers.Requests
{
    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "You must specify a password between 6 and 20 characters.")]
        public string Password { get; set; }
    }
}