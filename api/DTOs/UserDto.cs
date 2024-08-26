using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public UserStatsDto UserStats { get; set; }
        public ICollection<UserBookProgressDto> UserBookProgresses { get; set; }
        public ICollection<BookDto> FinishedBooks { get; set; }
    }
}