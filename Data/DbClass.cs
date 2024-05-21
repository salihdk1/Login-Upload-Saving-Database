using LDap.Models;
using Microsoft.EntityFrameworkCore;

namespace LDap.Data
{
    public class DbClass : DbContext
    {
        public DbClass(DbContextOptions<DbClass> options) : base(options)
        {
        }

        public DbSet<Model> EkDers { get; set; }
        public DbSet<AppUser> UserRole { get; set; } // AppUser için DbSet ekle

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Model>().HasKey(m => m.SıraNo);

            // AppUser için yapılandırma ekle
            modelBuilder.Entity<AppUser>().ToTable("UserRole");
        }
    }
}
