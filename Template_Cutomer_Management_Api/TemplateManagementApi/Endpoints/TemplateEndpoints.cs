using Microsoft.AspNetCore.Mvc;
using Shared;
using TemplateManagementApi.Extensions;
using TemplateManagementApi.Handlers.Template.Add;
using TemplateManagementApi.Handlers.Template.Delete;
using TemplateManagementApi.Handlers.Template.Update;
using TemplateManagementApi.Services.Template;
using TemplateManagementApi.Validators.Template;

namespace TemplateManagementApi.Endpoints;

public static class TemplateEndpoints
{
    public static void MapTemplateEndpoints(this WebApplication app)
    {
        var template = app.MapGroup("api/template");
        
        template.MapGet("{templateId:int}", async (int templateId, ITemplateService service, HttpContext ctx) => 
            await service.Get(templateId, ctx.RequestAborted).ToResponse())
            .CacheOutput(x =>
        {
            x.Tag(Constants.CacheTags.Template);
        }).RequireAuthorization()
        .WithName("GetTemplate");

        template.MapPost("/add", async (TemplateAddRequest req, TemplateAddValidator validator, ITemplateService service, HttpContext ctx) =>
        {
            var validationResult = await validator.ValidateAsync(req, ctx.RequestAborted);

            if (!validationResult.IsValid)
            {
                return validationResult.ToValidationResponse();
            }
            
            return await service.Add(req, ctx.RequestAborted).ToResponse();
        }).RequireAuthorization()
        .WithName("AddTemplate");
        
        template.MapPatch("/update", async (TemplateUpdateRequest req, TemplateUpdateValidator validator, ITemplateService service, HttpContext ctx) =>
        {
            var validationResult = await validator.ValidateAsync(req, ctx.RequestAborted);

            if (!validationResult.IsValid)
            {
                return validationResult.ToValidationResponse();
            }
            
            return await service.Update(req, ctx.RequestAborted).ToResponse();
        }).RequireAuthorization()
        .WithName("UpdateTemplate");
        
        template.MapDelete("/delete", async ([FromBody] TemplateDeleteRequest req, TemplateDeleteValidator validator, ITemplateService service, HttpContext ctx) =>
        {
            var validationResult = await validator.ValidateAsync(req, ctx.RequestAborted);

            if (!validationResult.IsValid)
            {
                return validationResult.ToValidationResponse();
            }
            
            return await service.Delete(req, ctx.RequestAborted).ToResponse();
        }).RequireAuthorization()
        .WithName("DeleteTemplate");
    }
}