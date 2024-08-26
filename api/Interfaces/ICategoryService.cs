using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Models;

namespace api.Interfaces
{
    public interface ICategoryService
    {
        void AddCategory(CategoryDto categoryDto);
        IEnumerable<CategoryDto> GetCategories();
        CategoryDto GetCategoryById(int id);
        void UpdateCategory(CategoryDto categoryDto);
        bool DeleteCategory(int id);

    }
}