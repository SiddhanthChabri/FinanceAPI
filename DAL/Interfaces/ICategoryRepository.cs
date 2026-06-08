using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ICategoryRepository
    {
        Task AddCategory(Category data);
        Task UpdateCategory(Category data);
        Task DeleteCategory(int id);
        Task<List<Category>> GetAllCategories();
        Task<Category> GetCategoryById(int id);
    }
}
