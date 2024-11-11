using System.Security.Claims;
using Core.Entities;
using Core.IServices;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController(ITransactionService _transactionService) : ControllerBase
    {

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTransactionsByUser(string userId)
        {
            var transactions = await _transactionService.GetTransactionsByUserIdAsync(userId);
            return Ok(transactions);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetTransactionsByCategory(int categoryId)
        {
            var transactions = await _transactionService.GetTransactionsByCategoryIdAsync(categoryId,GetUserId());
            return Ok(transactions);
        }

        [HttpGet("one/{id}")]
        public async Task<IActionResult> GetTransaction(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id,GetUserId());
            if (transaction == null) return NotFound("Transaction not found");
            return Ok(transaction);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTransaction([FromBody] Transaction transaction)
        {
            if (transaction == null)
            {
                return BadRequest("Transaction data is required.");
            }

            var createdTransaction = await _transactionService.CreateTransactionAsync(transaction,GetUserId());
            if(createdTransaction == null) return BadRequest("Error while creating");
            return Ok(createdTransaction);        
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] Transaction transaction)
        {
            if (transaction == null)
            {
                return BadRequest("Transaction data is required.");
            }

            var updatedTransaction = await _transactionService.UpdateTransactionAsync(id, transaction,GetUserId());
            if (updatedTransaction == null)return BadRequest("Failed to update Transaction");
            return Ok(updatedTransaction);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {

            var result = await _transactionService.DeleteTransactionAsync(id,GetUserId());
            if(!result) return BadRequest("Error While deleting the transaction");
            return Ok();
        }

        private string GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("User is not authenticated.");
            return userId;
        }
    }
}
