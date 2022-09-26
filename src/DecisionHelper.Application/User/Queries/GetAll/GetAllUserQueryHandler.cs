using DecisionHelper.Application.Dependencies;
using MediatR;

namespace DecisionHelper.Application.User.Queries.GetAll
{
    public class GetAllUserQueryHandler :IRequestHandler<GetAllUserQuery, IEnumerable<UserResult>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserResult>> Handle(GetAllUserQuery request, CancellationToken cancellationToken = default)
        {
            var users = await _userRepository.GetAllAsync(cancellationToken);

            return users.Select(x=> new UserResult(x)).ToList();
        }
    }
}
