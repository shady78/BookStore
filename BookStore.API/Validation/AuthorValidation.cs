using FluentValidation;

namespace BookStore.API.Validation
{
    public class CreateAuthorRequestValidator 
        : AbstractValidator<CreateAuthorRequest>
    {
        public CreateAuthorRequestValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .WithMessage("Author name is required.")
                .Length(2, 150)
                .WithMessage("minumum length 2 characters , maximum length 150 characters.");

            RuleFor(a => a.Bio)
                .MaximumLength(1000)
                .WithMessage("Bio can not exceed 1000 characters.");
        }
    }
    public class UpdateAuthorRequestValidator 
        : AbstractValidator<UpdateAuthorRequest>
    {
        public UpdateAuthorRequestValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .WithMessage("Author name is required.")
                .Length(2, 150)
                .WithMessage("minumum length 2 characters , maximum length 150 characters.");

            RuleFor(a => a.Bio)
                .MaximumLength(1000)
                .WithMessage("Bio can not exceed 1000 characters.");
        }
    }
}
