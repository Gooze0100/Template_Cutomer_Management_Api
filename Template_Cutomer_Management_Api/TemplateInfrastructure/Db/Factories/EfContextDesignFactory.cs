using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TemplateInfrastructure.Context;

namespace TemplateInfrastructure.Db.Factories;

public class EfContextDesignFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<DatabaseContext>();
        builder.UseSqlServer(GetDefaultConnectionString(),
            options =>
            {
                options.MigrationsHistoryTable(Constants.Tables.MigrationTable, Constants.Schemas.Dbo);
            });
        
        return new DatabaseContext(builder.Options);
    }

    private static string GetDefaultConnectionString()
    {
        const string projectName = "TemplateManagementApi";
        const string databaseName = "TemplateInfrastructure";
        char separator = Path.DirectorySeparatorChar;

        var path = AppContext.BaseDirectory.Replace($"{separator}{databaseName}{separator}bin{separator}",
            $"{separator}{projectName}{separator}bin{separator}", StringComparison.OrdinalIgnoreCase);
        
        var configureFile = $"{path}{separator}appsettings.Development.json";
        
        using var stream = new StreamReader(configureFile);
        var json = stream.ReadToEnd();

        var items = JsonSerializer.Deserialize<DbContextSettings>(json);
        var connectionString = items?.ConnectionStrings?.DefaultConnection ?? String.Empty;
        
        return connectionString;
    }
}