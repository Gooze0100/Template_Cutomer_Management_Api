using CustomerManagementApi.Handlers.Customer.Add;
using FluentValidation;

namespace CustomerManagementApi.Validators.Customer;

public class CustomerAddValidator : AbstractValidator<CustomerAddRequest>
{
    public CustomerAddValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required");

        RuleFor(x => x.Name)
            .MinimumLength(2)
            .WithMessage("Name is too short");
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required");
        
        RuleFor(x => x.Email)
            .MinimumLength(3)
            .WithMessage("Email is too short");
        
        RuleFor(x => x.Email)
            .Must(email => email.Contains('@'))
            .WithMessage("This is not email");
    }
}