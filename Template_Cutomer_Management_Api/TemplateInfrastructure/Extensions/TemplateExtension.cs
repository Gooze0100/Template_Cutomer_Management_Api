using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TemplateInfrastructure.Db.Entities;

namespace TemplateInfrastructure.Extensions;

public static class TemplateExtension
{
    public static void Config(this EntityTypeBuilder<Template> entity)
    {
        entity.HasKey(e => e.Id);
        
        entity.Property(p => p.Id)
            .UseIdentityColumn();

        entity.ToTable(Constants.Tables.Templates, Constants.Schemas.Dbo);
        
        entity.Property(p => p.Name)
            .IsRequired();
        
        entity.Property(p => p.Subject)
            .IsRequired();
        
        entity.Property(p => p.Body)
            .IsRequired();
        
        entity.Property(p => p.RemovedAt)
            .IsRequired(false);
        
        entity.Property(p => p.RemovedBy)
            .IsRequired(false);
        
        entity.Property(p => p.RemovedById)
            .IsRequired(false);
    }
}