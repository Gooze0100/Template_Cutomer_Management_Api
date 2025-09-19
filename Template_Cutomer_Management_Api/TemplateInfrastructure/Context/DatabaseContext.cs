using Microsoft.EntityFrameworkCore;
using TemplateInfrastructure.Db.Entities;
using TemplateInfrastructure.Extensions;

namespace TemplateInfrastructure.Context;

public class DatabaseContext : DbContext, IDatabaseContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    
    public DbSet<Template> Templates => Set<Template>();
}