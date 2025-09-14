using CustomerInfrastructure.Context;
using CustomerManagementApi.Handlers.Customer.Add;
using CustomerManagementApi.Handlers.Customer.Delete;
using CustomerManagementApi.Handlers.Customer.Update;
using CustomerManagementApi.Services.Customer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Exceptions;

namespace UnitTests.Services;

[TestFixture]
public class CustomerServiceTests
{
    private Mock<ILogger<CustomerService>> _loggerMock;
    private DatabaseContext _context;
    private CustomerService _service;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new DatabaseContext(options);

        var now = DateTime.UtcNow;

        _context.Customers.Add(new()
        {
            Id = 1,
            Name = "Test1",
            Email = "Test1@test.com",
            CreatedAt = now,
            CreatedBy = "Test",
            CreatedById = 1,
            UpdatedAt = now,
            UpdatedBy = "Test",
            UpdatedById = 1
        });
        _context.Customers.Add(new()
        {
            Id = 2,
            Name = "Test2",
            Email = "Test2@test.com",
            CreatedAt = now,
            CreatedBy = "Test",
            CreatedById = 1,
            UpdatedAt = now,
            UpdatedBy = "Test",
            UpdatedById = 1,
            DeletedAt = now,
            DeletedBy = "Test",
            DeletedById = 1
        });
        _context.SaveChanges();

        _loggerMock = new Mock<ILogger<CustomerService>>();
        _service = new CustomerService(_loggerMock.Object, _context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [TestCase(1, true)]
    [TestCase(999, false)]
    public async Task Get_CustomerById_ReturnsExpectedResult(int customerId, bool exists)
    {
        var result = await _service.Get(customerId, CancellationToken.None);

        if (exists)
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Id, Is.EqualTo(customerId));
        }
        else
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.InstanceOf<NotFoundException>());
        }
    }
    
    [Test]
    public async Task Get_DeletedCustomer_ReturnsNotFound()
    {
        var result = await _service.Get(2, CancellationToken.None);

        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error, Is.InstanceOf<NotFoundException>());
    }

    [Test]
    public async Task Add_ValidRequest_ReturnsSuccessAndAddsCustomer()
    {
        var addRequest = new CustomerAddRequest
        {
            Name = "New Customer",
            Email = "newcustomer@test.com"
        };

        var result = await _service.Add(addRequest, CancellationToken.None);

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(await _context.Customers.AnyAsync(c => c.Email == "newcustomer@test.com"), Is.True);
    }
    
    [Test]
    public async Task Add_MultipleCustomers_AllAreAdded()
    {
        var addRequest1 = new CustomerAddRequest { Name = "A", Email = "newcustomer1@test.com" };
        var addRequest2 = new CustomerAddRequest { Name = "B", Email = "newcustomer2@test.com" };

        await _service.Add(addRequest1, CancellationToken.None);
        await _service.Add(addRequest2, CancellationToken.None);

        Assert.That(await _context.Customers.AnyAsync(t => t.Name == "A"), Is.True);
        Assert.That(await _context.Customers.AnyAsync(t => t.Name == "B"), Is.True);
        Assert.That(await _context.Customers.AnyAsync(t => t.Email == "newcustomer1@test.com"), Is.True);
        Assert.That(await _context.Customers.AnyAsync(t => t.Email == "newcustomer2@test.com"), Is.True);
    }

    [TestCase(1, "Updated Name", "updatedemail@test.com", true)]
    [TestCase(999, "Name", "email@test.com", false)]
    public async Task Update_Customer_ReturnsExpectedResult(int id, string name, string email, bool exists)
    {
        var updateRequest = new CustomerUpdateRequest
        {
            Id = id,
            Name = name,
            Email = email
        };

        var result = await _service.Update(updateRequest, CancellationToken.None);

        if (exists)
        {
            Assert.That(result.IsSuccess, Is.True);
            var updatedCustomer = await _context.Customers.FindAsync(id);
            Assert.That(updatedCustomer.Name, Is.EqualTo(name));
            Assert.That(updatedCustomer.Email, Is.EqualTo(email));
        }
        else
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.InstanceOf<NotFoundException>());
        }
    }

    [Test]
    public async Task Update_DeletedCustomer_ReturnsNotFound()
    {
        var updateRequest = new CustomerUpdateRequest
        {
            Id = 2,
            Name = "Name",
            Email = "email@test.com"
        };

        var result = await _service.Update(updateRequest, CancellationToken.None);

        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error, Is.InstanceOf<NotFoundException>());
    }

    [TestCase(1, true)]
    [TestCase(999, false)]
    public async Task Delete_Customer_ReturnsExpectedResult(int id, bool exists)
    {
        var deleteRequest = new CustomerDeleteRequest { Id = id };
        var result = await _service.Delete(deleteRequest, CancellationToken.None);

        if (exists)
        {
            Assert.That(result.IsSuccess, Is.True);
            var deletedCustomer = await _context.Customers.FindAsync(id);
            Assert.That(deletedCustomer.DeletedAt, Is.Not.Null);
        }
        else
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.InstanceOf<NotFoundException>());
        }
    }

    [Test]
    public async Task Delete_AlreadyDeletedCustomer_ReturnsNotFound()
    {
        var deleteRequest = new CustomerDeleteRequest { Id = 2 };
        var result = await _service.Delete(deleteRequest, CancellationToken.None);

        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error, Is.InstanceOf<NotFoundException>());
    }
}