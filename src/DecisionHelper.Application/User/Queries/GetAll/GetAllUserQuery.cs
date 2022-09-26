using MediatR;

namespace DecisionHelper.Application.User.Queries.GetAll
{
    public record GetAllUserQuery() : IRequest<IEnumerable<UserResult>>;
}
