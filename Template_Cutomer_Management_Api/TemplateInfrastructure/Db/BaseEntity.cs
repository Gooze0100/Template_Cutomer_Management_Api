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
    public DateTime? RemovedAt { get; set; }
    public string? RemovedBy { get; set; }
    public int? RemovedById { get; set; }
}