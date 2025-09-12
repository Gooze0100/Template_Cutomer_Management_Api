using CustomerManagementApi.Services.Customer;
using CustomerManagementApi.Validators.Customer;
using FluentValidation;

namespace CustomerManagementApi.Startup;

public static class Dependencies
{
    public static void AddDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ICustomerService, CustomerService>();

        builder.Services
            .AddValidatorsFromAssemblyContaining<CustomerAddValidator>()
            .AddValidatorsFromAssemblyContaining<CustomerUpdateValidator>()
            .AddValidatorsFromAssemblyContaining<CustomerDeleteValidator>();
    }
}