using Microsoft.AspNetCore.Mvc;
using api.Interfaces;
using api.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using api.Models;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;

        public AuthController(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService, IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] UserForCreationDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<User>(userDto);
            user.PasswordHash = _passwordHasher.HashPassword(userDto.Password);
            _userRepository.AddUser(user);

            var userToReturn = _mapper.Map<UserDto>(user);
            return CreatedAtAction(nameof(UserController.GetUserStats), "User", new { id = userToReturn.UserId }, userToReturn);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] UserLoginDto loginDto)
        {
            var user = _userRepository.GetByUsername(loginDto.Username);
            if (user == null || !_passwordHasher.VerifyPassword(user.PasswordHash, loginDto.Password))
            {
                return Unauthorized();
            }

            var token = _jwtTokenService.GenerateToken(user);

            return Ok(new { Token = token });
        }
    }
}
