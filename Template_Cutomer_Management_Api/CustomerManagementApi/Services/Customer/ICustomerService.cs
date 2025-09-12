using CSharpFunctionalExtensions;
using CustomerManagementApi.Handlers.Customer.Add;
using CustomerManagementApi.Handlers.Customer.Delete;
using CustomerManagementApi.Handlers.Customer.Get;
using CustomerManagementApi.Handlers.Customer.Update;

namespace CustomerManagementApi.Services.Customer;

public interface ICustomerService
{
    Task<Result<CustomerGetResponse, Exception>> Get(int customerId, CancellationToken ct);
    Task<UnitResult<Exception>> Add(CustomerAddRequest req, CancellationToken ct);
    Task<UnitResult<Exception>> Update(CustomerUpdateRequest req, CancellationToken ct);
    Task<UnitResult<Exception>> Delete(CustomerDeleteRequest req, CancellationToken ct);
}