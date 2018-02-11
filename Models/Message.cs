using System;

namespace DatingAPI.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        
        public int SenderId { get; set; }
        
        public User Sender { get; set; }
        
        public int RecipientId { get; set; }
        
        public User Recipient { get; set; }
        
        public string Content { get; set; }
        
        public bool IsRead { get; set; }
        
        public DateTime? ReadAt { get; set; }
        
        public DateTime SendAt { get; set; }
        
        public bool SenderDeleted { get; set; }
        
        public bool RecipientDeleted { get; set; }
    }
}