using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IBookService
    {
        void AddBook(Book book);
        Book GetBookById(int id);
        void UpdateBook(Book book);
        bool DeleteBook(int id);
        IEnumerable<Book> GetBooks();
    }
}
