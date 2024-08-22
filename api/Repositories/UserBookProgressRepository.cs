using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;

namespace api.Repositories
{
    public class UserBookProgressRepository : IUserBookProgressRepository
    {
        private readonly LibraryAppContext _context;

        public UserBookProgressRepository(LibraryAppContext context)
        {
            _context = context;
        }

        public void AddProgress(UserBookProgress progress)
        {
            _context.UserBookProgresses.Add(progress);
            _context.SaveChanges();
        }

        public void DeleteProgress(UserBookProgress progress)
        {
            throw new NotImplementedException();
        }

        public UserBookProgress GetProgressByUserAndBook(int userId, int bookId)
        {
            return _context.UserBookProgresses.SingleOrDefault(ubp => ubp.UserId == userId && ubp.BookId == bookId);
        }

        public void UpdateProgress(UserBookProgress progress)
        {
            _context.UserBookProgresses.Update(progress);
            _context.SaveChanges();
        }
    }
}