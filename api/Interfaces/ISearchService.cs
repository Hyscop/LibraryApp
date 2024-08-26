using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface ISearchService
    {
        IEnumerable<Book> SearchBooks(string query, string category = null);
    }
}