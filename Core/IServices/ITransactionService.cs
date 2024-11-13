using System;
using Core.Dtos;
using Core.Entities;

namespace Core.IServices;

public interface ITransactionService
{
    Task<CreateUpdateTransaction> CreateTransactionAsync(CreateUpdateTransaction transaction, string userId);
    Task<CreateUpdateTransaction> GetTransactionByIdAsync(int id, string userId);
    Task<IEnumerable<CreateUpdateTransaction>> GetTransactionsByUserIdAsync(string userId);
    Task<IEnumerable<CreateUpdateTransaction>> GetTransactionsByCategoryIdAsync(int categoryId, string userId);
    // Task<CreateUpdateTransaction> UpdateTransactionAsync(int id, CreateUpdateTransaction transaction, string userId);
    Task<bool> DeleteTransactionAsync(int id, string userId);
}
