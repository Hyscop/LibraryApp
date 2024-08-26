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
using AutoMapper;
using api.DTOs;


public class BookServiceTest
{

    private readonly Mock<IBookRepository> _mockBookRepository;
    private readonly BookService _bookService;
    private readonly Mock<IMapper> _mockMapper;

    public BookServiceTest()
    {
        _mockBookRepository = new Mock<IBookRepository>();
        _mockMapper = new Mock<IMapper>();
        _bookService = new BookService(_mockBookRepository.Object, _mockMapper.Object);
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

        var bookDtos = new List<BookDto>
    {
        new BookDto{
            Id = 1,
            Title = "Test Book 1",
            Author = "Author 1",
            TotalPages = 100
        },
        new BookDto{
            Id = 2,
            Title = "Test Book 2",
            Author = "Author 2",
            TotalPages = 200
        }
    };

        _mockBookRepository.Setup(repo => repo.GetBooks()).Returns(books);
        _mockMapper.Setup(m => m.Map<List<BookDto>>(books)).Returns(bookDtos);

        //Act

        var result = _bookService.GetBooks();


        //Assert

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(bookDtos);
    }

    [Fact]
    public void BookService_AddBook_ShouldAddBookToRepository()
    {
        //Arrange

        var bookDto = new BookDto
        {
            Author = "Test Author",
            Title = "Test Book",
            TotalPages = 100
        };


        var book = new Book { Author = "Test Author", Title = "Test Book", TotalPages = 100 };
        _mockMapper.Setup(m => m.Map<Book>(bookDto)).Returns(book);

        //Act

        _bookService.AddBook(bookDto);


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
        // Arrange
        var bookId = 1;
        var existingBook = new Book
        {
            Id = bookId,
            Author = "First Author",
            Title = "First Title",
            TotalPages = 50
        };

        var updatedBookDto = new BookDto
        {
            Id = bookId,
            Author = "Updated Author",
            Title = "Updated Title",
            TotalPages = 100
        };

        _mockBookRepository.Setup(repo => repo.GetBookById(bookId)).Returns(existingBook);
        _mockMapper.Setup(m => m.Map(updatedBookDto, existingBook)).Verifiable();

        // Act
        _bookService.UpdateBook(bookId, updatedBookDto);

        // Assert
        _mockMapper.Verify(m => m.Map(updatedBookDto, existingBook), Times.Once);
        _mockBookRepository.Verify(repo => repo.UpdateBook(existingBook), Times.Once);

        existingBook.Title.Should().Be("Updated Title");
        existingBook.Author.Should().Be("Updated Author");
        existingBook.Id.Should().Be(bookId);
        existingBook.TotalPages.Should().Be(100);
        existingBook.TotalPages.Should().NotBe(50);
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
