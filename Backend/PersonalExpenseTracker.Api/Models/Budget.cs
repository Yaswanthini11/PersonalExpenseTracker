
namespace PersonalExpenseTracker.Api.Models;

public class Budget
{
    public int BudgetId { get; set; }

    public int Year { get; set; }

    public int Month { get; set; }

    public decimal Amount { get; set; }

    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;
}
