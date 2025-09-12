using FluentValidation;
using TemplateManagementApi.Handlers.Template.Delete;

namespace TemplateManagementApi.Validators.Template;

public class TemplateDeleteValidator :  AbstractValidator<TemplateDeleteRequest>
{
    public TemplateDeleteValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");
        
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0");
    }
}