using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(FinanceAPI.DTOs.CategoryDto data)
        {
            var res = new DAL.Models.Category
            {
                Name = data.Name,
                Type = data.Type,
            };
            await _categoryRepository.AddCategory(res);
            return Ok("Category Created Successfully");
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _categoryRepository.GetAllCategories();
            return Ok(res);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _categoryRepository.GetCategoryById(id);
            if (res == null)
                return NotFound("Category Not Found");
            return Ok(res);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, FinanceAPI.DTOs.CategoryDto data)
        {
            var res = await _categoryRepository.GetCategoryById(id);
            if (res == null)
                return NotFound("Category Not Found");

            res.Name = data.Name;
            res.Type = data.Type;

            await _categoryRepository.UpdateCategory(res);
            return Ok("Category Updated Successfully");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var res = await _categoryRepository.GetCategoryById(id);
            if (res == null)
                return NotFound("Category Not Found");

            await _categoryRepository.DeleteCategory(id);
            return Ok("Category Deleted Successfully");
        }

    }
}
