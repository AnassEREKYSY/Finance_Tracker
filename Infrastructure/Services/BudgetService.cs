using Core.Dtos;
using Core.Entities;
using Core.IServices;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class BudgetService(StoreContext _context, ICategoryService categoryService, IUserService userService) : IBudgetService
{
    public async Task<Budget> CreateBudgetAsync(CreateUpdateBudget budget, string userId)
    {
        if (budget != null && userId !=null)
        {
            var user=await userService.GetUserByIdAsync(userId)?? null;
            var categoryBudget= await categoryService.GetCategoryByNameAsync(budget.CategoryName);
            var createdBudget = new Budget
            {
                UserId = userId,
                Amount= budget.Amount,
                StartDate=budget.StartDate,
                EndDate=budget.EndDate,
                CategoryId=categoryBudget.CategoryId,
                Category=categoryBudget,
                User=user!
            };
            _context.Budgets.Add(createdBudget);
            await _context.SaveChangesAsync();
            budget.CategoryName=categoryBudget.Name;
            budget.UserFirstName=user!.FirstName;
            budget.UserLastName=user!.LastName;
            budget.BudgetId=createdBudget.BudgetId;
            return createdBudget;
        }

        throw new ArgumentNullException(nameof(budget));
    }

    public async Task<CreateUpdateBudget> GetBudgetByIdAsync(int id,string userId)
    {
        var budget = await _context.Budgets
        .Include(b => b.User)
        .Include(b => b.Category)
        .Where(b => b.BudgetId == id && b.UserId == userId)
        .FirstOrDefaultAsync();
        if(budget != null)
        {
            var searchedBudget= new CreateUpdateBudget
            {
                BudgetId=budget.BudgetId,
                Amount=budget.Amount,
                StartDate=budget.StartDate,
                EndDate=budget.EndDate,
                CategoryName=budget.Category.Name,
                UserFirstName=budget.User.FirstName,
                UserLastName=budget.User.LastName,
            };
            return searchedBudget;
        }

        throw new KeyNotFoundException($"Budget with id {id} not found or does not belong to the user.");
    }

    public async Task<IEnumerable<CreateUpdateBudget>> GetBudgetsAsync(string userId)
    {
        var searchedBudgets = await _context.Budgets
        .AsNoTracking()
        .Include(b => b.User)
        .Include(b => b.Category)
        .Where(b => b.UserId == userId)
        .ToListAsync();

        if (searchedBudgets == null || searchedBudgets.Count == 0)
        {
            throw new KeyNotFoundException($"No budgets found for the user with ID {userId}.");
        }
        var budgetDtos = searchedBudgets.Select(
            b => new CreateUpdateBudget
                {
                    BudgetId=b.BudgetId,
                    Amount = b.Amount,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    CategoryName = b.Category.Name,
                    UserFirstName = b.User.FirstName,
                    UserLastName = b.User.LastName,
                }
            ).ToList();

        return budgetDtos;
    }

    public async Task<CreateUpdateBudget> UpdateBudgetAsync(int id, CreateUpdateBudget budgetDto, string userId)
    {
        if (budgetDto != null)
        {
            var category = await categoryService.GetCategoryByNameAsync(budgetDto.CategoryName) ?? throw new KeyNotFoundException($"Category '{budgetDto.CategoryName}' not found.");
            var existingBudget = await _context.Budgets.FindAsync(id)
                                ?? throw new KeyNotFoundException($"Budget with id {id} not found.");

            if (existingBudget.UserId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to update this budget.");
            }
            var user = await userService.GetUserByIdAsync(userId);
            if (user != null)
            {
                existingBudget.User = user;
            }
            existingBudget.Amount = budgetDto.Amount;
            existingBudget.Category = category;
            existingBudget.StartDate = budgetDto.StartDate;
            existingBudget.EndDate = budgetDto.EndDate;
            existingBudget.CategoryId = category.CategoryId;

            await _context.SaveChangesAsync();
            budgetDto.UserFirstName = existingBudget.User.FirstName;
            budgetDto.UserLastName = existingBudget.User.LastName;
            budgetDto.BudgetId = existingBudget.BudgetId;

            return budgetDto;
        }

        throw new ArgumentNullException(nameof(budgetDto));
    }

    public async Task<bool> DeleteBudgetAsync(int id, string userId)
    {
        var budget = await _context.Budgets.FindAsync(id);
        if (budget != null)
        {
            if (budget.UserId == userId)
            {
                _context.Budgets.Remove(budget);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new UnauthorizedAccessException("You are not authorized to delete this budget.");

        }
        throw new KeyNotFoundException($"Budget with id {id} not found.");
    }
}
