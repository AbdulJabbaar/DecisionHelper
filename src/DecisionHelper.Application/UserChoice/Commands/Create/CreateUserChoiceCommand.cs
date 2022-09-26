using DecisionHelper.Domain.Enums;
using ErrorOr;
using MediatR;

namespace DecisionHelper.Application.UserChoice.Commands.Create
{
    public record CreateUserChoiceCommand(Guid UserId, Guid MessageId, Answer Answer) : IRequest<ErrorOr<UserChoiceResult>>;
}
