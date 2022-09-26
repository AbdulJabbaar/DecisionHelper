using ErrorOr;
using MediatR;

namespace DecisionHelper.Application.Message.Queries.UserNextMessage
{
    public record GetUserNextMessageQuery(Guid adventureId, Guid userId) : IRequest<ErrorOr<MessageResult>>;
}
