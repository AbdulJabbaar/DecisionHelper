using MediatR;

namespace DecisionHelper.Application.Adventure.Queries.GeteAll
{
    public record GetAllAdventureQuery() : IRequest<IEnumerable<AdventureResult>>;
}
