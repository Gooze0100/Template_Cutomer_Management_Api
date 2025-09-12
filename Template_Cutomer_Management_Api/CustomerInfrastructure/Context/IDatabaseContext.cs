using CustomerInfrastructure.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CustomerInfrastructure.Context;

public interface IDatabaseContext : IDisposable
{
    DbSet<Customer> Customers { get; set; }
    
    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}