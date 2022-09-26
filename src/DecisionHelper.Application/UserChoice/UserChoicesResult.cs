using DecisionHelper.Domain.Enums;

namespace DecisionHelper.Application.UserChoice
{
    public record UserChoicesResult(Guid userId, Guid MessageId, string Title, Answer Answer);
}
