using PersonalExpenseTracker.Api.DTOs.Expenses;
using PersonalExpenseTracker.Api.Models;
using PersonalExpenseTracker.Api.Repositories.Interfaces;
using PersonalExpenseTracker.Api.Services.Interfaces;

namespace PersonalExpenseTracker.Api.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;

    public ExpenseService(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<List<ExpenseResponse>> GetAllAsync(
        string userId,
        int? month,
        int? year,
        int? categoryId)
    {
        var expenses = await _expenseRepository.GetAllForUserAsync(
            userId,
            month,
            year,
            categoryId);

        return expenses.Select(MapToResponse).ToList();
    }

    public async Task<ExpenseResponse?> GetByIdAsync(
        int expenseId,
        string userId)
    {
        var expense = await _expenseRepository.GetByIdForUserAsync(
            expenseId,
            userId);

        if (expense == null)
        {
            return null;
        }

        return MapToResponse(expense);
    }

    public async Task<ExpenseResponse?> CreateAsync(
        CreateExpenseRequest request,
        string userId)
    {
        var categoryAvailable =
            await _expenseRepository.IsCategoryAvailableForUserAsync(
                request.CategoryId,
                userId);

        if (!categoryAvailable)
        {
            return null;
        }

        var expense = new Expense
        {
            Title = request.Title.Trim(),
            Amount = request.Amount,
            ExpenseDate = request.ExpenseDate.Date,
            Description = request.Description?.Trim(),
            CategoryId = request.CategoryId,
            UserId = userId
        };

        await _expenseRepository.AddAsync(expense);

        var savedExpense =
            await _expenseRepository.GetByIdForUserAsync(
                expense.ExpenseId,
                userId);

        return savedExpense == null
            ? null
            : MapToResponse(savedExpense);
    }

    public async Task<ExpenseUpdateResult> UpdateAsync(
        int expenseId,
        UpdateExpenseRequest request,
        string userId)
    {
        var expense = await _expenseRepository.GetByIdForUserAsync(
            expenseId,
            userId);

        if (expense == null)
        {
            return ExpenseUpdateResult.NotFound;
        }

        var categoryAvailable =
            await _expenseRepository.IsCategoryAvailableForUserAsync(
                request.CategoryId,
                userId);

        if (!categoryAvailable)
        {
            return ExpenseUpdateResult.InvalidCategory;
        }

        expense.Title = request.Title.Trim();
        expense.Amount = request.Amount;
        expense.ExpenseDate = request.ExpenseDate.Date;
        expense.Description = request.Description?.Trim();
        expense.CategoryId = request.CategoryId;

        await _expenseRepository.SaveChangesAsync();

        return ExpenseUpdateResult.Success;
    }

    public async Task<bool> DeleteAsync(
        int expenseId,
        string userId)
    {
        var expense = await _expenseRepository.GetByIdForUserAsync(
            expenseId,
            userId);

        if (expense == null)
        {
            return false;
        }

        await _expenseRepository.DeleteAsync(expense);

        return true;
    }

    private static ExpenseResponse MapToResponse(Expense expense)
    {
        return new ExpenseResponse
        {
            ExpenseId = expense.ExpenseId,
            Title = expense.Title,
            Amount = expense.Amount,
            ExpenseDate = expense.ExpenseDate,
            Description = expense.Description,
            CategoryId = expense.CategoryId,
            CategoryName = expense.Category.Name
        };
    }
}