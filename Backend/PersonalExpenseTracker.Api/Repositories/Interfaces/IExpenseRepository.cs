using PersonalExpenseTracker.Api.Models;

namespace PersonalExpenseTracker.Api.Repositories.Interfaces;

public interface IExpenseRepository
{
    Task<List<Expense>> GetAllForUserAsync(
        string userId,
        int? month,
        int? year,
        int? categoryId);

    Task<Expense?> GetByIdForUserAsync(
        int expenseId,
        string userId);

    Task<bool> IsCategoryAvailableForUserAsync(
        int categoryId,
        string userId);

    Task AddAsync(Expense expense);

    Task SaveChangesAsync();

    Task DeleteAsync(Expense expense);
}