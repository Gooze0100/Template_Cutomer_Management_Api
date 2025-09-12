namespace CustomerManagementApi.Handlers.Customer.Update;

public class CustomerUpdateRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}