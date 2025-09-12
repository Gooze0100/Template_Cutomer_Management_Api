using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using TemplateInfrastructure.Context;
using TemplateManagementApi.Handlers.Template.Add;
using TemplateManagementApi.Handlers.Template.Delete;
using TemplateManagementApi.Handlers.Template.Get;
using TemplateManagementApi.Handlers.Template.Update;
using Entities = TemplateInfrastructure.Db.Entities;

namespace TemplateManagementApi.Services.Template;

public class TemplateService : ITemplateService
{
    private readonly ILogger<TemplateService> _logger;
    private readonly IDatabaseContext _databaseContext;

    public TemplateService(ILogger<TemplateService> logger, IDatabaseContext databaseContext)
    {
        _logger = logger;
        _databaseContext = databaseContext;
    }
    
    public async Task<Result<TemplateGetResponse, Exception>> Get(int templateId, CancellationToken ct)
    {
        try
        {
            var template = await _databaseContext.Templates
                .TagWithCallSite()
                .AsNoTracking()
                .Where(x => x.Id == templateId && x.DeletedAt == null)
                .Select(x => new TemplateGetResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Subject = x.Subject,
                    Body = x.Body
                })
                .FirstOrDefaultAsync(ct);

            if (template == null)
            {
                return new NotFoundException();
            }
            
            return template;
        }
        catch (Exception e)
        {
            _logger.LogError(e, nameof(Get));
            return e;
        }
    }
    
    public async Task<UnitResult<Exception>> Add(TemplateAddRequest req, CancellationToken ct)
    {
        try
        {
            var template = new Entities.Template()
            {
                Name = req.Name,
                Subject = req.Subject,
                Body = req.Body
            };
            
            template.MarkAsCreated();
            await _databaseContext.Templates.AddAsync(template, ct);
            await _databaseContext.SaveChangesAsync(ct);

            return Result.Success<Exception>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, nameof(Add));
            return e;
        }
    }
    
    public async Task<UnitResult<Exception>> Update(TemplateUpdateRequest req, CancellationToken ct)
    {
        try
        {
            var template = await _databaseContext.Templates
                .TagWithCallSite()
                .Where(x => x.Id == req.Id && x.DeletedAt == null)
                .FirstOrDefaultAsync(ct);
            
            if (template == null)
            {
                return new NotFoundException();
            }
            
            template.Name = req.Name;
            template.Subject = req.Subject;
            template.Body = req.Body;
            template.MarkAsUpdated();
            
            await _databaseContext.SaveChangesAsync(ct);
            
            return UnitResult.Success<Exception>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, nameof(Update));
            return e;
        }
    }
    
    public async Task<UnitResult<Exception>> Delete(TemplateDeleteRequest req, CancellationToken ct)
    {
        try
        {
            var template = await _databaseContext.Templates
                .TagWithCallSite()
                .Where(x => x.Id == req.Id && x.DeletedAt == null)
                .FirstOrDefaultAsync(ct);
            
            if (template == null)
            {
                return new NotFoundException();
            }
            
            template.MarkAsDeleted();
            
            await _databaseContext.SaveChangesAsync(ct);
            
            return UnitResult.Success<Exception>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, nameof(Update));
            return e;
        }
    }
}