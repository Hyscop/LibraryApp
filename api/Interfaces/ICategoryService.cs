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
        void AddCategory(CategoryForCreationDto categoryDto);
        IEnumerable<CategoryForUpdateDto> GetCategories();
        CategoryForUpdateDto GetCategoryById(int id);
        void UpdateCategory(CategoryForUpdateDto categoryDto);
        bool DeleteCategory(int id);

    }
}