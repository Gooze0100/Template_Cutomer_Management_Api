using Microsoft.EntityFrameworkCore;
using TemplateInfrastructure.Db.Entities;
using TemplateInfrastructure.Extensions;

namespace TemplateInfrastructure.Context;

public class DatabaseContext : DbContext, IDatabaseContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    
    public virtual DbSet<Template> Templates { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Template>().Config();
    }
}