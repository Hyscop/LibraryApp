using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Models;

namespace api.Interfaces
{
    public interface IBookService
    {
        void AddBook(BookDto bookDto);
        BookDto GetBookById(int id);
        void UpdateBook(int id, BookDto bookDto);
        bool DeleteBook(int id);
        IEnumerable<BookDto> GetBooks();
    }
}
