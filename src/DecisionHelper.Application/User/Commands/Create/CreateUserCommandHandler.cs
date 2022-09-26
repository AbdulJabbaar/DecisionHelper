using DecisionHelper.Application.Dependencies;
using DecisionHelper.Domain.Errors;
using ErrorOr;
using MediatR;

namespace DecisionHelper.Application.User.Commands.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ErrorOr<UserResult>>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<UserResult>> Handle(CreateUserCommand request, CancellationToken cancellationToken = default)
        {
            var existingUser = await _userRepository.GetUserByEmail(request.Email, cancellationToken);
            if (existingUser is not null)
            {
                return Errors.User.DuplicateEmail;
            }
            var user = await _userRepository.AddAsync(new Domain.Entities.User
            {
                Id = Guid.NewGuid(),
                Name = request.Name, 
                Email = request.Email
            });

            return new UserResult(user);
        }
    }
}
