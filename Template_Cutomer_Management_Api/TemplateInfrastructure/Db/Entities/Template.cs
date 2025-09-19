namespace TemplateInfrastructure.Db.Entities;

public class Template : BaseEntity
{
    // Every entity has a state: Added, Modified, Deleted, Unchanged, or Detached
    public string Name { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}