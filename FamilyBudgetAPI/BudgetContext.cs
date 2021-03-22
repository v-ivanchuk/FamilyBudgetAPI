using FamilyBudgetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudgetAPI
{
    public class BudgetContext : DbContext
    {

        #region Tables
        public DbSet<Member> Members { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Family> Families { get; set; }
        #endregion

        public BudgetContext(DbContextOptions<BudgetContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
