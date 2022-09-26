using DecisionHelper.Application.Dependencies;
using DecisionHelper.Domain.Errors;
using ErrorOr;
using MediatR;

namespace DecisionHelper.Application.User.Queries.Get
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, ErrorOr<UserResult>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<UserResult>> Handle(GetUserQuery request, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (user is null)
            {
                return Errors.User.NotFound;
            }

            return new UserResult(user);
        }
    }
}
