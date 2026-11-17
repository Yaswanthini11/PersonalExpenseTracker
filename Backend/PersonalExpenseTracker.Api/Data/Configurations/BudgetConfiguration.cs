
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalExpenseTracker.Api.Models;

namespace PersonalExpenseTracker.Api.Data.Configurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.HasKey(b => b.BudgetId);

        builder.Property(b => b.Year)
            .IsRequired();

        builder.Property(b => b.Month)
            .IsRequired();

        builder.Property(b => b.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasOne(b => b.User)
            .WithMany(u => u.Budgets)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(b => new
        {
            b.UserId,
            b.Year,
            b.Month
        })
        .IsUnique();
    }
}
