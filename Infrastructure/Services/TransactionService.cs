using System;
using Core.Dtos;
using Core.Entities;
using Core.IServices;
using Core.Response;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class TransactionService(StoreContext _context, ICategoryService categoryService, IUserService userService) : ITransactionService
{
    public async Task<ServiceResponse<CreateUpdateTransaction>> CreateTransactionAsync(CreateUpdateTransaction transaction, string userId)
    {
        if (transaction == null || userId == null)
        {
            return GetReponse(false,"Invalid Transaction data or user ID.");
        }
        var user = await userService.GetUserByIdAsync(userId);
        if (user == null)
        {
           return GetReponse(false,"User not Found.");
        }
        var categoryTransaction = await categoryService.GetCategoryByNameAsync(transaction.CategoryName);
        if (categoryTransaction == null)
        {
            return GetReponse(false,"Category Not Found.");
        }
        Console.WriteLine($"Transaction date before saving: {transaction.Date}");
        var createdTransaction = new Transaction
        {
            UserId = userId,
            User=user,
            Amount= transaction.Amount,
            Date=  DateTime.Parse(transaction.Date.ToString()),
            Description=transaction.Description,
            Type=transaction.Type,
            Category=categoryTransaction,
            CategoryId=categoryTransaction.CategoryId
        };
        Console.WriteLine($"Transaction object being saved to DB: {createdTransaction.Date}");
        _context.Transactions.Add(createdTransaction);
        await _context.SaveChangesAsync();
        transaction.UserFirstName=user!.FirstName;
        transaction.UserLastName=user!.LastName;
        transaction.TransactionId=createdTransaction.TransactionId;
        transaction.Date=createdTransaction.Date;
        transaction.CategoryName= categoryTransaction.Name;
        return GetReponse(true,"Budget Created Successfully",transaction);
    }

    public async Task<ServiceResponse<CreateUpdateTransaction>>  GetTransactionByIdAsync(int id, string userId)
    {
        var transaction = await _context.Transactions
            .Where(t => t.TransactionId == id && t.UserId == userId)
            .FirstOrDefaultAsync();
            
        if (transaction != null)
        {
            var searchedTransaction = new CreateUpdateTransaction
            {
                TransactionId = transaction.TransactionId,
                Amount = transaction.Amount,
                Description = transaction.Description,
                Date = transaction.Date,
                Type = transaction.Type,
                CategoryName = transaction.Category?.Name!,
                UserFirstName = transaction.User?.FirstName,
                UserLastName = transaction.User?.LastName   
            };
            
            return GetReponse(true,"",searchedTransaction);
        }
        
        return GetReponse(false,$"Transaction with id {id} not found or does not belong to the user.");
    }

    public async Task<ServiceResponse<IEnumerable<CreateUpdateTransaction>>> GetTransactionsByUserIdAsync(string userId)
    {
        var response = new ServiceResponse<IEnumerable<CreateUpdateTransaction>>();
        if (string.IsNullOrEmpty(userId))
        {
            response.Success = false;
            response.Message = "User ID cannot be null or empty.";
            return response;
        }
        var searchedTransactions = await _context.Transactions
            .Where(t => t.UserId == userId)
            .Include(t => t.Category)
            .ToListAsync();
            
        if (searchedTransactions == null || searchedTransactions.Count == 0)
        {
            response.Success = false;
            response.Message = $"No transactions found for the user with ID {userId}.";
            return response;
        }

        response.Data = searchedTransactions.Select(t => new CreateUpdateTransaction
        {
            TransactionId = t.TransactionId,
            Amount = t.Amount,
            Description = t.Description,
            Date = t.Date,
            Type = t.Type,
            CategoryName = t.Category?.Name ?? "Unknown",
            UserFirstName = t.User?.FirstName, 
            UserLastName = t.User?.LastName 
        }).ToList();
        response.Success=true;
        return response;
    }
    public async Task<ServiceResponse<IEnumerable<CreateUpdateTransaction>>> GetTransactionsByCategoryNameAsync(string categoryName, string userId)
    {
        var response = new ServiceResponse<IEnumerable<CreateUpdateTransaction>>();
        if (string.IsNullOrEmpty(userId))
        {
            response.Success = false;
            response.Message = "User ID cannot be null or empty.";
            return response;
        }

        if (string.IsNullOrEmpty(categoryName))
        {
            response.Success = false;
            response.Message = "Category Name is missing.";
            return response;
        }

        var searchedTransactions = await _context.Transactions
            .AsNoTracking()
            .Where(t => t.Category != null && t.Category.Name == categoryName && t.UserId == userId)
            .ToListAsync();

        if (searchedTransactions == null || searchedTransactions.Count == 0)
        {
            response.Success = false;
            response.Message = $"No transactions found for the category : {categoryName}.";
            return response;        
        }

        response.Data = searchedTransactions.Select(
            t => new CreateUpdateTransaction
            {
                TransactionId = t.TransactionId,
                Amount = t.Amount,
                Description = t.Description,
                Date = t.Date,
                Type = t.Type,
                CategoryName = t?.Category?.Name!,
                UserFirstName = t?.User?.FirstName,
                UserLastName = t?.User?.LastName   
            }
        ).ToList();

        response.Success=true;
        return response;
    }

    public async Task<ServiceResponse<bool>> DeleteTransactionAsync(int id, string userId)
    {
        var response = new ServiceResponse<bool>();
        var transaction = await _context.Transactions.FindAsync(id);

        if (transaction == null)
        {
            response.Success = false;
            response.Message = $"Transaction with id {id} not found.";
            return response;
        }

        if (transaction.UserId != userId)
        {
            response.Success = false;
            response.Message = "You are not authorized to delete this budget.";
            return response;
        }

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();
        response.Data=true;
        response.Success = true;
        return response;
    }

    private ServiceResponse<CreateUpdateTransaction> GetReponse(bool Success, string? message=null,CreateUpdateTransaction? Data=null)
    {
        var response = new ServiceResponse<CreateUpdateTransaction>
        {
            Data= Data,
            Success = Success,
            Message = message!
        };
        return response;
    }

    public async Task<ServiceResponse<IEnumerable<CreateUpdateTransaction>>> GetTransactionsIntervalTimeAsync(DateTime startDate, DateTime endDate, string categoryName, string userId)
    {
        var response = new ServiceResponse<IEnumerable<CreateUpdateTransaction>>();

        if (string.IsNullOrEmpty(userId))
        {
            response.Success = false;
            response.Message = "User ID cannot be null or empty.";
            return await Task.FromResult(response);
        }

        var transactions = await _context.Transactions
            .Where(t => t.UserId == userId && 
                        t.Date >= startDate && 
                        t.Date <= endDate && 
                        t.Category.Name == categoryName)
            .AsNoTracking()
            .ToListAsync();

        if (transactions == null || transactions.Count == 0)
        {
            response.Success = false;
            response.Message = $"No transactions found in the specified interval or for the given category '{categoryName}'.";
        }
        else
        {
            response.Data = transactions.Select(t => new CreateUpdateTransaction
            {
                TransactionId = t.TransactionId,
                Amount = t.Amount,
                Description = t.Description,
                Date = t.Date,
                Type = t.Type,
                CategoryName = t?.Category?.Name!,
                UserFirstName = t?.User?.FirstName,
                UserLastName = t?.User?.LastName
            }).ToList();

            response.Success = true;
            response.Message = "Transactions found successfully.";
        }

        return await Task.FromResult(response);
    }

}
