using Microsoft.EntityFrameworkCore;
using MyReactApp.Models;
namespace MyReactApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<UserData> UserDatas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserData>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.HasKey(e => e.Id);
            });
        }
    }

}
