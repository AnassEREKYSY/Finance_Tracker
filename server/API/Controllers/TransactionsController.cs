using System.Security.Claims;
using Core.Dtos;
using Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController(ITransactionService _transactionService) : ControllerBase
    {

        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> GetTransactionsByUser()
        {
            var result = await _transactionService.GetTransactionsByUserIdAsync(GetUserId());
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [Authorize]
        [HttpGet("category/{categoryName}")]
        public async Task<IActionResult> GetTransactionsByCategory(string categoryName)
        {
            var result = await _transactionService.GetTransactionsByCategoryNameAsync(categoryName,GetUserId());
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [Authorize]
        [HttpGet("one/{id}")]
        public async Task<IActionResult> GetTransaction(int id)
        {
            var result = await _transactionService.GetTransactionByIdAsync(id,GetUserId());
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [Authorize]
        [HttpGet("transactionsInerval/{startDate}/{endDate}/{categoryName}")]
        public async Task<IActionResult> GetTransactionIntervalTime(DateTime startDate, DateTime endDate, string categoryName)
        {
            var result = await _transactionService.GetTransactionsIntervalTimeAsync(startDate, endDate, categoryName, GetUserId());
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateUpdateTransaction transaction)
        {
            var result = await _transactionService.CreateTransactionAsync(transaction,GetUserId());
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);      
        }

        // [HttpPut("update/{id}")]
        // public async Task<IActionResult> UpdateTransaction(int id, [FromBody] CreateUpdateTransaction transaction)
        // {
        //     if (transaction == null)
        //     {
        //         return BadRequest("Transaction data is required.");
        //     }

        //     var updatedTransaction = await _transactionService.UpdateTransactionAsync(id, transaction,GetUserId());
        //     if (updatedTransaction == null)return BadRequest("Failed to update Transaction");
        //     return Ok(updatedTransaction);
        // }


        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var result =await _transactionService.DeleteTransactionAsync(id,GetUserId());
            if (!result.Success)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }

        private string GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("User is not authenticated.");
            return userId;
        }
    }
}
