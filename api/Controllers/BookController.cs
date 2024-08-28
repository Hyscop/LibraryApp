using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using api.Interfaces;
using api.DTOs;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookController(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        [HttpGet("GetBooks")]
        //  [Authorize(Policy = "RegularUserOnly")]
        public IActionResult GetBooks()
        {
            var books = _bookRepository.GetBooks();
            var bookDtos = _mapper.Map<IEnumerable<BookDto>>(books);
            return Ok(bookDtos);
        }

        [HttpGet("GetById/{id}")]
        //[Authorize(Policy = "RegularUserOnly")]
        public IActionResult GetBookById([FromRoute] int id)
        {
            var book = _bookRepository.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }

            var bookDto = _mapper.Map<BookDto>(book);
            return Ok(bookDto);
        }

        [HttpPost("Create")]
        //[Authorize(Policy = "AdminOnly")]
        public IActionResult CreateBook([FromBody] BookForCreationDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = _mapper.Map<Book>(bookDto);
            _bookRepository.AddBook(book);

            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, _mapper.Map<BookDto>(book));
        }

        [HttpPut("Update/{id}")]
        //[Authorize(Policy = "AdminOnly")]
        public IActionResult UpdateBook([FromRoute] int id, [FromBody] BookForUpdateDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingBook = _bookRepository.GetBookById(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            bookDto.Id = id;
            _mapper.Map(bookDto, existingBook);
            _bookRepository.UpdateBook(existingBook);

            return Ok(_mapper.Map<BookDto>(existingBook));
        }

        [HttpDelete("Delete/{id}")]
        //[Authorize(Policy = "AdminOnly")]
        public IActionResult DeleteBook([FromRoute] int id)
        {
            var existingBook = _bookRepository.GetBookById(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            _bookRepository.DeleteBook(id);
            return NoContent();
        }
    }
}
