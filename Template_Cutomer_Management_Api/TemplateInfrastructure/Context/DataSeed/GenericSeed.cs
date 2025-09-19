using Microsoft.EntityFrameworkCore;
using TemplateInfrastructure.Db.Entities;

namespace TemplateInfrastructure.Context.DataSeed;

public class GenericSeed
{
    public static async Task Seed(DbContext context, bool b, CancellationToken ct)
    {
        bool hasTemplates = await context.Set<Template>().AnyAsync(ct);
        if (hasTemplates)
        {
            return;
        }
        
        var date = DateTime.UtcNow;

        for (int i = 0; i <= 10000; i++)
        {
            await context.Set<Template>().AddAsync(new Template()
            {
                Name = $"Template name {i}",
                Subject = $"Subject {i}",
                Body = $"This is body nr. {i}. " + "You have very nice name: {{name}}, and yours email is: {{email}}",
                CreatedAt = date,
                CreatedBy = "SYSTEM",
                CreatedById = 1,
                UpdatedAt = date,
                UpdatedBy = "SYSTEM",
                UpdatedById = 1
            }, ct);
        }
    
        await context.SaveChangesAsync(ct);
    }
}