using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IUserStatsService
    {
        public UserStats GetUserStatsByUserId(int userId);
        void UpdateUserStats(UserStats userStats);
        void IncrementBooksRead(int userId);
        void IncrementPagesRead(int userId, int pages);
        void ResetUserStats(int userId);
    }
}