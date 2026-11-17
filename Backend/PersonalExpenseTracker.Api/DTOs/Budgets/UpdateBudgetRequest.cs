
using System.ComponentModel.DataAnnotations;

namespace PersonalExpenseTracker.Api.DTOs.Budgets;

public class UpdateBudgetRequest
{
    [Range(1, 9999)]
    public int Year { get; set; }

    [Range(1, 12)]
    public int Month { get; set; }

    [Range(typeof(decimal), "0.01", "9999999999999999.99")]
    public decimal Amount { get; set; }
}
