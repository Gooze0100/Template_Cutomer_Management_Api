using CustomerManagementApi.Handlers.Customer.Update;
using FluentValidation;

namespace CustomerManagementApi.Validators.Customer;

public class CustomerUpdateValidator : AbstractValidator<CustomerUpdateRequest>
{
    public CustomerUpdateValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");
        
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0");
        
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