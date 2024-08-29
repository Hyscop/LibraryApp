using System.Collections.Generic;
using System.Linq;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryAppContext _context;

        public BookRepository(LibraryAppContext context)
        {
            _context = context;
        }

        public void AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public Book GetBookById(int id)
        {
            return _context.Books
                .Include(b => b.Category)
                .FirstOrDefault(b => b.Id == id);
        }

        public void UpdateBook(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
        }

        public bool DeleteBook(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null)
            {
                return false;
            }

            _context.Books.Remove(book);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<Book> GetBooks()
        {
            return _context.Books
                .Include(b => b.Category)
                .ToList();
        }

        public IEnumerable<Book> GetBooksByCategory(int categoryId)
        {
            return _context.Books
                .Include(b => b.Category) // Eagerly load the Category
                .Where(b => b.CategoryId == categoryId)
                .ToList();
        }
    }
}
