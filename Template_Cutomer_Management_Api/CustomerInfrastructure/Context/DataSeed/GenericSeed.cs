using CustomerInfrastructure.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerInfrastructure.Context.DataSeed;

public class GenericSeed
{
    public static async Task Seed(DbContext context, bool b, CancellationToken ct)
    {
        bool hasCustomers = await context.Set<Customer>().AnyAsync(ct);
        if (hasCustomers)
        {
            return;
        }
        
        var date = DateTime.UtcNow;

        for (int i = 0; i <= 5; i++)
        {
            context.Set<Customer>().Add(new Customer()
            {
                Name = $"Customer name {i}",
                Email = $"Email {i}",
                CreatedAt = date,
                CreatedBy = "SYSTEM",
                CreatedById = 1,
                UpdatedAt = date,
                UpdatedBy = "SYSTEM",
                UpdatedById = 1
            });
        }
        
        await context.SaveChangesAsync(ct);
    }
}