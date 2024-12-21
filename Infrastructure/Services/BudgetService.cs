using Core.Dtos;
using Core.Entities;
using Core.IServices;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Core.Response;

namespace Infrastructure.Services;

public class BudgetService(StoreContext _context, ICategoryService categoryService, IUserService userService, ITransactionService transactionService, INotificationService notificationService) : IBudgetService
{
    public async Task<ServiceResponse<CreateUpdateBudget>> CreateBudgetAsync(CreateUpdateBudget budget, string userId)
    {
        if (budget == null || userId == null)
        {
            return GetReponse(false,"Invalid budget data or user ID.");
        }
        var user = await userService.GetUserByIdAsync(userId);
        if (user == null)
        {
           return GetReponse(false,"User not Found.");
        }
        var categoryBudget = await categoryService.GetCategoryByNameAsync(budget.CategoryName);
        if (categoryBudget == null)
        {
            return GetReponse(false,"Category Not Found.");
        }
        var createdBudget = new Budget
        {
            UserId = userId,
            Amount = budget.Amount,
            StartDate = budget.StartDate,
            EndDate = budget.EndDate,
            CategoryId = categoryBudget.CategoryId,
            Category = categoryBudget,
            User = user
        };

        _context.Budgets.Add(createdBudget);
        await _context.SaveChangesAsync();

        budget.CategoryName = categoryBudget.Name;
        budget.UserFirstName = user.FirstName;
        budget.UserLastName = user.LastName;
        budget.BudgetId = createdBudget.BudgetId;

        return GetReponse(true,"Budget Created Successfully",budget);
    }

    public async Task<ServiceResponse<CreateUpdateBudget>> GetBudgetByIdAsync(int id, string userId)
    {
        var budget = await _context.Budgets
            .Include(b => b.User)
            .Include(b => b.Category)
            .Where(b => b.BudgetId == id && b.UserId == userId)
            .FirstOrDefaultAsync();

        if (budget != null)
        {
            CreateUpdateBudget Data = new()
            {
                BudgetId = budget.BudgetId,
                Amount = budget.Amount,
                StartDate = budget.StartDate,
                EndDate = budget.EndDate,
                CategoryName = budget.Category.Name,
                UserFirstName = budget.User.FirstName,
                UserLastName = budget.User.LastName,
            };
            return GetReponse(true,"",Data);
        }

        return GetReponse(false,$"Budget with id {id} not found or does not belong to the user.");
    }
    public async Task<ServiceResponse<IEnumerable<CreateUpdateBudget>>> GetBudgetsAsync(string userId)
    {
        var response = new ServiceResponse<IEnumerable<CreateUpdateBudget>>();

        if (string.IsNullOrEmpty(userId))
        {
            response.Success = false;
            response.Message = "User ID cannot be null or empty.";
            return response;
        }

        var searchedBudgets = await _context.Budgets
            .AsNoTracking()
            .Include(b => b.User)
            .Include(b => b.Category)
            .Where(b => b.UserId == userId)
            .ToListAsync();

        if (searchedBudgets == null || searchedBudgets.Count == 0)
        {
            response.Success = false;
            response.Message = $"No budgets found for the user with ID {userId}.";
            return response;
        }

        response.Data = searchedBudgets.Select(b => new CreateUpdateBudget
        {
            BudgetId = b.BudgetId,
            Amount = b.Amount,
            StartDate = b.StartDate,
            EndDate = b.EndDate,
            CategoryName = b.Category.Name,
            UserFirstName = b.User.FirstName,
            UserLastName = b.User.LastName,
        }).ToList();
        response.Success=true;
        return response;
    }
    public async Task<ServiceResponse<CreateUpdateBudget>> UpdateBudgetAsync(int id, CreateUpdateBudget budgetDto, string userId)
    {

        if (budgetDto == null)
        {
            return GetReponse(false,"Invalid budget data.");
        }

        var category = await categoryService.GetCategoryByNameAsync(budgetDto.CategoryName);
        if (category == null)
        {
            return GetReponse(false,$"Category '{budgetDto.CategoryName}' not found.");
        }

        var existingBudget = await _context.Budgets.FindAsync(id);
        if (existingBudget == null)
        {
            return GetReponse(false,$"Budget with id {id} not found.");
        }

        if (existingBudget.UserId != userId)
        {
            return GetReponse(false,"You are not authorized to update this budget.");
        }

        var user = await userService.GetUserByIdAsync(userId);
        existingBudget.User = user!;
        existingBudget.Amount = budgetDto.Amount;
        existingBudget.Category = category;
        existingBudget.StartDate = budgetDto.StartDate;
        existingBudget.EndDate = budgetDto.EndDate;
        existingBudget.CategoryId = category.CategoryId;

        await _context.SaveChangesAsync();

        budgetDto.UserFirstName = existingBudget.User.FirstName;
        budgetDto.UserLastName = existingBudget.User.LastName;
        budgetDto.BudgetId = existingBudget.BudgetId;

        return GetReponse(true,"Badget Updated Successfully",budgetDto);
    }
    public async Task<ServiceResponse<bool>> DeleteBudgetAsync(int id, string userId)
    {
        var response = new ServiceResponse<bool>();
        var budget = await _context.Budgets.FindAsync(id);

        if (budget == null)
        {
            response.Success = false;
            response.Message = $"Budget with id {id} not found.";
            return response;
        }

        if (budget.UserId != userId)
        {
            response.Success = false;
            response.Message = "You are not authorized to delete this budget.";
            return response;
        }

        _context.Budgets.Remove(budget);
        await _context.SaveChangesAsync();
        response.Data = true;
        response.Success = true;

        return response;
    }
    public async void CheckBudgetStatus(string userId){
        var response = await GetBudgetsAsync(userId);
        decimal totalAmount = 0;

         if (response.Success)
        {
            List<CreateUpdateBudget> budgets = response.Data
                .Select(createUpdateBudget => new CreateUpdateBudget
                {
                    BudgetId = createUpdateBudget.BudgetId,
                    Amount = createUpdateBudget.Amount,
                    StartDate = createUpdateBudget.StartDate,
                    EndDate = createUpdateBudget.EndDate,
                    CategoryName = createUpdateBudget.CategoryName
                })
                .ToList();

            foreach (var budget in budgets)
            {
                var transactionResponse = await transactionService.GetTransactionsIntervalTimeAsync(
                                                                    budget.StartDate, 
                                                                    budget.EndDate, 
                                                                    budget.CategoryName, 
                                                                    userId);

                if (transactionResponse.Success && transactionResponse.Data != null)
                {
                    foreach (var transaction in transactionResponse.Data)
                    {
                        totalAmount += transaction.Amount;
                    }
                }
                decimal percentBudgetAmount = totalAmount / budget.Amount * 100;
                var notification = new CreateUpdateNotifications{
                    Message=$"You have spent {percentBudgetAmount:F2}% of your budget.",

                };
                await notificationService.CreateNotificationAsync(notification,userId);                                                  
            }
        }
     }


    private ServiceResponse<CreateUpdateBudget> GetReponse(bool Success, string? message=null,CreateUpdateBudget? Data=null)
    {
        var response = new ServiceResponse<CreateUpdateBudget>
        {
            Data= Data,
            Success = Success,
            Message = message!
        };
        return response;
    }
}
