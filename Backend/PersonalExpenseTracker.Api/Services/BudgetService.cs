
using PersonalExpenseTracker.Api.DTOs.Budgets;
using PersonalExpenseTracker.Api.Models;
using PersonalExpenseTracker.Api.Repositories.Interfaces;
using PersonalExpenseTracker.Api.Services.Interfaces;

namespace PersonalExpenseTracker.Api.Services;

public class BudgetService : IBudgetService
{
    private readonly IBudgetRepository _budgetRepository;

    public BudgetService(IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    public async Task<(BudgetResponse Budget, bool Created)> CreateOrUpdateAsync(
        CreateBudgetRequest request,
        string userId)
    {
        var existingBudget =
            await _budgetRepository.GetByUserAndMonthAsync(
                userId,
                request.Year,
                request.Month);

        bool created = false;

        if (existingBudget == null)
        {
            existingBudget = new Budget
            {
                Year = request.Year,
                Month = request.Month,
                Amount = request.Amount,
                UserId = userId
            };

            await _budgetRepository.AddAsync(existingBudget);

            created = true;
        }
        else
        {
            existingBudget.Amount = request.Amount;

            await _budgetRepository.SaveChangesAsync();
        }

        var response = await BuildResponseAsync(existingBudget, userId);

        return (response, created);
    }

    public async Task<BudgetResponse?> GetBudgetAsync(
        string userId,
        int year,
        int month)
    {
        var budget = await _budgetRepository.GetByUserAndMonthAsync(
            userId,
            year,
            month);

        if (budget == null)
        {
            return null;
        }

        return await BuildResponseAsync(budget, userId);
    }

    public async Task<BudgetResponse?> UpdateAsync(
        UpdateBudgetRequest request,
        string userId)
    {
        var budget = await _budgetRepository.GetByUserAndMonthAsync(
            userId,
            request.Year,
            request.Month);

        if (budget == null)
        {
            return null;
        }

        budget.Amount = request.Amount;

        await _budgetRepository.SaveChangesAsync();

        return await BuildResponseAsync(budget, userId);
    }

    private async Task<BudgetResponse> BuildResponseAsync(
        Budget budget,
        string userId)
    {
        var spentAmount = await _budgetRepository.GetSpentAmountAsync(
            userId,
            budget.Year,
            budget.Month);

        var remainingAmount = budget.Amount - spentAmount;

        var percentageUsed = budget.Amount > 0
            ? Math.Round((spentAmount / budget.Amount) * 100, 2)
            : 0;

        return new BudgetResponse
        {
            BudgetId = budget.BudgetId,
            Year = budget.Year,
            Month = budget.Month,
            BudgetAmount = budget.Amount,
            SpentAmount = spentAmount,
            RemainingAmount = remainingAmount,
            PercentageUsed = percentageUsed
        };
    }
}
