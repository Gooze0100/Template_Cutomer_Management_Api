using FluentValidation;
using TemplateManagementApi.Handlers.Template.Update;

namespace TemplateManagementApi.Validators.Template;

public class TemplateUpdateValidator : AbstractValidator<TemplateUpdateRequest>
{
    public TemplateUpdateValidator()
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