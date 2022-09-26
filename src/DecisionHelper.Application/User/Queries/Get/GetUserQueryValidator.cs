using FluentValidation;

namespace DecisionHelper.Application.User.Queries.Get
{
    public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
