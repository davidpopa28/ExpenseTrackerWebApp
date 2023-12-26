using ExpenseTrackerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAccount>()
                .HasKey(ua => new {ua.UserId, ua.AccountId});
            modelBuilder.Entity<UserAccount>()
                .HasOne(u => u.User)
                .WithMany(ua => ua.UserAccounts)
                .HasForeignKey(u => u.UserId);
            modelBuilder.Entity<UserAccount>()
                .HasOne(a => a.Account)
                .WithMany(ua => ua.UserAccounts)
                .HasForeignKey(a => a.AccountId);
        }
    }
}
