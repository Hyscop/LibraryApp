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
using AutoMapper;
using api.DTOs;

namespace tests.UnitTests
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IJwtTokenService> _mockJwtTokenService;
        private readonly Mock<IPasswordHasher> _mockPasswordHasher;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _mockJwtTokenService = new Mock<IJwtTokenService>();
            _mockMapper = new Mock<IMapper>();
            _userService = new UserService(_mockUserRepository.Object, _mockPasswordHasher.Object, _mockJwtTokenService.Object, _mockMapper.Object);
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
            var userDto = new UserForCreationDto
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "hashedpassword",
                Role = UserRole.RegularUser
            };
            var user = new User { Username = "testuser", Email = "test@example.com", PasswordHash = "hashedpassword", Role = UserRole.RegularUser };

            _mockMapper.Setup(m => m.Map<User>(userDto)).Returns(user);

            // Act
            _userService.AddUser(userDto);

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

            var updatedUserDto = new UserForUpdateDto
            {
                UserId = 1,
                Username = "Changed User",
                Email = "changed@mail.com",
                Password = "HAshPassword",
                Role = UserRole.Admin
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
            _mockMapper.Setup(m => m.Map<User>(updatedUserDto)).Returns(updatedUser);

            //Act

            _userService.UpdateUser(updatedUserDto);

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
        public void Authenticate_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var username = "testuser";
            var password = "password";
            var user = new User { Username = username, PasswordHash = "hashedPassword" };

            _mockUserRepository.Setup(repo => repo.GetByUsername(username)).Returns(user);
            _mockPasswordHasher.Setup(hasher => hasher.VerifyPassword("hashedPassword", password)).Returns(true);
            _mockJwtTokenService.Setup(service => service.GenerateToken(user)).Returns("token");

            // Act
            var result = _userService.Authenticate(username, password);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be("token");
        }

        [Fact]
        public void Authenticate_ShouldReturnNull_WhenCredentialsAreInvalid()
        {
            // Arrange
            var username = "testuser";
            var password = "wrongpassword";

            _mockUserRepository.Setup(repo => repo.GetByUsername(username)).Returns((User)null);

            // Act
            var result = _userService.Authenticate(username, password);

            // Assert
            result.Should().BeNull();
        }



    }
}