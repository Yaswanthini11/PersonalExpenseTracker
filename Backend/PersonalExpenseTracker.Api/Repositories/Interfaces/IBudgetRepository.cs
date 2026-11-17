
using PersonalExpenseTracker.Api.Models;

namespace PersonalExpenseTracker.Api.Repositories.Interfaces;

public interface IBudgetRepository
{
    Task<Budget?> GetByUserAndMonthAsync(
        string userId,
        int year,
        int month);

    Task<decimal> GetSpentAmountAsync(
        string userId,
        int year,
        int month);

    Task AddAsync(Budget budget);

    Task SaveChangesAsync();
}
