using CustomerInfrastructure.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerInfrastructure.Extensions;

public static class CustomerExtension
{
    public static void Config(this EntityTypeBuilder<Customer> entity)
    {
        entity.HasKey(e => e.Id);
        
        entity.Property(p => p.Id)
            .UseIdentityColumn();

        entity.ToTable(Constants.Tables.Customer, Constants.Schemas.Dbo);
        
        entity.Property(e => e.Name)
            .IsRequired();
        
        entity.Property(e => e.Email)
            .IsRequired();

        entity.Property(p => p.RemovedAt)
            .IsRequired(false);
        
        entity.Property(p => p.RemovedBy)
            .IsRequired(false);
        
        entity.Property(p => p.RemovedById)
            .IsRequired(false);
    }
}