using System;
using Core.Entities;

namespace Core.IServices;

public interface ITransactionService
{
    Task<Transaction> CreateTransactionAsync(Transaction transaction, string userId);
    Task<Transaction> GetTransactionByIdAsync(int id, string userId);
    Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(string userId);
    Task<IEnumerable<Transaction>> GetTransactionsByCategoryIdAsync(int categoryId, string userId);
    Task<Transaction> UpdateTransactionAsync(int id, Transaction transaction, string userId);
    Task<bool> DeleteTransactionAsync(int id, string userId);
}
