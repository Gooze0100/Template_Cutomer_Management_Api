namespace TemplateManagementApi.Settings;

public class ConnectionStrings
{
    public static string SectionName => "ConnectionStrings";
    
    public string DefaultConnection { get; set; }
}