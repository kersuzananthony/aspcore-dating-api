﻿using System.Threading.Tasks;

namespace DatingAPI.Core
{
    public interface IUnitOfWork
    {
        Task<bool> CompleteAsync();
    }
}