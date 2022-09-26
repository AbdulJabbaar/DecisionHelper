using DecisionHelper.Application.Dependencies;
using DecisionHelper.Domain.Errors;
using ErrorOr;
using MediatR;

namespace DecisionHelper.Application.UserChoice.Queries.Get
{
    public class GetUserChoiceQueryHandler : IRequestHandler<GetUserChoiceQuery, ErrorOr.ErrorOr<IEnumerable<UserChoicesResult>>>
    {
        private readonly IUserChoiceRepository _userChoiceRepository;
        private readonly IUserRepository _userRepository;

        public GetUserChoiceQueryHandler(IUserChoiceRepository userChoiceRepository, IUserRepository userRepository)
        {
            _userChoiceRepository = userChoiceRepository;
            _userRepository = userRepository;
        }

        public async Task<ErrorOr<IEnumerable<UserChoicesResult>>> Handle(GetUserChoiceQuery request, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
            {
                return Errors.User.NotFound;
            }

            var userChoice = await _userChoiceRepository.GetUserChoices(request.UserId, cancellationToken);

            return userChoice.Select(x => 
                new UserChoicesResult(x.UserId, x.MessageId, x.Message.Title, x.Answer))
                .ToList();
        }
    }
}
