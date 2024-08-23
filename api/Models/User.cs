using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }

        public UserStats UserStats { get; set; }
        public ICollection<UserBookProgress> UserBookProgresses { get; set; }
        public ICollection<Book> FinishedBooks { get; set; }
    }

    public enum UserRole
    {
        Admin,
        RegularUser
    }
}