namespace TemplateInfrastructure.Db;

public class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public int CreatedById { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public int UpdatedById { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
    public int? DeletedById { get; set; }
    
    public void MarkAsCreated(string userName = "SYSTEM", int userId = 1)
    {
        DateTime date = DateTime.UtcNow;
        
        CreatedAt = date;
        CreatedBy = userName;
        CreatedById = userId;
        UpdatedAt = date;
        UpdatedBy = userName;
        UpdatedById = userId;
    }

    public void MarkAsUpdated(string userName = "SYSTEM", int userId = 1)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = userName;
        UpdatedById = userId;
    }
    
    public void MarkAsDeleted(string userName = "SYSTEM", int userId = 1)
    {
        DeletedAt = DateTime.UtcNow;
        DeletedBy = userName;
        DeletedById = userId;
    }
}