
using PersonalExpenseTracker.Api.DTOs.Budgets;

namespace PersonalExpenseTracker.Api.Services.Interfaces;

public interface IBudgetService
{
    Task<(BudgetResponse Budget, bool Created)> CreateOrUpdateAsync(
        CreateBudgetRequest request,
        string userId);

    Task<BudgetResponse?> GetBudgetAsync(
        string userId,
        int year,
        int month);

    Task<BudgetResponse?> UpdateAsync(
        UpdateBudgetRequest request,
        string userId);
}
