using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using api.Services;
using api.Repositories;
using api.Interfaces;
using api.Models;
using System.Security.Cryptography;
using System.IO.Compression;
using Azure.Core.Pipeline;
using Microsoft.AspNetCore.Identity;

namespace tests.UnitTests
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IPasswordHasher> _mockPasswordHasher;
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _userService = new UserService(_mockUserRepository.Object, _mockPasswordHasher.Object);
        }

        //-------------------------------------------------------------------------------------------
        [Fact]
        public void UserService_GetByUserId_ShouldReturnUser_WhenExists()
        {
            //Arrange
            var userId = 1;
            var user = new User
            {
                UserId = userId,
                Email = "test@mail.com",
                PasswordHash = "testHash",
                Username = "test",
                Role = UserRole.RegularUser
            };
            _mockUserRepository.Setup(repo => repo.GetByUserId(userId)).Returns(user);
            //Act

            var result = _userService.GetByUserId(userId);


            //Assert

            result.Should().NotBeNull();
            result.Should().BeOfType<User>();
            result.UserId.Should().Be(userId);
            result.Username.Should().Be("test");

        }
        //-------------------------------------------------------------------------------------------
        [Fact]
        public void UserService_GetByUsername_ShouldReturnUser_WhenExists()
        {

            //Arrange
            var username = "test";
            var user = new User
            {
                UserId = 1,
                Email = "test@mail.com",
                PasswordHash = "testHash",
                Username = username,
                Role = UserRole.RegularUser
            };
            _mockUserRepository.Setup(repo => repo.GetByUsername(username)).Returns(user);
            //Act

            var result = _userService.GetByUsername(username);


            //Assert

            result.Should().NotBeNull();
            result.Should().BeOfType<User>();
            result.Username.Should().Be(username);
            result.Username.Should().Be("test");
            result.Email.Should().Be("test@mail.com");


        }
        //-------------------------------------------------------------------------------------------
        [Fact]
        public void UserService_AddUser_ShouldAddUserToRepository()
        {
            // Arrange
            var user = new User { Username = "testuser", Email = "test@example.com", PasswordHash = "hashedpassword", Role = UserRole.RegularUser };

            // Act
            _userService.AddUser(user);

            // Assert
            _mockUserRepository.Verify(repo => repo.AddUser(It.Is<User>(u => u.Username == "testuser")), Times.Once);
        }
        //-------------------------------------------------------------------------------------------
        [Fact]

        public void UserService_UpdateUser_ShouldUpdateUserDetails()
        {
            //Arrange
            var existingUser = new User
            {
                UserId = 1,
                Username = "og User",
                Email = "og@mail.com",
                PasswordHash = "passwordHash",
                Role = UserRole.RegularUser

            };

            var updatedUser = new User
            {
                UserId = 1,
                Username = "Changeed User",
                Email = "changed@mail.com",
                PasswordHash = "HAshPassword",
                Role = UserRole.Admin
            };

            _mockUserRepository.Setup(repo => repo.GetByUserId(1)).Returns(existingUser);

            //Act

            _userService.UpdateUser(updatedUser);

            //Assert

            _mockUserRepository.Verify(repo => repo.UpdateUser(It.Is<User>(u => u.UserId == 1 &&
            u.Username == "Changeed User" &&
            u.Email == "changed@mail.com" &&
            u.PasswordHash == "HAshPassword" &&
            u.Role == UserRole.Admin)), Times.Once);

            existingUser.UserId.Should().Be(1);
            existingUser.Username.Should().Be("Changeed User");
            existingUser.PasswordHash.Should().Be("HAshPassword");
            existingUser.Email.Should().Be("changed@mail.com");
            existingUser.Role.Should().Be(UserRole.Admin);
        }
        //-------------------------------------------------------------------------------------------
        [Fact]
        public void UserService_DeleteUser_ShouldReturnTrue_WhenUserExists()
        {
            //Arrange
            var userId = 1;
            var user = new User
            {
                UserId = userId,
                Username = "test",
                Email = "test@mail.com",
                PasswordHash = "1234",
                Role = UserRole.RegularUser
            };

            _mockUserRepository.Setup(repo => repo.GetByUserId(userId)).Returns(user);

            //Act

            var result = _userService.DeleteUser(userId);

            //Assert

            result.Should().BeTrue();
            _mockUserRepository.Verify(repo => repo.DeleteUser(userId), Times.Once);
        }
        [Fact]
        public void UserService_DeleteUser_ShouldReturnFalse_WhenUserNotExists()
        {
            //Arrange
            int nonUserId = 9999;

            _mockUserRepository.Setup(repo => repo.GetByUserId(nonUserId)).Returns((User)null);
            //Act

            var result = _userService.DeleteUser(nonUserId);
            //Assert
            result.Should().BeFalse();
            _mockUserRepository.Verify(repo => repo.DeleteUser(It.IsAny<int>()), Times.Never);
        }

        //-------------------------------------------------------------------------------------------
        [Fact]

        public void UserService_Authentication_ShouldReturnUser_WhenCredentialsAreCorrect()
        {
            //Arrange
            var username = "testuser";
            var password = "testpassword";
            var passwordHasher = new PasswordHasher();
            var hashedPassword = passwordHasher.HashPassword(password);

            var user = new User
            {
                UserId = 1,
                Username = username,
                Email = "test@mail.com",
                PasswordHash = hashedPassword,
                Role = UserRole.RegularUser
            };

            _mockUserRepository.Setup(repo => repo.GetByUsername(username)).Returns(user);

            var userService = new UserService(_mockUserRepository.Object, passwordHasher);

            //Act
            var result = userService.AuthenticateUser(username, password);

            //Assert

            result.Should().NotBeNull();
            result.Username.Should().Be(username);
            result.PasswordHash.Should().Be(hashedPassword);

        }

        [Fact]

        public void UserService_Authentication_ShouldReturnNull_WhenCredentialsIncorrect()
        {
            //Arrange
            var username = "testuser";
            var correctPassword = "correct";
            var incorrectPassword = "incorrect";

            var passwordHasher = new PasswordHasher();
            var hashedPassword = passwordHasher.HashPassword(correctPassword);

            var user = new User
            {
                UserId = 1,
                Username = username,
                Email = "test@mail.com",
                PasswordHash = hashedPassword,
                Role = UserRole.RegularUser
            };

            _mockUserRepository.Setup(repo => repo.GetByUsername(username)).Returns(user);

            var userService = new UserService(_mockUserRepository.Object, passwordHasher);
            //Act

            var result = userService.AuthenticateUser(username, incorrectPassword);
            //Assert

            result.Should().BeNull();
        }



    }
}