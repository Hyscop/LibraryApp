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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet("GetCategories")]
        //[Authorize(Policy = "RegularUserOnly")]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.GetCategories();
            var categoryDtos = _mapper.Map<IEnumerable<CategoryForUpdateDto>>(categories);
            return Ok(categoryDtos);
        }

        [HttpGet("GetById/{id}")]
        //[Authorize(Policy = "RegularUserOnly")]
        public IActionResult GetCategoryById(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }

            var categoryDto = _mapper.Map<CategoryForUpdateDto>(category);
            return Ok(categoryDto);
        }

        [HttpPost("Create")]
        //[Authorize(Policy = "AdminOnly")]
        public IActionResult CreateCategory([FromBody] CategoryForCreationDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = _mapper.Map<Category>(categoryDto);
            _categoryRepository.AddCategory(category);

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.CategoryId }, category);
        }

        [HttpPut("/Update{id}")]
        //[Authorize(Policy = "AdminOnly")]
        public IActionResult UpdateCategory(int id, [FromBody] CategoryForUpdateDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCategory = _categoryRepository.GetCategoryById(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            categoryDto.CategoryId = id;

            _mapper.Map(categoryDto, existingCategory);
            _categoryRepository.UpdateCategory(existingCategory);

            return NoContent();
        }

        [HttpDelete("Delete/{id}")]
        //[Authorize(Policy = "AdminOnly")]
        public IActionResult DeleteCategory(int id)
        {
            var existingCategory = _categoryRepository.GetCategoryById(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            _categoryRepository.DeleteCategory(id);
            return NoContent();
        }
    }
}
