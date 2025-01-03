using Core.Dtos;
using Core.Entities;
using Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService _categoryService) : ControllerBase
    {
        [Authorize]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _categoryService.GetCategoriesAsync();
            if (!result.Success)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }

        [Authorize]
        [HttpGet("one/{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id);
            if (!result.Success)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateUpdateCategory category)
        {
            var result = await _categoryService.CreateCategoryAsync(category);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);      
        }

        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CreateUpdateCategory category)
        {
            var result = await _categoryService.UpdateCategoryAsync(id, category);
            if (!result.Success)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var response = await _categoryService.DeleteCategoryAsync(id);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
