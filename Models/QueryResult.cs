using System.Collections.Generic;

namespace DatingAPI.Models
{
    public class QueryResult<T>
    {
        public int TotalItems { get; set; }

        public IEnumerable<T> Results { get; set; }
    }
}