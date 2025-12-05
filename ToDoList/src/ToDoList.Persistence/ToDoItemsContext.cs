namespace ToDoList.Persistence;

using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class ToDoItemsContext : DbContext
{
    private readonly string connectionString;
    public ToDoItemsContext(string connectionString = "DataSource=../../data/localdb.db")
    {
        this.connectionString = connectionString;
    }

    public DbSet<ToDoItem> ToDoItems { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ToDoItem>()
            .HasOne(t => t.Category)
            .WithMany(c => c.ToDoItems)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
