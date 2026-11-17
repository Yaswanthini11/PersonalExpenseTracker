using PersonalExpenseTracker.Api.DTOs.Expenses;

namespace PersonalExpenseTracker.Api.Services.Interfaces;

public interface IExpenseService
{
    Task<List<ExpenseResponse>> GetAllAsync(
        string userId,
        int? month,
        int? year,
        int? categoryId);

    Task<ExpenseResponse?> GetByIdAsync(
        int expenseId,
        string userId);

    Task<ExpenseResponse?> CreateAsync(
        CreateExpenseRequest request,
        string userId);

    Task<ExpenseUpdateResult> UpdateAsync(
        int expenseId,
        UpdateExpenseRequest request,
        string userId);

    Task<bool> DeleteAsync(
        int expenseId,
        string userId);
}