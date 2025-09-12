using CustomerManagementApi.Extensions;
using CustomerManagementApi.Handlers.Customer.Add;
using CustomerManagementApi.Handlers.Customer.Delete;
using CustomerManagementApi.Handlers.Customer.Update;
using CustomerManagementApi.Services.Customer;
using CustomerManagementApi.Validators.Customer;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace CustomerManagementApi.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        var customer = app.MapGroup("api/customer");

        customer.MapGet("{customerId:int}", async (int customerId, ICustomerService service, HttpContext ctx) => 
            await service.Get(customerId, ctx.RequestAborted).ToResponse())
            .CacheOutput(x =>
        {
            x.Tag(Constants.CacheTags.Customer);
        }).RequireAuthorization()
        .WithName("GetCustomer");

        customer.MapPost("/add", async (CustomerAddRequest req, CustomerAddValidator validator, ICustomerService service, HttpContext ctx) =>
        {
            var validationResult = await validator.ValidateAsync(req, ctx.RequestAborted);

            if (!validationResult.IsValid)
            {
                return validationResult.ToValidationResponse();
            }
            
            return await service.Add(req, ctx.RequestAborted).ToResponse();
        }).RequireAuthorization()
        .WithName("AddCustomer");
        
        customer.MapPatch("/update", async (CustomerUpdateRequest req, CustomerUpdateValidator validator, ICustomerService service, HttpContext ctx) =>
        {
            var validationResult = await validator.ValidateAsync(req, ctx.RequestAborted);

            if (!validationResult.IsValid)
            {
                return validationResult.ToValidationResponse();
            }
            
            return await service.Update(req, ctx.RequestAborted).ToResponse();
        }).RequireAuthorization()
        .WithName("UpdateCustomer");
        
        customer.MapDelete("/delete", async ([FromBody] CustomerDeleteRequest req, CustomerDeleteValidator validator, ICustomerService service, HttpContext ctx) =>
        {
            var validationResult = await validator.ValidateAsync(req, ctx.RequestAborted);

            if (!validationResult.IsValid)
            {
                return validationResult.ToValidationResponse();
            }
            
            return await service.Delete(req, ctx.RequestAborted).ToResponse();
        }).RequireAuthorization()
        .WithName("DeleteCustomer");
    }
    
    
}