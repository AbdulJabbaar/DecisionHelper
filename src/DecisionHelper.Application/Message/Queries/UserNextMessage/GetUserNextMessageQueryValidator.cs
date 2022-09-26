using FluentValidation;

namespace DecisionHelper.Application.Message.Queries.UserNextMessage
{
    public class GetUserNextMessageQueryValidator:AbstractValidator<GetUserNextMessageQuery>
    {
        public GetUserNextMessageQueryValidator()
        {
            RuleFor(x => x.userId).NotEmpty();
            RuleFor(c => c.adventureId).NotEmpty();
        }
    }
}
