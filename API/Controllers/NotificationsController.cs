using Core.Entities;
using Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController(INotificationService _notificationService) : ControllerBase
    {

        [HttpPost("create")]
        public async Task<IActionResult> CreateNotification([FromBody] Notification notification)
        {
            if (notification == null)
            {
                return BadRequest("Notification data is required.");
            }

            var createdNotification = await _notificationService.CreateNotificationAsync(notification);
            if(createdNotification == null) return BadRequest("Error while creating");
            return Ok(createdNotification);        }

        [HttpPut("asRead/{id}")]
        public async Task<IActionResult> MarkNotificationAsRead(int id)
        {
            var result = await _notificationService.MarkAsReadAsync(id);
            return result ? Ok() : NotFound();
        }
    }
}
