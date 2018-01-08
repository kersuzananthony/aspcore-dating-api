using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DatingAPI.Models;

namespace DatingAPI.Controllers.Response
{
    public class UserListResponse
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Gender { get; set; }

        public int Age { get; set; }

        public string KnownAs { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime LastActiveAt { get; set; }
        
        public string Country { get; set; }
        
        public string City { get; set; }
        
        public string PhotoUrl { get; set; }
    }
}