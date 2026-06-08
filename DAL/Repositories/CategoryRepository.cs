using DAL.Data;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _db;
        public CategoryRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task AddCategory(Category data)
        {
            await _db.Categories.AddAsync(data);
            await _db.SaveChangesAsync();
        }
        public async Task<List<Category>> GetAllCategories()
        {
            var res = await (from c in _db.Categories select c).AsNoTracking().ToListAsync();
            return res;
        }
        public async Task<Category> GetCategoryById(int id)
        {
            var res = await (from c in _db.Categories select c).FirstOrDefaultAsync(x => x.Id == id);
            return res;
        }
        public async Task UpdateCategory(Category data)
        {
            _db.Categories.Update(data);
            await _db.SaveChangesAsync();
        }
        public async Task DeleteCategory(int id)
        {
            var res = await(from c in _db.Categories select c).FirstOrDefaultAsync(x => x.Id == id);
            if (res != null)
            {
                _db.Categories.Remove(res);
                await _db.SaveChangesAsync();
            }
        }
    }
}
