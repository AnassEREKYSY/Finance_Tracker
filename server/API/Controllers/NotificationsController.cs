using System.Security.Claims;
using Core.Entities;
using Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController(INotificationService _notificationService) : ControllerBase
    {
        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> GetNotificationsForUser()
        {
            var response = await _notificationService.GetNotificationsByUserIdAsync(GetUserId());
            if(!response.Success)
            {
                return BadRequest("Error while Getting the norifications");
            }
            return Ok(response.Data);        
        }

        [Authorize]
        [HttpPut("asRead/{id}")]
        public async Task<IActionResult> MarkNotificationAsRead(int id)
        {
            var result = await _notificationService.MarkAsReadAsync(id);
            return result ? Ok() : NotFound();
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var result =await _notificationService.DeleteAsync(id,GetUserId());
            if (!result.Success)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }

        private string GetUserId()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedAccessException("User ID not found in claims.");
                }

                return userId;
            }
            else
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
        }
    }
}
