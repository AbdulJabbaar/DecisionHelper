using ErrorOr;
using MediatR;

namespace DecisionHelper.Application.User.Commands.Create
{
    public record CreateUserCommand(string Name, string Email) : IRequest<ErrorOr<UserResult>>;
}
