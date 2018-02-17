using System;
using DatingAPI.Models;

namespace DatingAPI.Controllers.Response
{
    public class MessageResponse
    {
        public Guid Id { get; set; }
        
        public int SenderId { get; set; }
        
        public string SenderKnownAs { get; set; }
        
        public string SenderPhotoUrl { get; set; }
        
        public int RecipientId { get; set; }
        
        public string RecipientKnownAs { get; set; }
        
        public string RecipientPhotoUrl { get; set; }
        
        public string Content { get; set; }
        
        public bool IsRead { get; set; }
        
        public DateTime? ReadAt { get; set; }
        
        public DateTime SendAt { get; set; }
    }
}