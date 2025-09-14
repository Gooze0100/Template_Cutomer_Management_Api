using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;
using TemplateInfrastructure.Context;
using TemplateManagementApi.Handlers.Template.Add;
using TemplateManagementApi.Handlers.Template.Delete;
using TemplateManagementApi.Handlers.Template.Update;
using TemplateManagementApi.Services.Template;
using Entities = TemplateInfrastructure.Db.Entities;

namespace UnitTests.Services;

[TestFixture]
public class TemplateServiceTests
{
    private Mock<ILogger<TemplateService>> _logger;
    private DatabaseContext _context;
    private TemplateService _service;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new DatabaseContext(options);

        var now = DateTime.UtcNow;

        _context.Templates.Add(new Entities.Template()
        {
            Id = 1,
            Name = "Template1",
            Subject = "Subject1",
            Body = "Body1 and this should be quite long as this one ",
            CreatedAt = now,
            CreatedBy = "Test",
            CreatedById = 1,
            UpdatedAt = now,
            UpdatedBy = "Test",
            UpdatedById = 1
        });
        _context.Templates.Add(new Entities.Template()
        {
            Id = 2,
            Name = "Template2",
            Subject = "Subject2",
            Body = "Body1 and this should be quite long as this one ",
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

        _logger = new Mock<ILogger<TemplateService>>();
        _service = new TemplateService(_logger.Object, _context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [TestCase(1, true)]
    [TestCase(999, false)]
    public async Task Get_TemplateById_ReturnsExpectedResult(int templateId, bool exists)
    {
        var result = await _service.Get(templateId, CancellationToken.None);

        if (exists)
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Id, Is.EqualTo(templateId));
        }
        else
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.InstanceOf<NotFoundException>());
        }
    }
    
    [Test]
    public async Task Get_DeletedTemplate_ReturnsNotFound()
    {
        var result = await _service.Get(2, CancellationToken.None);

        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error, Is.InstanceOf<NotFoundException>());
    }

    [Test]
    public async Task Add_ValidRequest_ReturnsSuccessAndAddsTemplate()
    {
        var addRequest = new TemplateAddRequest
        {
            Name = "New Template",
            Subject = "New Subject",
            Body = "New Body this should be quite long as this one, or more",
        };

        var result = await _service.Add(addRequest, CancellationToken.None);

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(await _context.Templates.AnyAsync(t => t.Name == "New Template"), Is.True);
    }
    
    [Test]
    public async Task Add_MultipleTemplates_AllAreAdded()
    {
        var addRequest1 = new TemplateAddRequest { Name = "A", Subject = "S1", Body = "B1" };
        var addRequest2 = new TemplateAddRequest { Name = "B", Subject = "S2", Body = "B2" };

        await _service.Add(addRequest1, CancellationToken.None);
        await _service.Add(addRequest2, CancellationToken.None);

        Assert.That(await _context.Templates.AnyAsync(t => t.Name == "A"), Is.True);
        Assert.That(await _context.Templates.AnyAsync(t => t.Subject == "S1"), Is.True);
        Assert.That(await _context.Templates.AnyAsync(t => t.Body == "B1"), Is.True);
        Assert.That(await _context.Templates.AnyAsync(t => t.Name == "B"), Is.True);
        Assert.That(await _context.Templates.AnyAsync(t => t.Subject == "S2"), Is.True);
        Assert.That(await _context.Templates.AnyAsync(t => t.Body == "B2"), Is.True);
    }

    [TestCase(1, "Updated Name", "Updated Subject", "Updated Body", true)]
    [TestCase(999, "Name", "Subject", "Body", false)]
    public async Task Update_Template_ReturnsExpectedResult(int id, string name, string subject, string body,
        bool exists)
    {
        var updateRequest = new TemplateUpdateRequest
        {
            Id = id,
            Name = name,
            Subject = subject,
            Body = body
        };

        var result = await _service.Update(updateRequest, CancellationToken.None);

        if (exists)
        {
            Assert.That(result.IsSuccess, Is.True);
            var updatedTemplate = await _context.Templates.FindAsync(id);
            Assert.That(updatedTemplate.Name, Is.EqualTo(name));
            Assert.That(updatedTemplate.Subject, Is.EqualTo(subject));
            Assert.That(updatedTemplate.Body, Is.EqualTo(body));
        }
        else
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.InstanceOf<NotFoundException>());
        }
    }
    
    [Test]
    public async Task Update_DeletedTemplate_ReturnsNotFound()
    {
        var updateRequest = new TemplateUpdateRequest
        {
            Id = 2,
            Name = "Name",
            Subject = "Subject",
            Body = "Body"
        };

        var result = await _service.Update(updateRequest, CancellationToken.None);

        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error, Is.InstanceOf<NotFoundException>());
    }

    [TestCase(1, true)]
    [TestCase(999, false)]
    public async Task Delete_Template_ReturnsExpectedResult(int id, bool exists)
    {
        var deleteRequest = new TemplateDeleteRequest { Id = id };
        var result = await _service.Delete(deleteRequest, CancellationToken.None);

        if (exists)
        {
            Assert.That(result.IsSuccess, Is.True);
            var deletedTemplate = await _context.Templates.FindAsync(id);
            Assert.That(deletedTemplate.DeletedAt, Is.Not.Null);
        }
        else
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.InstanceOf<NotFoundException>());
        }
    }
    
    [Test]
    public async Task Delete_AlreadyDeletedTemplate_ReturnsNotFound()
    {
        var deleteRequest = new TemplateDeleteRequest { Id = 2 };

        var result = await _service.Delete(deleteRequest, CancellationToken.None);

        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Error, Is.InstanceOf<NotFoundException>());
    }
}