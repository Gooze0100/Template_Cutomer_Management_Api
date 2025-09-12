using FluentValidation;
using TemplateManagementApi.Handlers.Template.Add;

namespace TemplateManagementApi.Validators.Template;

public class TemplateAddValidator : AbstractValidator<TemplateAddRequest> 
{
    public TemplateAddValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required");

        RuleFor(x => x.Name)
            .MinimumLength(2)
            .WithMessage("Name is too short");
        
        RuleFor(x => x.Subject)
            .NotEmpty()
            .WithMessage("Subject is required");
        
        RuleFor(x => x.Subject)
            .MinimumLength(5)
            .WithMessage("Subject is too short");
        
        RuleFor(x => x.Body)
            .NotEmpty()
            .WithMessage("Body is required");
        
        RuleFor(x => x.Body)
            .MinimumLength(20)
            .WithMessage("Body is too short");
    }
}