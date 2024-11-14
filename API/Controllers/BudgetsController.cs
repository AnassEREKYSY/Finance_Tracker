using System.Security.Claims;
using Core.Dtos;
using Core.IServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetsController(IBudgetService _budgetService) : ControllerBase
    {

        [HttpGet("getAll")]
        public async Task<IActionResult> GetBudgets()
        {
            var result = await _budgetService.GetBudgetsAsync(GetUserId());
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpGet("one/{id}")]
        public async Task<IActionResult> GetBudget(int id)
        {
            var result = await _budgetService.GetBudgetByIdAsync(id, GetUserId());
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBudget([FromBody] CreateUpdateBudget budget)
        {
            var result = await _budgetService.CreateBudgetAsync(budget,GetUserId());
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateBudget(int id, [FromBody] CreateUpdateBudget budget)
        {
            var result = await _budgetService.UpdateBudgetAsync(id, budget,GetUserId());
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBudget(int id)
        {
            var response = await _budgetService.DeleteBudgetAsync(id, GetUserId());

            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        private string GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("User is not authenticated.");
            return userId;
        }
    }
}
