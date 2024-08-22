using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;
using api.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace tests.UnitTests
{
    public class UserBookProgressServiceTest
    {

        private readonly Mock<IUserBookProgressRepository> _mockUserBookProgressRepository;

        private readonly UserBookProgressService _userBookProgressService;

        private readonly Mock<IBookRepository> _mockBookRepository;

        public UserBookProgressServiceTest()
        {
            _mockUserBookProgressRepository = new Mock<IUserBookProgressRepository>();

            _mockBookRepository = new Mock<IBookRepository>();

            _userBookProgressService = new UserBookProgressService(_mockUserBookProgressRepository.Object, _mockBookRepository.Object);
        }

        [Fact]
        public void UserBookProgress_GetProgressByUserAndBook_ShouldReturnProgress_WhenExists()
        {
            //Arrange
            var userId = 1;
            var bookId = 1;
            var expectedProgress = new UserBookProgress
            {
                UserBookProgressId = 1,
                UserId = userId,
                BookId = bookId,
                CurrentPage = 100,
                StartDate = DateTime.UtcNow.AddDays(-5),
                LastUpdatedTime = DateTime.UtcNow
            };

            _mockUserBookProgressRepository.Setup(repo => repo.GetProgressByUserAndBook(userId, bookId)).Returns(expectedProgress);


            //Act

            var result = _userBookProgressService.GetProgressByUserAndBook(userId, bookId);

            //Assert

            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);
            result.BookId.Should().Be(bookId);
            result.CurrentPage.Should().Be(100);
            result.StartDate.Should().NotBeAfter(DateTime.UtcNow);
            result.LastUpdatedTime.Should().BeAfter(DateTime.UtcNow.AddDays(-5));
        }

        [Fact]
        public void UserBookProgress_StartReadingBook_ShouldAddNewProgress_WhenProgressNotExists()
        {
            // Arrange

            var userId = 1;
            var bookId = 1;
            var newProgress = new UserBookProgress
            {
                UserId = userId,
                BookId = bookId,
                CurrentPage = 1,
                StartDate = DateTime.UtcNow,
                LastUpdatedTime = DateTime.Now
            };

            _mockUserBookProgressRepository.Setup(repo => repo.GetProgressByUserAndBook(userId, bookId)).Returns((UserBookProgress)null);
            _mockUserBookProgressRepository.Setup(repo => repo.AddProgress(It.IsAny<UserBookProgress>())).Verifiable();

            // When

            _userBookProgressService.StartReadingBook(userId, bookId);

            // Then

            _mockUserBookProgressRepository.Verify(repo => repo.AddProgress(It.Is<UserBookProgress>(p =>
            p.UserId == userId &&
            p.BookId == bookId &&
            p.CurrentPage == 1 &&
            p.StartDate != default(DateTime) &&
            p.LastUpdatedTime != default(DateTime)
            )), Times.Once);
        }

        [Fact]
        public void UserBookProgress_StartReadingBook_ShouldThrowExcpetion_WhenProgressAlreadyExists()
        {
            // Arrange
            var userId = 1;
            var bookId = 1;
            var existingProgress = new UserBookProgress
            {
                UserBookProgressId = 1,
                UserId = userId,
                BookId = bookId,
                CurrentPage = 50,
                StartDate = DateTime.UtcNow.AddDays(-10),
                LastUpdatedTime = DateTime.UtcNow.AddDays(-1)
            };

            _mockUserBookProgressRepository.Setup(repo => repo.GetProgressByUserAndBook(userId, bookId)).Returns(existingProgress);
            // Act
            // Assert

            Assert.Throws<InvalidOperationException>(() => _userBookProgressService.StartReadingBook(userId, bookId));
        }

        [Fact]
        public void UserBookProgress_UpdateReadingProgress_ShouldUpdateProgress_WhenExists()
        {
            // Arrange
            var userId = 1;
            var bookId = 1;
            var newCurrentPage = 150;
            var constLastUpdatetime = DateTime.UtcNow.AddDays(-2);


            var existingProgress = new UserBookProgress
            {
                UserBookProgressId = 1,
                UserId = userId,
                BookId = bookId,
                CurrentPage = 100,
                StartDate = DateTime.UtcNow.AddDays(-10),
                LastUpdatedTime = DateTime.UtcNow.AddDays(-2)
            };

            _mockUserBookProgressRepository.Setup(repo => repo.GetProgressByUserAndBook(userId, bookId))
                                           .Returns(existingProgress);

            _mockBookRepository.Setup(repo => repo.GetBookById(bookId))
                               .Returns(new Book
                               {
                                   Id = bookId,
                                   TotalPages = 300
                               });

            _mockUserBookProgressRepository.Setup(repo => repo.UpdateProgress(It.IsAny<UserBookProgress>())).Verifiable();

            // Act
            _userBookProgressService.UpdateReadingProgress(userId, bookId, newCurrentPage);

            // Assert
            _mockUserBookProgressRepository.Verify(repo => repo.UpdateProgress(It.Is<UserBookProgress>(p =>
                p.UserId == userId &&
                p.BookId == bookId &&
                p.CurrentPage == newCurrentPage &&
                p.LastUpdatedTime > constLastUpdatetime
            )), Times.Once);
        }

        [Fact]
        public void UserBookProgress_UpdateProgress_ShouldThrowException_When_ProgressNotExists()
        {
            // Arrange
            var userId = 1;
            var bookId = 1;
            var newCurrentPage = 50;

            _mockUserBookProgressRepository.Setup(repo => repo.GetProgressByUserAndBook(userId, bookId)).Returns((UserBookProgress)null);
            // Act

            // Assert
            Assert.Throws<KeyNotFoundException>(() => _userBookProgressService.UpdateReadingProgress(userId, bookId, newCurrentPage));
        }

        [Fact]
        public void UserBookProgress_UpdateProgress_ShouldThrowException_WhenCurrentPageIsInvalid()
        {
            // Arrange
            var userId = 1;
            var bookId = 1;
            var newCurrentPage = -5;
            var existingProgress = new UserBookProgress
            {
                UserBookProgressId = 1,
                UserId = userId,
                BookId = bookId,
                CurrentPage = 100,
                StartDate = DateTime.UtcNow.AddDays(-10),
                LastUpdatedTime = DateTime.UtcNow.AddDays(-2)
            };

            _mockUserBookProgressRepository.Setup(repo => repo.GetProgressByUserAndBook(userId, bookId)).Returns(existingProgress);

            // Act

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _userBookProgressService.UpdateReadingProgress(userId, bookId, newCurrentPage));
        }
    }
}