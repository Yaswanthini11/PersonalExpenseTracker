using System.ComponentModel.DataAnnotations;

namespace PersonalExpenseTracker.Api.DTOs.Expenses;

public class CreateExpenseRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Title { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "9999999999999999.99")]
    public decimal Amount { get; set; }

    [Required]
    public DateTime ExpenseDate { get; set; }

    [StringLength(250)]
    public string? Description { get; set; }

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }
}