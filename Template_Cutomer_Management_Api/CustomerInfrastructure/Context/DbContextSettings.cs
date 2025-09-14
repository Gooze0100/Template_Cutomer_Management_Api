namespace CustomerInfrastructure.Context;

public class DbContextSettings
{
    public ConnectionStringSettings ConnectionStrings { get; set; }
}

public class ConnectionStringSettings
{
    public required string DefaultConnection { get; set; }
}