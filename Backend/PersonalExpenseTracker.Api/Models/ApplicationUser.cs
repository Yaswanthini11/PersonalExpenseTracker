
using Microsoft.AspNetCore.Identity;

namespace PersonalExpenseTracker.Api.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public ICollection<Category> Categories { get; set; }
        = new List<Category>();
    public ICollection<Expense> Expenses { get; set; }
    = new List<Expense>();

    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();

}
