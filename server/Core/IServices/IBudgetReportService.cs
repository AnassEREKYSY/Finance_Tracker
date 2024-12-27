using Core.Dtos;
using Core.Response;

namespace Core.IServices;

public interface IBudgetReportService
{
    Task<ServiceResponse<byte[]>> GenerateBudgetReportAsync(IEnumerable<int> budgetIds, string userId);
}