using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;
using AutoMapper;
using api.DTOs;

namespace api.Services
{
    public class UserBookProgressService : IUserBookProgressService
    {
        private readonly IUserBookProgressRepository _progressRepository;

        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserStatsRepository _userStatsRepository;
        private readonly IMapper _mapper;


        public UserBookProgressService(IUserBookProgressRepository progressRepository, IBookRepository bookRepository, IUserRepository userRepository, IUserStatsRepository userStatsRepository, IMapper mapper)
        {
            _progressRepository = progressRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _userStatsRepository = userStatsRepository;
            _mapper = mapper;
        }

        public void FinishReadingBook(int userId, int bookId)
        {
            var progress = _progressRepository.GetProgressByUserAndBook(userId, bookId);
            if (progress == null)
            {
                throw new KeyNotFoundException("No reading progress found");
            }

            _progressRepository.DeleteProgress(progress);

            var userStats = _userStatsRepository.GetUserStatsByUserId(userId);

            if (userStats != null)
            {
                userStats.TotalBooksRead += 1;
                userStats.TotalPagesRead += progress.Book.TotalPages;
                _userStatsRepository.UpdateUserStats(userStats);
            }
        }




        public UserBookProgressDto GetProgressByUserAndBook(int userId, int bookId)
        {
            var progress = _progressRepository.GetProgressByUserAndBook(userId, bookId);
            if (progress == null)
            {
                return null;
            }

            return _mapper.Map<UserBookProgressDto>(progress);
        }

        public void StartReadingBook(int userId, int bookId)
        {
            var existingProgress = _progressRepository.GetProgressByUserAndBook(userId, bookId);
            if (existingProgress != null)
            {
                throw new InvalidOperationException("Reading progress for this book already exists.");
            }

            var newProgress = new UserBookProgress
            {
                UserId = userId,
                BookId = bookId,
                CurrentPage = 1,
                StartDate = DateTime.UtcNow,
                LastUpdatedTime = DateTime.UtcNow
            };

            _progressRepository.AddProgress(newProgress);
        }

        public void UpdateReadingProgress(int userId, int bookId, int currentPage)
        {
            var progress = _progressRepository.GetProgressByUserAndBook(userId, bookId);
            if (progress == null)
            {
                throw new KeyNotFoundException("No reading progress found for the specified user and book.");
            }

            if (currentPage < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(currentPage), "Current page must be greater than zero.");
            }


            progress.CurrentPage = currentPage;
            progress.LastUpdatedTime = DateTime.UtcNow; // Make sure this line is correctly updating the timestamp


            _progressRepository.UpdateProgress(progress);
        }
    }
}