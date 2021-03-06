﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatingAPI.Controllers.Queries;
using DatingAPI.Models;

namespace DatingAPI.Core
{
    public interface IDatingRepository
    {
        void Add<T>(T entity) where T : class;

        void Remove<T>(T entity) where T : class;

        Task<QueryResult<User>> GetUsersAsync(UserQuery queryObject);

        Task<User> GetUserAsync(int id);

        Task<Photo> GetPhotoAsync(string id);

        Task<Photo> GetMainPhotoForUserAsync(int userId);

        Task<Like> GetLikeAsync(int userId, int recipientId);

        Task<Message> GetMessageAsync(Guid id);

        Task<QueryResult<Message>> GetMessagesForUserAsync(MessageQuery queryObject);

        Task<IEnumerable<Message>> GetMessagesThreadAsync(int userId, int recipientId);
    }
}