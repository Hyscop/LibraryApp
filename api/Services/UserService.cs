using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Interfaces;
using api.Models;
using AutoMapper;

namespace api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService, IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
            _mapper = mapper;
        }

        public string Authenticate(string username, string password)
        {
            var user = _userRepository.GetByUsername(username);
            if (user == null || !_passwordHasher.VerifyPassword(user.PasswordHash, password))
            {
                return null;
            }

            return _jwtTokenService.GenerateToken(user);
        }

        public UserDto GetUserWithStats(int userId)
        {
            var user = _userRepository.GetUserWithStats(userId);
            return _mapper.Map<UserDto>(user);
        }
        public void AddUser(UserForCreationDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.PasswordHash = _passwordHasher.HashPassword(userDto.Password);
            _userRepository.AddUser(user);
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

        public UserDto GetByUserId(int id)
        {

            var user = _userRepository.GetByUserId(id);
            return _mapper.Map<UserDto>(user);
        }

        public UserDto GetByUsername(string username)
        {
            var user = _userRepository.GetByUsername(username);
            return _mapper.Map<UserDto>(user);
        }

        public void UpdateUser(UserForUpdateDto userDto)
        {
            var existingUser = _userRepository.GetByUserId(userDto.UserId);
            if (existingUser == null)
            {
                throw new ArgumentNullException(nameof(existingUser), "No User Found");
            }

            _mapper.Map(userDto, existingUser);

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                existingUser.PasswordHash = _passwordHasher.HashPassword(userDto.Password);
            }

            _userRepository.UpdateUser(existingUser);
        }
    }
}