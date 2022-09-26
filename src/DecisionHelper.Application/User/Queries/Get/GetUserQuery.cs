using MediatR;

namespace DecisionHelper.Application.User.Queries.Get
{
    public record GetUserQuery(Guid UserId) : IRequest<ErrorOr.ErrorOr<UserResult>>;
}
