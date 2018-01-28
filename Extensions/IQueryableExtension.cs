using System.Linq;

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
    }
}