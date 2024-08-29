using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Models;

namespace api.Interfaces
{
    public interface IUserService
    {
        UserDto GetUserWithStats(int userId);
        void AddUser(UserForCreationDto userDto);
        void UpdateUser(UserForUpdateDto userDto);
        UserDto GetByUserId(int id);

        UserDto GetByUsername(string username);

        bool DeleteUser(int id);
    }
}