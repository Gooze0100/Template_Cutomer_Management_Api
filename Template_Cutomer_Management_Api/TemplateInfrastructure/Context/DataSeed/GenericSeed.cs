using Microsoft.EntityFrameworkCore;
using TemplateInfrastructure.Db.Entities;

namespace TemplateInfrastructure.Context.DataSeed;

public class GenericSeed
{
    public static async Task Seed(DbContext context, bool b, CancellationToken ct)
    {
        var date = DateTime.UtcNow;

        for (int i = 0; i <= 5; i++)
        {
            context.Set<Template>().Add(new Template()
            {
                Name = $"Template name {i}",
                Subject = $"Subject {i}",
                Body = $"Body {i}, very nice <Name>, and yours <Email> ",
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