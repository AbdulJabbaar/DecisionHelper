using FluentValidation;

namespace DecisionHelper.Application.Adventure.Commands.CreateAdventureJourney
{
    public class CreateAdventureJourneyCommandValidator : AbstractValidator<CreateAdventureJourneyCommand>
    {
        public CreateAdventureJourneyCommandValidator()
        {
            RuleFor(x=>x.Message).NotEmpty();
            RuleFor(x=>x.AdventureId).NotEmpty();
            RuleFor(x => x.Message.Title).NotEmpty();
        }
    }
}
