using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using AutoMapper;
using FluentAssertions;
using api.Models;
using api.DTOs;
using api.Mapper;
using Moq;
using api.Interfaces;

namespace tests.UnitTests
{
    public class AutoMapperTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<ICategoryService> _mockCategoryService;

        public AutoMapperTest()
        {
            _mockCategoryService = new Mock<ICategoryService>();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
                cfg.ConstructServicesUsing(type =>
                {
                    if (type == typeof(CategoryResolver))
                    {
                        return new CategoryResolver(_mockCategoryService.Object);
                    }
                    return null;
                });
            });
            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void MappingProfile_Should_Have_Valid_Configuration()
        {
            var configuration = _mapper.ConfigurationProvider;
            configuration.Invoking(cfg => cfg.AssertConfigurationIsValid())
                .Should().NotThrow();
        }

        [Fact]
        public void Should_Map_Book_To_BookDto_And_Back()
        {
            // Arrange
            var category = new Category { CategoryId = 1, CategoryName = "Science Fiction" };
            var book = new Book
            {
                Id = 1,
                Title = "Test Book",
                Author = "Test Author",
                TotalPages = 300,
                Category = category
            };

            _mockCategoryService.Setup(x => x.GetCategoryById(1)).Returns(category);
            // Act
            var bookDto = _mapper.Map<BookDto>(book);
            var mappedBackBook = _mapper.Map<Book>(bookDto);

            // Assert - Forward mapping
            bookDto.Should().NotBeNull();
            bookDto.Id.Should().Be(book.Id);
            bookDto.Title.Should().Be(book.Title);
            bookDto.Author.Should().Be(book.Author);
            bookDto.TotalPages.Should().Be(book.TotalPages);
            bookDto.CategoryId.Should().Be(book.Category.CategoryId);
            bookDto.CategoryName.Should().Be(book.Category.CategoryName);

            // Assert - Reverse mapping
            mappedBackBook.Should().NotBeNull();
            mappedBackBook.Id.Should().Be(bookDto.Id);
            mappedBackBook.Title.Should().Be(bookDto.Title);
            mappedBackBook.Author.Should().Be(bookDto.Author);
            mappedBackBook.TotalPages.Should().Be(bookDto.TotalPages);
            mappedBackBook.Category.Should().Be(category); // Category handling is manual since it was ignored
        }


        [Fact]
        public void Should_Map_Category_To_CategoryDto_And_Back()
        {
            // Arrange
            var category = new Category
            {
                CategoryId = 1,
                CategoryName = "Science Fiction"
            };

            // Act
            var categoryDto = _mapper.Map<CategoryDto>(category);
            var mappedBackCategory = _mapper.Map<Category>(categoryDto);

            // Assert - Forward mapping
            categoryDto.Should().NotBeNull();
            categoryDto.CategoryId.Should().Be(category.CategoryId);
            categoryDto.CategoryName.Should().Be(category.CategoryName);

            // Assert - Reverse mapping
            mappedBackCategory.Should().NotBeNull();
            mappedBackCategory.CategoryId.Should().Be(categoryDto.CategoryId);
            mappedBackCategory.CategoryName.Should().Be(categoryDto.CategoryName);
        }

        [Fact]
        public void Should_Map_User_To_UserDto_And_Back()
        {
            // Arrange
            var userStats = new UserStats { UserStatsId = 1, TotalBooksRead = 5, TotalPagesRead = 1500 };
            var user = new User
            {
                UserId = 1,
                Username = "TestUser",
                Email = "testuser@example.com",
                Role = UserRole.RegularUser,
                UserStats = userStats
            };

            // Act
            var userDto = _mapper.Map<UserDto>(user);
            var mappedBackUser = _mapper.Map<User>(userDto);

            // Assert - Forward mapping
            userDto.Should().NotBeNull();
            userDto.UserId.Should().Be(user.UserId);
            userDto.Username.Should().Be(user.Username);
            userDto.Email.Should().Be(user.Email);
            userDto.Role.Should().Be(user.Role);
            userDto.UserStats.TotalBooksRead.Should().Be(user.UserStats.TotalBooksRead);

            // Assert - Reverse mapping
            mappedBackUser.Should().NotBeNull();
            mappedBackUser.UserId.Should().Be(userDto.UserId);
            mappedBackUser.Username.Should().Be(userDto.Username);
            mappedBackUser.Email.Should().Be(userDto.Email);
            mappedBackUser.Role.Should().Be(userDto.Role);
            mappedBackUser.UserStats.TotalBooksRead.Should().Be(userDto.UserStats.TotalBooksRead);
            mappedBackUser.PasswordHash.Should().Be("");



        }

        [Fact]
        public void Should_Map_UserStats_To_UserStatsDto_And_Back()
        {
            // Arrange
            var userStats = new UserStats
            {
                UserStatsId = 1,
                TotalBooksRead = 5,
                TotalPagesRead = 1500
            };

            // Act
            var userStatsDto = _mapper.Map<UserStatsDto>(userStats);
            var mappedBackUserStats = _mapper.Map<UserStats>(userStatsDto);

            // Assert - Forward mapping
            userStatsDto.Should().NotBeNull();
            userStatsDto.UserStatsId.Should().Be(userStats.UserStatsId);
            userStatsDto.TotalBooksRead.Should().Be(userStats.TotalBooksRead);
            userStatsDto.TotalPagesRead.Should().Be(userStats.TotalPagesRead);

            // Assert - Reverse mapping
            mappedBackUserStats.Should().NotBeNull();
            mappedBackUserStats.UserStatsId.Should().Be(userStatsDto.UserStatsId);
            mappedBackUserStats.TotalBooksRead.Should().Be(userStatsDto.TotalBooksRead);
            mappedBackUserStats.TotalPagesRead.Should().Be(userStatsDto.TotalPagesRead);
        }

        [Fact]
        public void Should_Map_UserBookProgress_To_UserBookProgressDto_And_Back()
        {
            // Arrange
            var userBookProgress = new UserBookProgress
            {
                UserBookProgressId = 1,
                UserId = 1,
                BookId = 1,
                CurrentPage = 50,
                StartDate = DateTime.UtcNow.AddDays(-10),
                LastUpdatedTime = DateTime.UtcNow
            };

            // Act
            var userBookProgressDto = _mapper.Map<UserBookProgressDto>(userBookProgress);
            var mappedBackUserBookProgress = _mapper.Map<UserBookProgress>(userBookProgressDto);

            // Assert - Forward mapping
            userBookProgressDto.Should().NotBeNull();
            userBookProgressDto.UserBookProgressId.Should().Be(userBookProgress.UserBookProgressId);
            userBookProgressDto.UserId.Should().Be(userBookProgress.UserId);
            userBookProgressDto.BookId.Should().Be(userBookProgress.BookId);
            userBookProgressDto.CurrentPage.Should().Be(userBookProgress.CurrentPage);
            userBookProgressDto.StartDate.Should().Be(userBookProgress.StartDate);
            userBookProgressDto.LastUpdatedTime.Should().Be(userBookProgress.LastUpdatedTime);

            // Assert - Reverse mapping
            mappedBackUserBookProgress.Should().NotBeNull();
            mappedBackUserBookProgress.UserBookProgressId.Should().Be(userBookProgressDto.UserBookProgressId);
            mappedBackUserBookProgress.UserId.Should().Be(userBookProgressDto.UserId);
            mappedBackUserBookProgress.BookId.Should().Be(userBookProgressDto.BookId);
            mappedBackUserBookProgress.CurrentPage.Should().Be(userBookProgressDto.CurrentPage);
            mappedBackUserBookProgress.StartDate.Should().Be(userBookProgressDto.StartDate);
            mappedBackUserBookProgress.LastUpdatedTime.Should().Be(userBookProgressDto.LastUpdatedTime);
        }
    }
}