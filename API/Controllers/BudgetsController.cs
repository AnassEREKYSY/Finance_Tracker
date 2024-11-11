using System.Security.Claims;
using Core.Dtos;
using Core.Entities;
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
            var budgets = await _budgetService.GetBudgetsAsync(GetUserId());
            return Ok(budgets);
        }

        [HttpGet("one/{id}")]
        public async Task<IActionResult> GetBudget(int id)
        {
            var budget = await _budgetService.GetBudgetByIdAsync(id, GetUserId());
            if (budget == null) return NotFound("Budget not found");
            return Ok(budget);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBudget([FromBody] Budget budget)
        {
            if (budget == null)
            {
                return BadRequest("Budget data is required.");
            }

            var createdBudget = await _budgetService.CreateBudgetAsync(budget,GetUserId());
            if(createdBudget == null) return BadRequest("Error while creating");
            return Ok(createdBudget);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateBudget(int id, [FromBody] BudgetUpdatedDto budget)
        {
            if (budget == null)
            {
                return BadRequest("Budget data is required.");
            }

            var updatedBudget = await _budgetService.UpdateBudgetAsync(id, budget,GetUserId());
            if (updatedBudget == null)return BadRequest("Failed to update Budget");
            return Ok(updatedBudget);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteBudget(int id)
        {
            var result = await _budgetService.DeleteBudgetAsync(id, GetUserId());
            if(!result) return BadRequest("Error While deleting the budget");
            return Ok();
        }

        private string GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("User is not authenticated.");
            return userId;
        }
    }
}
