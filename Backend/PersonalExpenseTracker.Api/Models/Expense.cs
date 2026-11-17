
namespace PersonalExpenseTracker.Api.Models;

public class Expense
{
    public int ExpenseId { get; set; }

    public string Title { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime ExpenseDate { get; set; }

    public string? Description { get; set; }

    // Foreign key to Category
    public int CategoryId { get; set; }

    public Category Category { get; set; } = null!;

    // Foreign key to ApplicationUser
    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;
}
