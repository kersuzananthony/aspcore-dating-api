using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DatingAPI.Controllers.Requests
{
    public class PhotoCreationRequest
    {
        public string Url { get; set; }

        [Required]
        public IFormFile File { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public string PublicId { get; set; }

        public PhotoCreationRequest()
        {
            CreatedAt = DateTime.Now;
        }
    }
}