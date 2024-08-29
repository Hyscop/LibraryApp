using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LibraryAppContext _context;

        public UserRepository(LibraryAppContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public User GetByUserId(int id)
        {
            return _context.Users
                .Include(u => u.UserBookProgresses)
                    .ThenInclude(ubp => ubp.Book)  // Include related books
                .SingleOrDefault(u => u.UserId == id);
        }

        public User GetByUsername(string username)
        {
            return _context.Users.SingleOrDefault(u => u.Username == username);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public User GetUserWithStats(int userId)
        {
            return _context.Users.Include(u => u.UserStats).SingleOrDefault(u => u.UserId == userId);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users
                           .Include(u => u.UserStats)
                           .Include(u => u.UserBookProgresses)
                           .ToList();
        }
    }

}