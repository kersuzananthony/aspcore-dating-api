﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DatingAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string KnownAs { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime LastActiveAt { get; set; }
        
        public string Introduction { get; set; }
        
        public string LookingFor { get; set; }
        
        public string Interests { get; set; }

        public string Country { get; set; }
        
        public string City { get; set; }
        
        public ICollection<Photo> Photos { get; set; }

        public ICollection<Like> Likers { get; set; }

        public ICollection<Like> Likees { get; set; }
        
        public ICollection<Message> MessagesSent { get; set; }
        
        public ICollection<Message> MessagesReceived { get; set; }
        
        public User()
        {
            Photos = new Collection<Photo>();
            Likers = new Collection<Like>();
            Likees = new Collection<Like>();
            MessagesSent = new Collection<Message>();
            MessagesReceived = new Collection<Message>();
        }
    }
}
