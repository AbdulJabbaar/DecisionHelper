using DecisionHelper.Application.User;
using DecisionHelper.Application.User.Commands.Create;
using ErrorOr;
using FluentValidation;
using MediatR;

namespace DecisionHelper.Application.Common.Behaviors
{
    // TODO: Need to remove this - not in USE
    public class ValidateCreateUserCommandBehavior : IPipelineBehavior<CreateUserCommand, ErrorOr.ErrorOr<UserResult>>
    {
        private readonly IValidator<CreateUserCommand> _validator;

        public ValidateCreateUserCommandBehavior(IValidator<CreateUserCommand> validator)
        {
            _validator = validator;
        }

        public async Task<ErrorOr<UserResult>> Handle(
            CreateUserCommand request, 
            CancellationToken cancellationToken, 
            RequestHandlerDelegate<ErrorOr<UserResult>> next)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsValid)
            {
                return await next();
            }

            var errors = validationResult.Errors.Select(validationFailure =>
                    Error.Validation(validationFailure.PropertyName, validationFailure.ErrorMessage))
                .ToList();
            return errors;
        }
    }
}
