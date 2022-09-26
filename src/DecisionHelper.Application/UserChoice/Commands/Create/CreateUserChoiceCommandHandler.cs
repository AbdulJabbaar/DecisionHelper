using DecisionHelper.Application.Dependencies;
using DecisionHelper.Domain.Errors;
using ErrorOr;
using MediatR;

namespace DecisionHelper.Application.UserChoice.Commands.Create
{
    public class CreateUserChoiceCommandHandler : IRequestHandler<CreateUserChoiceCommand, ErrorOr<UserChoiceResult>>
    {
        private readonly IUserChoiceRepository _userChoiceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        

        public CreateUserChoiceCommandHandler(IUserChoiceRepository userChoiceRepository, IUserRepository userRepository, IMessageRepository messageRepository)
        {
            _userChoiceRepository = userChoiceRepository;
            _userRepository = userRepository;
            _messageRepository = messageRepository;
        }

        public async Task<ErrorOr<UserChoiceResult>> Handle(CreateUserChoiceCommand request, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
            {
                return Errors.User.NotFound;
            }

            var message = await _messageRepository.GetByIdAsync(request.MessageId, cancellationToken);
            if (message is null)
            {
                return Errors.Message.NotFound;
            }

            var userAlreadyMadeChoice = await _userChoiceRepository.HasUserChoice(request.UserId, request.MessageId,
                message.AdventureId, cancellationToken);
            if (userAlreadyMadeChoice)
            {
                return Errors.UserChoice.DuplicateUserChoice;
            }

            var userChoice = await _userChoiceRepository.AddAsync(new Domain.Entities.UserChoice
            {
                Id = Guid.NewGuid(),
                MessageId = request.MessageId,
                UserId = request.UserId,
                Answer = request.Answer,
                AdventureId = message.AdventureId,
                AddedAt = DateTime.UtcNow
            });

            return new UserChoiceResult(userChoice);
        }
    }
}
