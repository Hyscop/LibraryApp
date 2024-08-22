using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using api.Models;
using FluentAssertions;
using api.Interfaces;
using api.Services;
using api.Repositories;
using System.IO.Compression;
using System.Diagnostics;


public class BookServiceTest
{

    private readonly Mock<IBookRepository> _mockBookRepository;
    private readonly BookService _bookService;

    public BookServiceTest()
    {
        _mockBookRepository = new Mock<IBookRepository>();
        _bookService = new BookService(_mockBookRepository.Object);
    }

    [Fact]
    public void BookService_GetBooks_ShouldReturnAllBooks()
    {
        //Arrange

        var books = new List<Book>
        {
            new Book{
            Id = 1,
            Title = "Test Book 1",
            Author = "Author 1",
            TotalPages = 100
            },
            new Book{
                Id = 2,
                Title = "Test Book 2",
                Author = "Author 2",
                TotalPages = 200
            }
        };

        _mockBookRepository.Setup(repo => repo.GetBooks()).Returns(books);

        //Act

        var result = _bookService.GetBooks();


        //Assert

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(books);
    }

    [Fact]
    public void BookService_AddBook_ShouldAddBookToRepository()
    {
        //Arrange

        var book = new Book { Author = "Test Author", Title = "Test Book", TotalPages = 100 };

        //Act

        _bookService.AddBook(book);


        //Assert
        _mockBookRepository.Verify(repo => repo.AddBook(It.Is<Book>(b => b.Title == "Test Book")), Times.Once);
    }

    [Fact]
    public void BookService_GetBookById_ShouldReturnBook_WhenBookExists()
    {
        //Arrange

        var book = new Book
        {
            Id = 1,
            Author = "Test Author",
            Title = "Test Title",
            TotalPages = 100
        };

        _mockBookRepository.Setup(repo => repo.GetBookById(1)).Returns(book);

        //Act
        var result = _bookService.GetBookById(1);

        //Assert

        result.Should().NotBeNull();
        result.Should().BeOfType<Book>();
        result.Id.Should().Be(1);
        result.Title.Should().Be("Test Title");
        result.TotalPages.Should().Be(100);
        result.Author.Should().Be("Test Author");


    }

    [Fact]
    public void BookService_UpdateBook_ShouldUpdateBookDetails()
    {

        //Arrange

        var book = new Book
        {
            Id = 1,
            Author = "First Author",
            Title = "First Title",
            TotalPages = 50

        };
        var updatedBook = book;
        updatedBook.Author = "Updated Author";
        updatedBook.Title = "Updated Title";
        updatedBook.TotalPages = 100;

        _mockBookRepository.Setup(repo => repo.GetBookById(1)).Returns(book);

        //Act

        _bookService.UpdateBook(updatedBook);

        //Assert

        _mockBookRepository.Verify(repo => repo.UpdateBook(It.Is<Book>(b => b.Title == "Updated Title" && b.Author == "Updated Author" && b.Id == 1 && b.TotalPages == 100)));

        book.Title.Should().Be("Updated Title");
        book.Author.Should().Be("Updated Author");
        book.Id.Should().Be(1);
        book.TotalPages.Should().Be(100);
        book.TotalPages.Should().NotBe(50);


    }

    [Fact]
    public void BookService_DeleteBook_ShouldReturnFalse_WhenBookDoesNotExist()
    {
        // Arrange
        int nonExistentBookId = 99999;
        _mockBookRepository.Setup(repo => repo.GetBookById(nonExistentBookId)).Returns((Book)null);

        // Act
        var result = _bookService.DeleteBook(nonExistentBookId);

        // Assert
        result.Should().BeFalse();
        _mockBookRepository.Verify(repo => repo.DeleteBook(It.IsAny<int>()), Times.Never);
    }


    [Fact]
    public void BookService_DeleteBook_ShouldReturnTrue_WhenBookExists()
    {
        //Arrange
        int existingId = 1;
        var book = new Book
        {
            Id = existingId,
            Title = "Test Book",
            Author = "Test Author",
            TotalPages = 100
        };
        _mockBookRepository.Setup(repo => repo.GetBookById(existingId)).Returns(book);

        _mockBookRepository.Setup(repo => repo.DeleteBook(existingId)).Verifiable();

        //Act

        var result = _bookService.DeleteBook(existingId);


        //Assert

        result.Should().BeTrue();
        _mockBookRepository.Verify(repo => repo.DeleteBook(existingId), Times.Once);


    }


}
