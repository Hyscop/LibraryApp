using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class UserBookProgress
    {
        public int UserBookProgressId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int CurrentPage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime LastUpdatedTime { get; set; }

        public User User { get; set; }
        public Book Book { get; set; }


    }
}