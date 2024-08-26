using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int TotalPages { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}