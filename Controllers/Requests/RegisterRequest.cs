using System;
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

        [Required]
        public string Gender { get; set; }

        [Required]
        public string KnownAs { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime LastActiveAt { get; set; }

        public RegisterRequest()
        {
            CreatedAt = DateTime.Now;
            LastActiveAt = DateTime.Now;
        }
    }
}