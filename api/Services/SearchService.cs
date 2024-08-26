using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;

namespace api.Services
{
    public class SearchService : ISearchService
    {
        private readonly IBookRepository _bookRepository;

        public SearchService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public IEnumerable<Book> SearchBooks(string query, string category = null)
        {
            var books = _bookRepository.GetBooks();

            if (!string.IsNullOrEmpty(category))
            {
                books = books.Where(b => b.Category.CategoryName.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(query))
            {
                books = books.Where(b => b.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                                         b.Author.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return books;
        }
    }
}