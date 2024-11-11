using System;
using Core.Entities;
using Core.IData;
using Core.IServices;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class TransactionService(IStoreContext _context) : ITransactionService
{
    public async Task<Transaction> CreateTransactionAsync(Transaction transaction, string userId)
    {
        if (transaction != null)
        {
            transaction.UserId=userId;
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        throw new ArgumentNullException(nameof(transaction));
    }

    public async Task<Transaction> GetTransactionByIdAsync(int id, string userId)
    {
        var transaction = await _context.Transactions.Where(t => t.TransactionId == id && t.UserId == userId).FirstOrDefaultAsync();
        return transaction ?? throw new KeyNotFoundException($"Transaction with id {id} not found.");
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(string userId)
    {
        return await _context.Transactions.Where(t => t.UserId == userId).ToListAsync();
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByCategoryIdAsync(int categoryId, string userId)
    {
        return await _context.Transactions.Where(t => t.CategoryId == categoryId && t.UserId == userId).ToListAsync();
    }

    public async Task<Transaction> UpdateTransactionAsync(int id, Transaction transaction, string userId)
    {
        if (transaction != null)
        {
            var existingTransaction = await _context.Transactions.FindAsync(id) ?? throw new KeyNotFoundException($"Transaction with id {id} not found.");
            if (existingTransaction.UserId == userId)
            {
                existingTransaction.Amount = transaction.Amount;
                existingTransaction.Description = transaction.Description;
                existingTransaction.Date = transaction.Date;
                existingTransaction.Type = transaction.Type;
                existingTransaction.Category = transaction.Category;
                await _context.SaveChangesAsync();
                return existingTransaction;
            }
            throw new UnauthorizedAccessException("You are not authorized to update this transaction.");
        }

        throw new ArgumentNullException(nameof(transaction));
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
