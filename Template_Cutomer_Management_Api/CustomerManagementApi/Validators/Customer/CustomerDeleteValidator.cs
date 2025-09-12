using CustomerManagementApi.Handlers.Customer.Delete;
using FluentValidation;

namespace CustomerManagementApi.Validators.Customer;

public class CustomerDeleteValidator : AbstractValidator<CustomerDeleteRequest>
{
    public CustomerDeleteValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");
        
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0");
    }
}