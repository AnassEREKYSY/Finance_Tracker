

namespace Core.Dtos;

public class CreateUpdateBudget
{
    public int BudgetId { get; set; }
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; } = DateTime.UtcNow;
    public string CategoryName { get; set; } = string.Empty;
    public string? UserFirstName { get; set; } = string.Empty;
    public string? UserLastName { get; set; } = string.Empty;
}
