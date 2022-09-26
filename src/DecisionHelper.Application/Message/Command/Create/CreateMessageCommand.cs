using DecisionHelper.Domain.Enums;
using MediatR;

namespace DecisionHelper.Application.Message.Command.Create
{
    public record CreateMessageCommand(string Title, bool IsQuestion, Guid AdventureId, Guid? ParentId, Answer? ByAnswer) : IRequest<ErrorOr.ErrorOr<MessageResult>>;
}