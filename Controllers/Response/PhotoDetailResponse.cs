using System;

namespace DatingAPI.Controllers.Response
{
    public class PhotoDetailResponse
    {
        public Guid Id { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public bool IsMain { get; set; }

        public string PublicId { get; set; }
    }
}