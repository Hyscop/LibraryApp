using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Interfaces;
using api.Models;
using AutoMapper;

namespace api.Mapper
{
    public class CategoryResolver : IValueResolver<BookDto, Book, Category>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryResolver(ICategoryRepository categoryService)
        {
            _categoryRepository = categoryService;
        }
        public Category Resolve(BookDto source, Book destination, Category destMember, ResolutionContext context)
        {
            if (source.CategoryId == 0) // Assuming 0 means no valid category
            {
                return null;
            }

            // Fetch the existing category from the repository using the ID
            var category = _categoryRepository.GetCategoryById(source.CategoryId);

            if (category != null)
            {
                return category; // Return the existing category
            }

            // If the category doesn't exist, create a new one
            return new Category
            {
                CategoryId = source.CategoryId,
                CategoryName = source.CategoryName
            };
        }
    }
}