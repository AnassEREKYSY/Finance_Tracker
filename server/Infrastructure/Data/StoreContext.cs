using Core.Entities;
using Core.IData;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StoreContext : IdentityDbContext<AppUser>, IStoreContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ExportRequest> ExportRequests { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Budget - User relationship
            modelBuilder.Entity<Budget>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .HasPrincipalKey(u => u.Id);

            // Budget - Category relationship with SetNull on delete
            modelBuilder.Entity<Budget>()
                .HasOne(b => b.Category)
                .WithMany()
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // Transaction - User relationship
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .HasPrincipalKey(u => u.Id);

            // ExportRequest - User relationship
            modelBuilder.Entity<ExportRequest>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);

            // Notification - User relationship
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId);

            // Precision for amounts
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Budget>()
                .Property(b => b.Amount)
                .HasPrecision(18, 2);

            // Category - Transaction relationship
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Transactions)
                .WithOne(t => t.Category)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // Category - Budget relationship
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Budgets)
                .WithOne(b => b.Category)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Notification - User and Budget relationship
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Budget)
                .WithMany()
                .HasForeignKey(n => n.BudgetId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Business & Expenses" },
                new Category { CategoryId = 2, Name = "Childcare & Education" },
                new Category { CategoryId = 3, Name = "Clothing & Accessories" },
                new Category { CategoryId = 4, Name = "Debt" },
                new Category { CategoryId = 5, Name = "Dining Out" },
                new Category { CategoryId = 6, Name = "Education" },
                new Category { CategoryId = 7, Name = "Entertainment" },
                new Category { CategoryId = 8, Name = "Fuel" },
                new Category { CategoryId = 9, Name = "Gifts & Donations" },
                new Category { CategoryId = 10, Name = "Groceries" },
                new Category { CategoryId = 11, Name = "Health" },
                new Category { CategoryId = 12, Name = "Insurance" },
                new Category { CategoryId = 13, Name = "Medications" },
                new Category { CategoryId = 14, Name = "Maintenance" },
                new Category { CategoryId = 15, Name = "Parking" },
                new Category { CategoryId = 16, Name = "Personal Care" },
                new Category { CategoryId = 17, Name = "Repairs" },
                new Category { CategoryId = 18, Name = "Rent/Mortgage" },
                new Category { CategoryId = 19, Name = "Savings & Investments" },
                new Category { CategoryId = 20, Name = "Taxes" },
                new Category { CategoryId = 21, Name = "Technology & Electronics" },
                new Category { CategoryId = 22, Name = "Transportation" },
                new Category { CategoryId = 23, Name = "Utilities" },
                new Category { CategoryId = 24, Name = "Vacation" }
            );
        }
    }
}
