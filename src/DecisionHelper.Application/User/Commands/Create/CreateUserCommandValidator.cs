using FluentValidation;

namespace DecisionHelper.Application.User.Commands.Create
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x=>x.Name).NotEmpty();
            RuleFor(x=>x.Email).NotEmpty().EmailAddress();
        }
    }
}
