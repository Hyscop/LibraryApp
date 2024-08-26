using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using api.Models;
using api.Interfaces;
using api.Services;
using api.Repositories;
using Moq;
using Azure.Core.Pipeline;
using FluentAssertions;
using System.IO.Compression;
using Microsoft.VisualBasic;

namespace tests.UnitTests
{
    public class UserStatsServiceTest
    {
        private readonly Mock<IUserStatsRepository> _mockUserStatsRepository;
        private readonly UserStatsService _userStatsService;

        public UserStatsServiceTest()
        {
            _mockUserStatsRepository = new Mock<IUserStatsRepository>();
            _userStatsService = new UserStatsService(_mockUserStatsRepository.Object);
        }
        [Fact]
        public void UserStatsService_GetUserStatsByUserId_ShouldReturnUserStats()
        {
            //Arrange

            var userId = 1;
            var expectedStats = new UserStats
            {
                UserStatsId = 1,
                UserId = userId,
                TotalBooksRead = 5,
                TotalPagesRead = 1234

            };

            _mockUserStatsRepository.Setup(repo => repo.GetUserStatsByUserId(userId)).Returns(expectedStats);

            //Act

            var result = _userStatsService.GetUserStatsByUserId(userId);

            //Arrange

            result.Should().NotBeNull();
            result.UserId.Should().Be(userId);
            result.TotalBooksRead.Should().Be(5);
            result.TotalPagesRead.Should().Be(1234);

        }

        [Fact]
        public void UserStatsService_UpdateUserStats_ShouldUpdateStats()
        {
            //Arrange
            var userStats = new UserStats
            {
                UserStatsId = 1,
                UserId = 1,
                TotalBooksRead = 7,
                TotalPagesRead = 3000
            };

            _mockUserStatsRepository.Setup(repo => repo.GetUserStatsByUserId(userStats.UserId)).Returns(userStats);
            _mockUserStatsRepository.Setup(repo => repo.UpdateUserStats(It.IsAny<UserStats>())).Verifiable();

            //Act

            userStats.TotalBooksRead = 5;
            userStats.TotalPagesRead = 1234;
            _userStatsService.UpdateUserStats(userStats);

            //Assert

            _mockUserStatsRepository.Verify(repo => repo.UpdateUserStats(It.Is<UserStats>(us => us.TotalBooksRead == 5 && us.TotalPagesRead == 1234)), Times.Once);

        }

        [Fact]
        public void UserStatsService_IncrementBooksRead_ShouldIncrementBooksRead()
        {
            // Arrange
            var userId = 1;
            var userStats = new UserStats
            {
                UserStatsId = 1,
                UserId = userId,
                TotalBooksRead = 5,
                TotalPagesRead = 1000
            };

            _mockUserStatsRepository.Setup(repo => repo.GetUserStatsByUserId(userId)).Returns(userStats);
            _mockUserStatsRepository.Setup(repo => repo.UpdateUserStats(It.IsAny<UserStats>())).Verifiable();

            // Act

            _userStatsService.IncrementBooksRead(userId);

            // Assert

            _mockUserStatsRepository.Verify(repo => repo.UpdateUserStats(It.Is<UserStats>(us => us.TotalBooksRead == 6)), Times.Once);
        }

        [Fact]
        public void UserStatsService_IncrementPagesRead_ShouldIncrementPageRead()
        {
            // Arrange
            var userId = 1;
            var pagesToAdd = 50;
            var userStats = new UserStats
            {
                UserStatsId = 1,
                UserId = userId,
                TotalBooksRead = 5,
                TotalPagesRead = 150
            };

            _mockUserStatsRepository.Setup(repo => repo.GetUserStatsByUserId(userId)).Returns(userStats);
            _mockUserStatsRepository.Setup(repo => repo.UpdateUserStats(It.IsAny<UserStats>())).Verifiable();

            // Act

            _userStatsService.IncrementPagesRead(userId, pagesToAdd);

            // Assert

            _mockUserStatsRepository.Verify(repo => repo.UpdateUserStats(It.Is<UserStats>(us => us.TotalPagesRead == 200)), Times.Once);
        }

        [Fact]
        public void UserStatsService_ResetUserStats_ShouldResetStats()
        {
            // Arrange

            var userId = 1;
            var userStats = new UserStats
            {
                UserStatsId = 1,
                UserId = userId,
                TotalBooksRead = 5,
                TotalPagesRead = 1000
            };

            _mockUserStatsRepository.Setup(repo => repo.GetUserStatsByUserId(userId)).Returns(userStats);
            _mockUserStatsRepository.Setup(repo => repo.UpdateUserStats(It.IsAny<UserStats>())).Verifiable();

            // Act
            _userStatsService.ResetUserStats(userId);

            // Arrange

            _mockUserStatsRepository.Verify(repo => repo.UpdateUserStats(It.Is<UserStats>(us => us.TotalBooksRead == 0 && us.TotalPagesRead == 0)), Times.Once);
        }


    }
}