using CSharpFunctionalExtensions;
using CustomerInfrastructure.Context;
using CustomerManagementApi.Handlers.Customer.Add;
using CustomerManagementApi.Handlers.Customer.Delete;
using CustomerManagementApi.Handlers.Customer.Get;
using CustomerManagementApi.Handlers.Customer.Update;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using Entities = CustomerInfrastructure.Db.Entities;

namespace CustomerManagementApi.Services.Customer;

public class CustomerService : ICustomerService
{
    private readonly ILogger<CustomerService> _logger;
    private readonly IDatabaseContext _databaseContext;

    public CustomerService(ILogger<CustomerService> logger, IDatabaseContext databaseContext)
    {
        _logger = logger;
        _databaseContext = databaseContext;
    }

    /// <summary>
    /// Get customer information from database
    /// </summary>
    /// <param name="customerId">Id of a customer in database</param>
    /// <param name="ct">Cancellation token that can be used to cancel the operation.</param>
    /// <returns>Customer information or exception</returns>
    public async Task<Result<CustomerGetResponse, Exception>> Get(int customerId, CancellationToken ct)
    {
        try
        {
            var customer = await _databaseContext.Customers
                .TagWithCallSite()
                .AsNoTracking()
                .Where(x => x.Id == customerId && x.DeletedAt == null)
                .Select(x => new CustomerGetResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email
                })
                .FirstOrDefaultAsync(ct);

            if (customer == null)
            {
                return new NotFoundException();
            }
            
            return customer;
        }
        catch (Exception e)
        {
            _logger.LogError(e, nameof(Get));
            return e;
        }
    }

    /// <summary>
    /// Add new customer to database
    /// </summary>
    /// <param name="req">The <see cref="CustomerAddRequest"/> model containing customer add details.</param>
    /// <param name="ct">Cancellation token that can be used to cancel the operation.</param>
    /// <returns>Status of action or exception</returns>
    public async Task<UnitResult<Exception>> Add(CustomerAddRequest req, CancellationToken ct)
    {
        try
        {
            var customer = new Entities.Customer()
            {
                Name = req.Name,
                Email = req.Email,
            };
            
            customer.MarkAsCreated();
            await _databaseContext.Customers.AddAsync(customer, ct);
            await _databaseContext.SaveChangesAsync(ct);

            return Result.Success<Exception>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, nameof(Add));
            return e;
        }
    }

    /// <summary>
    /// Update existing customer in database
    /// </summary>
    /// <param name="req">The <see cref="CustomerUpdateRequest"/> model containing customer update details.</param>
    /// <param name="ct">Cancellation token that can be used to cancel the operation.</param>
    /// <returns>Status of action or exception</returns>
    public async Task<UnitResult<Exception>> Update(CustomerUpdateRequest req, CancellationToken ct)
    {
        try
        {
            var customer = await _databaseContext.Customers
                .TagWithCallSite()
                .Where(x => x.Id == req.Id && x.DeletedAt == null)
                .FirstOrDefaultAsync(ct);
            
            if (customer == null)
            {
                return new NotFoundException();
            }
            
            customer.Name = req.Name;
            customer.Email = req.Email;
            customer.MarkAsUpdated();
            
            await _databaseContext.SaveChangesAsync(ct);
            
            return UnitResult.Success<Exception>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, nameof(Update));
            return e;
        }
    }
    
    /// <summary>
    /// Delete existing customer in database
    /// </summary>
    /// <param name="req">The <see cref="CustomerDeleteRequest"/> model containing customer deletion details.</param>
    /// <param name="ct">Cancellation token that can be used to cancel the operation</param>
    /// <returns>Status of action or exception</returns>
    public async Task<UnitResult<Exception>> Delete(CustomerDeleteRequest req, CancellationToken ct)
    {
        try
        {
            var customer = await _databaseContext.Customers
                .TagWithCallSite()
                .Where(x => x.Id == req.Id && x.DeletedAt == null)
                .FirstOrDefaultAsync(ct);
            
            if (customer == null)
            {
                return new NotFoundException();
            }
            
            customer.MarkAsDeleted();
            
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