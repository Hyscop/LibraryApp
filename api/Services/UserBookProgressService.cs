using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;

namespace api.Services
{
    public class UserBookProgressService : IUserBookProgressService
    {
        private readonly IUserBookProgressRepository _progressRepository;

        private readonly IBookRepository _bookRepository;

        public UserBookProgressService(IUserBookProgressRepository progressRepository, IBookRepository bookRepository)
        {
            _progressRepository = progressRepository;
            _bookRepository = bookRepository;
        }

        public void FinishReadingBook(int userId, int bookId)
        {
            throw new NotImplementedException();
        }

        public UserBookProgress GetProgressByUserAndBook(int userId, int bookId)
        {
            return _progressRepository.GetProgressByUserAndBook(userId, bookId);
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

            Console.WriteLine($"Before update: {progress.LastUpdatedTime}"); // Log before update

            progress.CurrentPage = currentPage;
            progress.LastUpdatedTime = DateTime.UtcNow; // Make sure this line is correctly updating the timestamp

            Console.WriteLine($"After update: {progress.LastUpdatedTime}"); // Log before update

            _progressRepository.UpdateProgress(progress);
        }
    }
}