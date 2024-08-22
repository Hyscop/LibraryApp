using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IUserService
    {
        public User GetUserWithStats(int userId);
        void AddUser(User user);
        User AuthenticateUser(string username, string password);
        void UpdateUser(User user);
        User GetByUserId(int id);

        User GetByUsername(string username);

        bool DeleteUser(int id);
    }
}