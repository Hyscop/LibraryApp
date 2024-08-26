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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public void AddCategory(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            _categoryRepository.AddCategory(category);
        }

        public bool DeleteCategory(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                return false;
            }

            _categoryRepository.DeleteCategory(id);
            return true;
        }

        public IEnumerable<CategoryDto> GetCategories()
        {
            var categories = _categoryRepository.GetCategories();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public CategoryDto GetCategoryById(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            return _mapper.Map<CategoryDto>(category);
        }

        public void UpdateCategory(CategoryDto categoryDto)
        {
            var existingCategory = _categoryRepository.GetCategoryById(categoryDto.CategoryId);
            if (existingCategory == null)
            {
                throw new ArgumentNullException(nameof(existingCategory), "Category not found");
            }

            _mapper.Map(categoryDto, existingCategory);
            _categoryRepository.UpdateCategory(existingCategory);
        }
    }
}