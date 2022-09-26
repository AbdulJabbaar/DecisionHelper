using MediatR;

namespace DecisionHelper.Application.UserChoice.Queries.Get
{
    public record GetUserChoiceQuery(Guid UserId) : IRequest<ErrorOr.ErrorOr<IEnumerable<UserChoicesResult>>>;
}
