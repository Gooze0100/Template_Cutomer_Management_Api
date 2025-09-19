using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TemplateInfrastructure.Db.Entities;

namespace TemplateInfrastructure.Context;

public interface IDatabaseContext : IDisposable
{
    DbSet<Template> Templates { get; }
    
    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}