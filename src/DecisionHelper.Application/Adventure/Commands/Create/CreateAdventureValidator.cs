using FluentValidation;

namespace DecisionHelper.Application.Adventure.Commands.Create
{
    public class CreateAdventureValidator : AbstractValidator<CreateAdventureCommand>
    {
        public CreateAdventureValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
