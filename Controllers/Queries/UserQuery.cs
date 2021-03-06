﻿using DatingAPI.Extensions;

namespace DatingAPI.Controllers.Queries
{
    public class UserQuery : IQueryObject
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

        public string Gender { get; set; }

        public int MaxAge { get; set; } = 99;

        public int MinAge { get; set; } = 18;

        public string SortBy { get; set; }
        
        public bool IsSortAscending { get; set; }

        public bool Likees { get; set; } = false;

        public bool Likers { get; set; } = false;
    }
}