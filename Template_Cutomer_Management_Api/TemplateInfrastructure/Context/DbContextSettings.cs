namespace TemplateInfrastructure.Context;

public class DbContextSettings
{
    public ConnectionStringSettings ConnectionStrings { get; set; }
}

public class ConnectionStringSettings
{
    public string DefaultConnectionString { get; set; }
}