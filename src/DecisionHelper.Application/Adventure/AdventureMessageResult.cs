namespace DecisionHelper.Application.Adventure
{
    public record AdventureMessageResult(Domain.Entities.Adventure Adventure, IEnumerable<Domain.Entities.Message> Messages);
}
