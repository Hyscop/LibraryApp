using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface ICategoryService
    {
        void AddCategory(Category category);
        IEnumerable<Category> GetCategories();
        Category GetCategoryById(int id);
        void UpdateCategory(Category category);
        bool DeleteCategory(int id);

    }
}