using Microsoft.EntityFrameworkCore;
using ProductCatalogApi.Models;

namespace ProductCatalogApi.Data;

// DbContext represents a session with the database
public class AppDbContext : DbContext
{
    // DbSet properties represent database tables
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    // Constructor that accepts DbContextOptions
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Configure the model relationships and constraints
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the one-to-many relationship
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)          // A product has one category
            .WithMany(c => c.Products)        // A category has many products
            .HasForeignKey(p => p.CategoryId); // Foreign key is CategoryId
        
     


    }
}