using CustomerInfrastructure.Db.Entities;
using CustomerInfrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CustomerInfrastructure.Context;

public class DatabaseContext : DbContext, IDatabaseContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    
    public virtual DbSet<Customer> Customers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().Config();
    }
}