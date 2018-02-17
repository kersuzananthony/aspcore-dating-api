using DatingAPI.Extensions;

namespace DatingAPI.Controllers.Queries
{
    public class MessageQuery : IQueryObject
    {
        private const int MaxSize = 20;

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
            set => _pageSize = (value > MaxSize || value <= 0) ? MaxSize : value;
        }

        public int UserId { get; set; }

        public string SortBy { get; set; } = "sendAt";

        public bool IsSortAscending { get; set; } = false;

        public string MessageContainer { get; set; } = "Unread";
    }
}