using CSharpFunctionalExtensions;
using TemplateManagementApi.Handlers.Template.Add;
using TemplateManagementApi.Handlers.Template.Delete;
using TemplateManagementApi.Handlers.Template.Get;
using TemplateManagementApi.Handlers.Template.Update;
using Entities = TemplateInfrastructure.Db.Entities;

namespace TemplateManagementApi.Services.Template;

public interface ITemplateService
{
    Task<Result<TemplateGetResponse, Exception>> Get(int templateId, CancellationToken ct);
    Task<UnitResult<Exception>> Add(TemplateAddRequest req, CancellationToken ct);
    Task<UnitResult<Exception>> Update(TemplateUpdateRequest req, CancellationToken ct);
    Task<UnitResult<Exception>> Delete(TemplateDeleteRequest req, CancellationToken ct);


    Task<List<Entities.Template>> GetTemplatesAsync(CancellationToken ct);
}