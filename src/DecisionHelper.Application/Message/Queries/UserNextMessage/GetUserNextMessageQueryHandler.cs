using DecisionHelper.Application.Dependencies;
using DecisionHelper.Domain.Errors;
using ErrorOr;
using MediatR;

namespace DecisionHelper.Application.Message.Queries.UserNextMessage
{
    public class GetUserNextMessageQueryHandler : IRequestHandler<GetUserNextMessageQuery, ErrorOr<MessageResult>>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAdventureRepository _adventureRepository;

        public GetUserNextMessageQueryHandler(IMessageRepository messageRepository, IUserRepository userRepository, IAdventureRepository adventureRepository)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _adventureRepository = adventureRepository;
        }

        public async Task<ErrorOr<MessageResult>> Handle(GetUserNextMessageQuery request, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(request.userId, cancellationToken);
            if (user is null)
            {
                return Errors.User.NotFound;
            }

            var adventure = await _adventureRepository.GetByIdAsync(request.adventureId, cancellationToken);
            if (adventure is null)
            {
                return Errors.Adventure.NotFound;
            }

            var nextMessage =
                await _messageRepository.GetUserNextChoice(request.userId, request.adventureId, cancellationToken);
            return new MessageResult(nextMessage);
        }
    }
}
