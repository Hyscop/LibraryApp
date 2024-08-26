using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Models;

namespace api.Interfaces
{
    public interface IUserStatsService
    {
        public UserStatsDto GetUserStatsByUserId(int userId);
        void UpdateUserStats(UserStatsDto userStatsDto);
        void IncrementBooksRead(int userId);
        void IncrementPagesRead(int userId, int pages);
        void ResetUserStats(int userId);
    }
}