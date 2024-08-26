using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Models;

namespace api.Interfaces
{
    public interface IUserRepository
    {
        public User GetUserWithStats(int userId);
        void AddUser(User userDto);
        void UpdateUser(User userDto);
        User GetByUserId(int id);
        User GetByUsername(String username);

        void DeleteUser(int id);
    }
}