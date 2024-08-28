using Microsoft.AspNetCore.Mvc;
using api.Interfaces;
using api.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserBookProgressRepository _userBookProgresses;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IPasswordHasher passwordHasher, IMapper mapper, IUserBookProgressRepository userBookProgressRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _userBookProgresses = userBookProgressRepository;
        }

        [HttpGet("GetUserWithStats/{id}")]
        //[Authorize(Policy = "AdminOnly")]
        public IActionResult GetUserWithStats(int id)
        {
            var user = _userRepository.GetUserWithStats(id);
            if (user == null)
            {
                return NotFound();
            }

            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }


        [HttpPut("UpdateUser/{id}")]
        //[Authorize(Policy = "AdminOnly")]
        public IActionResult UpdateUser(int id, [FromBody] UserForUpdateDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = _userRepository.GetByUserId(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            userDto.UserId = id;
            _mapper.Map(userDto, existingUser);

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                existingUser.PasswordHash = _passwordHasher.HashPassword(userDto.Password);
            }

            _userRepository.UpdateUser(existingUser);

            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        //[Authorize(Policy = "AdminOnly")]
        public IActionResult DeleteUser(int id)
        {
            var existingUser = _userRepository.GetByUserId(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            _userRepository.DeleteUser(id);
            return NoContent();
        }
    }
}
