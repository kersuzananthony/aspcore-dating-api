using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DatingAPI.Extensions
{
    public static class IQueryableExtension
    {
        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, IQueryObject queryObject)
        {
            if (queryObject.Page <= 0)
            {
                queryObject.Page = 1;
            }

            if (queryObject.PageSize <= 0 || queryObject.PageSize > 20)
            {
                queryObject.PageSize = 20;
            }

            return query.Skip((queryObject.Page - 1) * queryObject.PageSize).Take(queryObject.PageSize);
        }
        
        public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, IQueryObject queryObject,
            Dictionary<string, Expression<Func<T, object>>> columnsMap)
        {
            if (string.IsNullOrWhiteSpace(queryObject.SortBy) || !columnsMap.ContainsKey(queryObject.SortBy))
                return query;
            
            return queryObject.IsSortAscending ? query.OrderBy(columnsMap[queryObject.SortBy]) : query.OrderByDescending(columnsMap[queryObject.SortBy]);           
        }
    }
}