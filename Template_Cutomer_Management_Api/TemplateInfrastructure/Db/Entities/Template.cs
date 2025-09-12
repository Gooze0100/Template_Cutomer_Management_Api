namespace TemplateInfrastructure.Db.Entities;

public class Template : BaseEntity
{
    public string Name { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}