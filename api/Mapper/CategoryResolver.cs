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
        private readonly ICategoryService _categoryService;

        public CategoryResolver(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public Category Resolve(BookDto source, Book destination, Category destMember, ResolutionContext context)
        {
            return _categoryService.GetCategoryById(source.CategoryId);
        }
    }
}