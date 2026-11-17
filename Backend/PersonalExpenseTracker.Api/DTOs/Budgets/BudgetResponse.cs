
namespace PersonalExpenseTracker.Api.DTOs.Budgets;

public class BudgetResponse
{
    public int BudgetId { get; set; }

    public int Year { get; set; }

    public int Month { get; set; }

    public decimal BudgetAmount { get; set; }

    public decimal SpentAmount { get; set; }

    public decimal RemainingAmount { get; set; }

    public decimal PercentageUsed { get; set; }
}
