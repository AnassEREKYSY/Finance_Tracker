using System;
using Core.Dtos;
using Core.Entities;
using Core.Response;

namespace Core.IServices;

public interface ITransactionService
{
    Task<ServiceResponse<CreateUpdateTransaction>> CreateTransactionAsync(CreateUpdateTransaction transaction, string userId);
    Task<ServiceResponse<CreateUpdateTransaction>> GetTransactionByIdAsync(int id, string userId);
    Task<ServiceResponse<IEnumerable<CreateUpdateTransaction>>> GetTransactionsByUserIdAsync(string userId);
    Task<ServiceResponse<IEnumerable<CreateUpdateTransaction>>> GetTransactionsByCategoryNameAsync(string categoryName, string userId);
    // Task<CreateUpdateTransaction> UpdateTransactionAsync(int id, CreateUpdateTransaction transaction, string userId);
    Task<ServiceResponse<bool>> DeleteTransactionAsync(int id, string userId);
}
