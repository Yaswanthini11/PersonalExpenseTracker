using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalExpenseTracker.Api.Models;

namespace PersonalExpenseTracker.Api.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.CategoryId);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Description)
            .HasMaxLength(200);

        builder.HasOne(c => c.User)
            .WithMany(u => u.Categories)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Default categories are shared by all users.
        builder.HasData(
            new Category
            {
                CategoryId = 1,
                Name = "Food",
                Description = "Food and dining expenses",
                UserId = null
            },
            new Category
            {
                CategoryId = 2,
                Name = "Transportation",
                Description = "Travel and transportation expenses",
                UserId = null
            },
            new Category
            {
                CategoryId = 3,
                Name = "Shopping",
                Description = "Shopping and personal purchases",
                UserId = null
            },
            new Category
            {
                CategoryId = 4,
                Name = "Bills and Utilities",
                Description = "Household bills and utility payments",
                UserId = null
            },
            new Category
            {
                CategoryId = 5,
                Name = "Healthcare",
                Description = "Medical and healthcare expenses",
                UserId = null
            },
            new Category
            {
                CategoryId = 6,
                Name = "Entertainment",
                Description = "Movies and entertainment expenses",
                UserId = null
            },
            new Category
            {
                CategoryId = 7,
                Name = "Education",
                Description = "Education and learning expenses",
                UserId = null
            }
        );
    }
}