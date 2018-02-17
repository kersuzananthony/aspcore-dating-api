using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DatingAPI.Controllers.Queries;
using DatingAPI.Core;
using DatingAPI.Extensions;
using DatingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAPI.Data.Repositories
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;
        
        private static Dictionary<string, Expression<Func<User, object>>> GetUserColumnsMap()
        {
            return new Dictionary<string, Expression<Func<User, object>>>()
            {
                ["active"] = user => user.LastActiveAt,
                ["created"] = user => user.CreatedAt
            };
        }

        public DatingRepository(DataContext context)
        {
            _context = context;
        }
        
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Remove<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<QueryResult<User>> GetUsersAsync([FromQuery] UserQuery queryObject)
        {
            var query = _context.Users
                .Where(u => u.Id != queryObject.UserId)
                .Where(u => u.Gender == queryObject.Gender)
                .Include(u => u.Photos)
                .AsQueryable();

            if (queryObject.Likers)
            {
                query = query.Where(u => u.Likers.Any(l => l.LikerId == u.Id));
            }

            if (queryObject.Likees)
            {
                query = query.Where(u => u.Likees.Any(l => l.LikeeId == u.Id));
            }

            query = query.Where(u => u.DateOfBirth.CalculateAge() >= queryObject.MinAge && u.DateOfBirth.CalculateAge() <= queryObject.MaxAge);

            query = query.ApplyOrdering(queryObject, GetUserColumnsMap());
            
            var result = new QueryResult<User>
            {
                TotalItems = await query.CountAsync()
            };
            
            query = query.ApplyPaging(queryObject);
            result.Results = await query.ToListAsync();

            return result;
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await _context.Users.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Photo> GetPhotoAsync(string id)
        {
            return await _context.Photos.FirstOrDefaultAsync(p => p.Id == new Guid(id));
        }

        public async Task<Photo> GetMainPhotoForUserAsync(int userId)
        {
            return await _context.Photos.Where(p => p.UserId == userId).FirstOrDefaultAsync(p => p.IsMain);
        }

        public async Task<Like> GetLikeAsync(int userId, int recipientId)
        {
            return await _context.Likes.FirstOrDefaultAsync(l => l.LikerId == userId && l.LikeeId == recipientId);
        }

        public async Task<Message> GetMessageAsync(Guid id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<QueryResult<Message>> GetMessagesForUserAsync(MessageQuery queryObject)
        {
            var query = _context.Messages
                .Include(m => m.Sender).ThenInclude(u => u.Photos)
                .Include(m => m.Recipient).ThenInclude(u => u.Photos)
                .AsQueryable();

            switch (queryObject.MessageContainer.ToLower())
            {
                case "inbox":
                    query = query.Where(m => m.RecipientId == queryObject.UserId);
                    break;
                case "outbox":
                    query = query.Where(m => m.SenderId == queryObject.UserId);
                    break;
                default:
                    query = query.Where(m => m.RecipientId == queryObject.UserId && m.IsRead == false);
                    break;
            }

            query.ApplyOrdering(queryObject, new Dictionary<string, Expression<Func<Message, object>>>
            {
                ["sendAt"] = message => message.SendAt
            });
            
            var result = new QueryResult<Message>
            {
                TotalItems = await query.CountAsync()
            };

            query = query.ApplyPaging(queryObject);
            result.Results = await query.ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Message>> GetMessagesThreadAsync(int userId, int recipientId)
        {
            return await _context.Messages
                .Include(m => m.Sender).ThenInclude(u => u.Photos)
                .Include(m => m.Recipient).ThenInclude(u => u.Photos)
                .Where(m => (m.RecipientId == userId && m.SenderId == recipientId) ||
                            (m.RecipientId == recipientId && m.SenderId == userId))
                .OrderByDescending(m => m.SendAt)
                .ToListAsync();
        }
    }
}