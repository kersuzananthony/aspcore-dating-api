using System;

namespace DatingAPI.Models
{
    public class Photo
    {
        public Guid Id { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public bool IsMain { get; set; }
        
        public User User { get; set; }
        
        public int UserId { get; set; }
    }
}