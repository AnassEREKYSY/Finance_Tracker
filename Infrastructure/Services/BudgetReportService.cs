using Core.Dtos;
using Core.IServices;
using System.Text;
using DinkToPdf;
using Core.Response;

namespace Infrastructure.Services
{
    public class BudgetReportService(IBudgetService _budgetService, ITransactionService _transactionService) : IBudgetReportService
    {

        public async Task<ServiceResponse<byte[]>> GenerateBudgetReportAsync(IEnumerable<int> budgetIds, string userId)
        {
            var selectedBudgets = new List<CreateUpdateBudget>();
            var response = new ServiceResponse<byte[]>();
            foreach (var budgetId in budgetIds)
            {
                var budgetResponse = await _budgetService.GetBudgetByIdAsync(budgetId, userId);
                if (budgetResponse.Success)
                    selectedBudgets.Add(budgetResponse.Data!);
            }

            if (selectedBudgets.Count == 0)
            {
                response.Success=false;
                response.Message="No valid budgets found for the given IDs.";
                response.Data=null;
                return response;
            }

            var htmlBuilder = new StringBuilder();
            htmlBuilder.Append("<html><body><h1>Budget Report</h1>");

            foreach (var budget in selectedBudgets)
            {
                var transactionsResponse = await _transactionService.GetTransactionsIntervalTimeAsync(
                    budget.StartDate, budget.EndDate, budget.CategoryName, userId);

                htmlBuilder.Append($@"
                    <h2>Budget: {budget.CategoryName} ({budget.Amount:C})</h2>
                    <p>Date: {budget.StartDate:yyyy-MM-dd} to {budget.EndDate:yyyy-MM-dd}</p>
                    <p>Created by: {budget.UserFirstName} {budget.UserLastName}</p>
                ");

                htmlBuilder.Append("<h3>Transactions:</h3>");
                htmlBuilder.Append("<table border='1'><tr><th>Amount</th><th>Date</th><th>Description</th></tr>");

                decimal totalSpent = 0;
                if (transactionsResponse.Success && transactionsResponse.Data != null)
                {
                    foreach (var transaction in transactionsResponse.Data)
                    {
                        totalSpent += transaction.Amount;
                        htmlBuilder.Append($@"
                            <tr>
                                <td>{transaction.Amount:C}</td>
                                <td>{transaction.Date:yyyy-MM-dd}</td>
                                <td>{transaction.Description}</td>
                            </tr>
                        ");
                    }
                }

                htmlBuilder.Append("</table>");
                htmlBuilder.Append($"<p>Total Spent: {totalSpent:C}</p>");
                htmlBuilder.Append($"<p>Remaining: {(budget.Amount - totalSpent):C}</p>");
            }

            htmlBuilder.Append("</body></html>");

            var pdfConverter = new SynchronizedConverter(new PdfTools());
            var document = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                }
            };

            document.Objects.Add(new ObjectSettings
            {
                HtmlContent = htmlBuilder.ToString(),
            });
            response.Success=true;
            response.Data=pdfConverter.Convert(document);
            return response;
        }
    }
}
