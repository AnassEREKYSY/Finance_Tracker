using System.Security.Claims;
using Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportController(IBudgetReportService _pdfReportService) : ControllerBase
{

        [HttpPost("generate-budget-report")]
        public async Task<IActionResult> GenerateBudgetReport([FromBody] List<int> budgetIds)
        {
            try
            {
                var response = await _pdfReportService.GenerateBudgetReportAsync(budgetIds, GetUserId());

                if (!response.Success)
                {
                    return BadRequest(response.Message); 
                }

                return File(response.Data, "application/pdf", "BudgetReport.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }

     private string GetUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("User is not authenticated.");
        return userId;
    }
}
