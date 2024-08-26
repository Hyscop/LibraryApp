using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;
using api.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Web;
using Moq;
using Xunit;

namespace tests.UnitTests
{
    public class SearchServiceTest
    {
        private readonly Mock<IBookRepository> _mockBookRepository;

        private readonly ISearchService _searchService;

        public SearchServiceTest()
        {
            _mockBookRepository = new Mock<IBookRepository>();
            _searchService = new SearchService(_mockBookRepository.Object);
        }
        [Fact]
        public void SearchServiceTest_SearchBooks_ShouldReturnBooks_WhenTitleMatched()
        {
            //Arrange
            var books = new List<Book>
            {
                new Book {Title = ".Net api programming" , Author = "Hyscop" },
                new Book {Title = "Java Programming" , Author = "Mehmet"},
                new Book {Title = "Java Intro to OOP"}
            };

            _mockBookRepository.Setup(repo => repo.GetBooks()).Returns(books);


            //Act
            var result = _searchService.SearchBooks("java");
            //Assert
            result.Should().Contain(b => b.Title == "Java Programming");
            result.Should().Contain(b => b.Title == "Java Intro to OOP");
        }

        [Fact]
        public void SearchService_SearchBooks_ShouldReturmBooks_WhenAuthorMatches()
        {

            //Arrange
            var books = new List<Book>
            {
                new Book {Title = ".Net api programming" , Author = "Hyscop" },
                new Book {Title = "Java Programming" , Author = "Mehmet"},
                new Book {Title = "Java Intro to OOP", Author = "Mehmet"}
            };

            _mockBookRepository.Setup(repo => repo.GetBooks()).Returns(books);


            //Act
            var result = _searchService.SearchBooks("meh");
            //Assert
            result.Should().Contain(b => b.Title == "Java Programming");
            result.Should().Contain(b => b.Title == "Java Intro to OOP");
            result.Should().Contain(b => b.Author == "Mehmet");
        }
    }
}