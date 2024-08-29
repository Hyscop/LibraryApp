using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.DTOs
{
    public class UserBookProgressDto
    {
        public int UserBookProgressId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int CurrentPage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime LastUpdatedTime { get; set; }
    }

    public class StartFinishReadingDto
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
    }

    public class UpdateReadingDto
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int CurrentPage { get; set; }

    }
}