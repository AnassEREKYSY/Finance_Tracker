using Core.Entities;
using Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService _categoryService) : ControllerBase
    {

        [HttpGet("getAll")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("one/{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if(category == null) return NotFound("Category not found");
            return Ok(category);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("Category data is required.");
            }

            var createdCategory = await _categoryService.CreateCategoryAsync(category);
            if(createdCategory == null) return BadRequest("Error while creating");
            return Ok(createdCategory);        
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("Category data is required.");
            }

            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, category);
            if (updatedCategory == null)return BadRequest("Failed to update Category");
            return Ok(updatedCategory);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if(!result) return BadRequest("Error While deleting the category");
            return Ok();
        }
    }
}
