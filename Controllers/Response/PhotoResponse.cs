using System;

namespace DatingAPI.Controllers.Response
{
    public class PhotoResponse
    {
        public Guid Id { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public bool IsMain { get; set; }
    }
}