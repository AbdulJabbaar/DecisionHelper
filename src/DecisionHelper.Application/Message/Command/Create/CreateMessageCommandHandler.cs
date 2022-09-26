using DecisionHelper.Application.Dependencies;
using DecisionHelper.Domain.Errors;
using ErrorOr;
using MediatR;

namespace DecisionHelper.Application.Message.Command.Create
{
    public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, ErrorOr<MessageResult>>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IAdventureRepository _adventureRepository;

        public CreateMessageCommandHandler(IMessageRepository messageRepository, IAdventureRepository adventureRepository)
        {
            _messageRepository = messageRepository;
            _adventureRepository = adventureRepository;
        }

        public async Task<ErrorOr<MessageResult>> Handle(CreateMessageCommand request, CancellationToken cancellationToken = default)
        {
            var adventure = await _adventureRepository.GetByIdAsync(request.AdventureId, cancellationToken);
            if (adventure is null)
            {
                return Errors.Adventure.NotFound;
            }
            if (request.ParentId is not null)
            {
                var parentMessage = await _messageRepository.GetByIdAsync(request.ParentId.Value, cancellationToken);
                if (parentMessage is null)
                {
                    return Errors.Message.InvalidParentMessageId;
                }
            }

            var message = await _messageRepository.AddAsync(new Domain.Entities.Message
            {
                Id = Guid.NewGuid(),
                AdventureId = request.AdventureId,
                ParentId = request.ParentId,
                ByAnswer = request.ByAnswer,
                IsQuestion = request.IsQuestion,
                Title = request.Title
            });

            return new MessageResult(message);
        }
    }
}
