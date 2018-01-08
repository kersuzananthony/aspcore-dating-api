using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DatingAPI.Controllers.Response
{
    public class UserDetailResponse
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Gender { get; set; }

        public int Age { get; set; }

        public string KnownAs { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime LastActiveAt { get; set; }
        
        public string Introduction { get; set; }
        
        public string LookingFor { get; set; }
        
        public string Interests { get; set; }

        public string Country { get; set; }
        
        public string City { get; set; }
        
        public ICollection<PhotoResponse> Photos { get; set; }
        
        public string PhotoUrl { get; set; }
        
        public UserDetailResponse()
        {
            Photos = new Collection<PhotoResponse>();
        }
    }
}