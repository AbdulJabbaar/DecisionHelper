using ErrorOr;
using MediatR;

namespace DecisionHelper.Application.Adventure.Commands.Create
{
    public record CreateAdventureCommand(string Name) : IRequest<ErrorOr<AdventureResult>>;
}
