using CongratulatorWebAppAPI.BuisnesObjects;
using Microsoft.EntityFrameworkCore;

namespace CongratulatorWebAppAPI.DataBase
{
    public class CongratulationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserCongratulation> UserCongratulations { get; set; }
        public DbSet<UserImage> UserImages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CongratulatorDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка связи один-к-одному
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserImage)
                .WithOne(p => p.User)
                .HasForeignKey<UserImage>(p => p.UserId);
        }
    }
}
