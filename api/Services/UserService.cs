using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;

namespace api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public User GetUserWithStats(int userId)
        {
            return _userRepository.GetUserWithStats(userId);
        }
        public void AddUser(User user)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user.PasswordHash);
            _userRepository.AddUser(user);
        }

        public User AuthenticateUser(string username, string password)
        {
            var user = _userRepository.GetByUsername(username);
            if (user == null || !_passwordHasher.VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }

        public bool DeleteUser(int id)
        {
            var existingUser = _userRepository.GetByUserId(id);
            if (existingUser == null)
            {
                return false;
            }

            _userRepository.DeleteUser(id);
            return true;
        }

        public User GetByUserId(int id)
        {
            return _userRepository.GetByUserId(id);
        }

        public User GetByUsername(string username)
        {
            return _userRepository.GetByUsername(username);
        }

        public void UpdateUser(User user)
        {
            var existingUser = _userRepository.GetByUserId(user.UserId);
            if (existingUser != null)
            {
                existingUser.Username = user.Username;
                existingUser.Email = user.Email;
                existingUser.PasswordHash = user.PasswordHash;
                existingUser.Role = user.Role;
                _userRepository.UpdateUser(user);
            }


        }
    }
}