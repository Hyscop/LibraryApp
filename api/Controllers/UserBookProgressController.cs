using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserBookProgressController : ControllerBase
    {

        private readonly IUserBookProgressService _userBookProgressService;

        private readonly IMapper _mapper;

        public UserBookProgressController(IUserBookProgressService userBookProgressService, IMapper mapper)
        {
            _userBookProgressService = userBookProgressService;
            _mapper = mapper;
        }
        [HttpPost("StartReading")]
        public IActionResult StartReadingBook([FromBody] StartFinishReadingDto dto)
        {
            try
            {
                _userBookProgressService.StartReadingBook(dto.UserId, dto.BookId);
                return Ok("Started reading");
            }
            catch (InvalidOperationException ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpPut("UpdateProgress")]
        public IActionResult UpdateReadingProgress([FromBody] UpdateReadingDto dto)
        {
            try
            {
                _userBookProgressService.UpdateReadingProgress(dto.UserId, dto.BookId, dto.CurrentPage);
                return Ok("Progress Updated");
            }
            catch (KeyNotFoundException ex)
            {

                return NotFound(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("FİnishedReading")]
        public IActionResult FinishReading([FromBody] StartFinishReadingDto dto)
        {
            _userBookProgressService.FinishReadingBook(dto.UserId, dto.BookId);
            return Ok("Finished reading book");
        }

        [HttpGet("GetCurrentReading/{userId}")]
        public IActionResult GetCurrentReading(int userId)
        {
            try
            {
                var readingBooks = _userBookProgressService.GetCurrentReadingBooks(userId);
                return Ok(readingBooks);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("No reading progress found for this user.");
            }
        }

    }
}