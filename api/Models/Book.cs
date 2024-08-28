using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int TotalPages { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<UserBookProgress> UserBookProgresses { get; set; }
    }
}