using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;

namespace api.Services
{
    public class UserStatsService : IUserStatsService
    {
        private readonly IUserStatsRepository _userStatsRepository;
        public UserStatsService(IUserStatsRepository userStatsRepository)
        {
            _userStatsRepository = userStatsRepository;
        }

        public UserStats GetUserStatsByUserId(int userId)
        {
            return _userStatsRepository.GetUserStatsByUserId(userId);
        }

        public void IncrementBooksRead(int userId)
        {
            var userStats = _userStatsRepository.GetUserStatsByUserId(userId);
            if (userStats != null)
            {
                userStats.TotalBookRead += 1;
                _userStatsRepository.UpdateUserStats(userStats);
            }
        }

        public void IncrementPagesRead(int userId, int pages)
        {
            var userStats = _userStatsRepository.GetUserStatsByUserId(userId);

            if (userStats != null)
            {
                userStats.TotalPagesRead += pages;
                _userStatsRepository.UpdateUserStats(userStats);
            }
        }

        public void ResetUserStats(int userId)
        {
            var userStats = _userStatsRepository.GetUserStatsByUserId(userId);

            if (userStats != null)
            {
                userStats.TotalBookRead = 0;
                userStats.TotalPagesRead = 0;
                _userStatsRepository.UpdateUserStats(userStats);
            }
        }

        public void UpdateUserStats(UserStats userStats)
        {
            var existingStats = _userStatsRepository.GetUserStatsByUserId(userStats.UserId);
            if (existingStats != null)
            {
                existingStats.TotalBookRead = userStats.TotalBookRead;
                existingStats.TotalPagesRead = userStats.TotalPagesRead;

                _userStatsRepository.UpdateUserStats(existingStats);
            }
        }
    }
}