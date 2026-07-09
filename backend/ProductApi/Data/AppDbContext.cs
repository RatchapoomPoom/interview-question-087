using com.example.productapi.Models;
using Microsoft.EntityFrameworkCore;


namespace com.example.productapi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // บังคับให้ ProductCode ห้ามซ้ำในฐานข้อมูล
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.ProductCode)
                .IsUnique();
        }
    }
}
