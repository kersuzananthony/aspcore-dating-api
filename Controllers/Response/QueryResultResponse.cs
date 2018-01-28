using System.Collections.Generic;

namespace DatingAPI.Controllers.Response
{
    public class QueryResultResponse<T>
    {
        public int TotalItems { get; set; }

        public IEnumerable<T> Results { get; set; }
    }
}