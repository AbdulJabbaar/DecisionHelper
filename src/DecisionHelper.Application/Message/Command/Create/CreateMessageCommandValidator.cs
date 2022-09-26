using FluentValidation;

namespace DecisionHelper.Application.Message.Command.Create
{
    public class CreateMessageCommandValidator : AbstractValidator<CreateMessageCommand>
    {
        public CreateMessageCommandValidator()
        {
            RuleFor(x=>x.Title).NotEmpty();
            RuleFor(x => x.AdventureId).NotEmpty();
        }
    }
}
