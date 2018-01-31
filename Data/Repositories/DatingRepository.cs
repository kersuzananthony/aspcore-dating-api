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
    }
}