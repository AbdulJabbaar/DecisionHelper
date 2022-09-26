using FluentValidation;

namespace DecisionHelper.Application.UserChoice.Commands.Create
{
    public class CreateUserChoiceCommandValidator : AbstractValidator<CreateUserChoiceCommand>
    {
        public CreateUserChoiceCommandValidator()
        {
            RuleFor(x=>x.Answer).NotEmpty();
            RuleFor(x=>x.MessageId).NotEmpty();
            RuleFor(x=>x.UserId).NotEmpty();
        }
    }
}
