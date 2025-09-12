namespace TemplateManagementApi.Handlers.Template.Update;

public class TemplateUpdateRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}