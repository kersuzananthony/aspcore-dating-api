using DatingAPI.Extensions;

namespace DatingAPI.Controllers.Queries
{
    public class UserQuery : IQueryObject
    {
        private const int MAX_SIZE = 20;

        private int _page = 1;
        public int Page
        {
            get => _page;
            set => _page = (value <= 0) ? 1 : value;
        }

        private int _pageSize = 20;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MAX_SIZE || value <= 0) ? MAX_SIZE : value;
        }
    }
}