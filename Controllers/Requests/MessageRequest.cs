using System;
using System.ComponentModel.DataAnnotations;

namespace DatingAPI.Controllers.Requests
{
    public class MessageRequest
    {
        public int SenderId { get; set; }

        [Required]
        public int RecipentId { get; set; }

        public DateTime SendAt { get; set; }

        [Required]
        public string Content { get; set; }

        public MessageRequest()
        {
            SendAt = DateTime.Now;
        }
    }
}