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
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _mapper = mapper;
            _bookRepository = bookRepository;
        }
        public void AddBook(BookDto bookDto)
        {

            var book = _mapper.Map<Book>(bookDto);
            _bookRepository.AddBook(book);
        }

        public BookDto GetBookById(int id)
        {
            var book = _bookRepository.GetBookById(id);
            return _mapper.Map<BookDto>(book);
        }

        public void UpdateBook(int id, BookDto bookDto)
        {
            var existingBook = _bookRepository.GetBookById(id);
            if (existingBook == null)
            {
                throw new KeyNotFoundException("Book not found");
            }

            _mapper.Map(bookDto, existingBook);

            _bookRepository.UpdateBook(existingBook);
        }

        public bool DeleteBook(int id)
        {
            var existingBook = _bookRepository.GetBookById(id);
            if (existingBook == null)
            {
                return false;
            }

            _bookRepository.DeleteBook(id);
            return true;
        }

        public IEnumerable<BookDto> GetBooks()
        {
            var books = _bookRepository.GetBooks();
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }
    }
}