using DecisionHelper.Domain.Enums;
using MediatR;

namespace DecisionHelper.Application.Adventure.Commands.CreateAdventureJourney
{
    public record CreateAdventureJourneyCommand(Guid AdventureId, AdventureMessage Message) : IRequest<ErrorOr.ErrorOr<AdventureMessageResult>>;

    public class AdventureMessage
    {
        public bool IsQuestion { get; set; }
        public string Title { get; set; } = null!;
        public Answer? ByAnswer { get; set; }
        public List<AdventureMessage> Messages { get; set; }
    }
}
