using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;

namespace api.Repositories
{
    public class UserStatsRepository : IUserStatsRepository
    {
        private readonly LibraryAppContext _context;

        public UserStatsRepository(LibraryAppContext context)
        {
            _context = context;
        }

        public UserStats GetUserStatsByUserId(int userId)
        {
            return _context.UserStats.SingleOrDefault(us => us.UserId == userId);
        }

        public void UpdateUserStats(UserStats userStats)
        {
            _context.UserStats.Update(userStats);
            _context.SaveChanges();
        }
    }
}