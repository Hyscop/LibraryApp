using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Interfaces;
using api.Models;
using AutoMapper;

namespace api.Services
{
    public class UserStatsService : IUserStatsService
    {
        private readonly IUserStatsRepository _userStatsRepository;
        private readonly IMapper _mapper;
        public UserStatsService(IUserStatsRepository userStatsRepository, IMapper mapper)
        {
            _userStatsRepository = userStatsRepository;
            _mapper = mapper;
        }

        public UserStatsDto GetUserStatsByUserId(int userId)
        {
            var userStats = _userStatsRepository.GetUserStatsByUserId(userId);
            return _mapper.Map<UserStatsDto>(userStats);
        }

        public void IncrementBooksRead(int userId)
        {
            var userStats = _userStatsRepository.GetUserStatsByUserId(userId);
            if (userStats != null)
            {
                userStats.TotalBooksRead += 1;
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
                userStats.TotalBooksRead = 0;
                userStats.TotalPagesRead = 0;
                _userStatsRepository.UpdateUserStats(userStats);
            }
        }

        public void UpdateUserStats(UserStatsDto userStatsDto)
        {
            var existingStats = _userStatsRepository.GetUserStatsByUserId(userStatsDto.UserId);
            if (existingStats != null)
            {
                _mapper.Map(userStatsDto, existingStats);
                _userStatsRepository.UpdateUserStats(existingStats);
            }
        }
    }
}