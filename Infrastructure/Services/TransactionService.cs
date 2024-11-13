using System;
using Core.Dtos;
using Core.Entities;
using Core.IServices;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class TransactionService(StoreContext _context, ICategoryService categoryService, IUserService userService) : ITransactionService
{
    public async Task<CreateUpdateTransaction> CreateTransactionAsync(CreateUpdateTransaction transaction, string userId)
    {
        if (transaction != null && userId != null)
        {
            var user=await userService.GetUserByIdAsync(userId)?? null;
            var categoryBudget= await categoryService.GetCategoryByNameAsync(transaction.CategoryName);
            var createdTransaction = new Transaction
            {
                UserId = userId,
                User=user!,
                Amount= transaction.Amount,
                Date=DateTime.Now,
                Description=transaction.Description,
                Type=transaction.Type,
                Category=categoryBudget,
                CategoryId=categoryBudget.CategoryId
            };
            _context.Transactions.Add(createdTransaction);
            await _context.SaveChangesAsync();
            transaction.UserFirstName=user!.FirstName;
            transaction.UserLastName=user!.LastName;
            transaction.TransactionId=createdTransaction.TransactionId;
            transaction.Date=createdTransaction.Date;
            return transaction;
        }

        throw new ArgumentNullException(nameof(transaction));
    }

    public async Task<CreateUpdateTransaction> GetTransactionByIdAsync(int id, string userId)
    {
        var transaction = await _context.Transactions
        .Where(t => t.TransactionId == id && t.UserId == userId)
        .FirstOrDefaultAsync();
        if(transaction !=  null)
        {
             var searchedTansaction= new CreateUpdateTransaction
            {
                TransactionId=transaction.TransactionId,
                Amount=transaction.Amount,
                Description=transaction.Description,
                Date=transaction.Date,
                Type=transaction.Type,
                CategoryName=transaction.Category.Name,
                UserFirstName=transaction.User.FirstName,
                UserLastName=transaction.User.LastName
            };
            return searchedTansaction;
        }
         throw new KeyNotFoundException($"Transaction with id {id} not found.");
    }

    public async Task<IEnumerable<CreateUpdateTransaction>> GetTransactionsByUserIdAsync(string userId)
    {
        var searchedTransactions=await _context.Transactions.Where(t => t.UserId == userId).ToListAsync();
        if(searchedTransactions == null || searchedTransactions.Count==0)
        {
            throw new KeyNotFoundException($"No transactions found for the user with ID {userId}.");
        }
        var searchedTransactionsDtos = searchedTransactions.Select(
            t => new CreateUpdateTransaction
                {
                TransactionId=t.TransactionId,
                Amount=t.Amount,
                Description=t.Description,
                Date=t.Date,
                Type=t.Type,
                CategoryName=t.Category.Name,
                UserFirstName=t.User.FirstName,
                UserLastName=t.User.LastName
                }
            ).ToList();

        return searchedTransactionsDtos;
    }

    public async Task<IEnumerable<CreateUpdateTransaction>> GetTransactionsByCategoryIdAsync(int categoryId, string userId)
    {
        var searchedTransactions=await _context.Transactions.Where(t => t.CategoryId == categoryId && t.UserId == userId).ToListAsync();
        if(searchedTransactions == null || searchedTransactions.Count==0)
        {
            throw new KeyNotFoundException($"No transactions found for the user with ID {userId}.");
        }
        var searchedTransactionsDtos = searchedTransactions.Select(
            t => new CreateUpdateTransaction
                {
                TransactionId=t.TransactionId,
                Amount=t.Amount,
                Description=t.Description,
                Date=t.Date,
                Type=t.Type,
                CategoryName=t.Category.Name,
                UserFirstName=t.User.FirstName,
                UserLastName=t.User.LastName
                }
            ).ToList();

        return searchedTransactionsDtos;
    }

    public async Task<bool> DeleteTransactionAsync(int id, string userId)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction != null)
        {
            if (transaction.UserId == userId)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new UnauthorizedAccessException("You are not authorized to delete this transaction.");
        }
        throw new KeyNotFoundException($"Transaction with id {id} not found.");
    }
}
