using Microsoft.EntityFrameworkCore;
using MyFin.Domain.Entities;

namespace MyFin.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<TBLUser> Users { get; set; }
        public DbSet<TBLAccount> Accounts { get; set; }
        public DbSet<TBLCategory> Categories { get; set; }
        public DbSet<TBLTransaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TBLTransaction>()
                .HasOne(t => t.Account)           
                .WithMany(a => a.Transactions)    
                .HasForeignKey(t => t.AccountId)  
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TBLTransaction>()
                .HasOne(t => t.Category)          
                .WithMany()                       
                .HasForeignKey(t => t.AccountId) 
                .IsRequired(false);

            modelBuilder.Entity<TBLAccount>()
                .HasOne(a => a.User)
                .WithMany(u => u.Accounts)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<TBLCategory>()
                .HasOne(c => c.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(c => c.UserId)
                .IsRequired(false);

            modelBuilder.Entity<TBLTransaction>()
                .HasIndex(t => new { t.UserId, t.DtTimeStamp });
            modelBuilder.Entity<TBLTransaction>()
                .HasIndex(t => t.AccountId);

            modelBuilder.Entity<TBLCategory>()
                .Property(c => c.Type)
                .HasDefaultValue("Expense");
        }
    }
}
