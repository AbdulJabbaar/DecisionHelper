using DecisionHelper.Application.Dependencies;
using DecisionHelper.Domain.Errors;
using ErrorOr;
using MediatR;

namespace DecisionHelper.Application.Adventure.Commands.CreateAdventureJourney
{
    public class CreateAdventureJourneyCommandHandler : IRequestHandler<CreateAdventureJourneyCommand, ErrorOr.ErrorOr<AdventureMessageResult>>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IAdventureRepository _adventureRepository;

        public CreateAdventureJourneyCommandHandler(IMessageRepository messageRepository, IAdventureRepository adventureRepository)
        {
            _messageRepository = messageRepository;
            _adventureRepository = adventureRepository;
        }

        public async Task<ErrorOr<AdventureMessageResult>> Handle(CreateAdventureJourneyCommand request, CancellationToken cancellationToken = default)
        {
            var adventure = await _adventureRepository.GetByIdAsync(request.AdventureId, cancellationToken);
            if (adventure is null)
            {
                return Errors.Adventure.NotFound;
            }

            var adventureHasMessages = await _messageRepository.IsAdventureHasMessages(request.AdventureId, cancellationToken);
            if (adventureHasMessages)
            {
                return Errors.Adventure.JourneyAlreadyExists;
            }

            var lstMessages = GetAdventureMessages(request.AdventureId, request.Message);
            foreach (var message in lstMessages)
            {
                await _messageRepository.AddAsync(message);
            }

            var adventureMessages =
                await _messageRepository.GetAllMessagesByAdventureId(request.AdventureId, cancellationToken);

            return new AdventureMessageResult(adventure, adventureMessages);
        }


        private List<Domain.Entities.Message> GetAdventureMessages(Guid adventureId, AdventureMessage adventureMessage)
        {
            var messageId = Guid.NewGuid();
            var lstMessages = new List<Domain.Entities.Message>
            {
                new Domain.Entities.Message
                {
                    Id = messageId,
                    IsQuestion = adventureMessage.IsQuestion,
                    AdventureId = adventureId,
                    ByAnswer = adventureMessage.ByAnswer,
                    ParentId = null,
                    Title = adventureMessage.Title
                }
            };

            var messages = adventureMessage.Messages;
            if (messages is not null && messages.Count > 0)
            {
                HandleChild(adventureId, messageId, adventureMessage.Messages, lstMessages);
            }
            return lstMessages;
        }

        private void HandleChild(Guid adventureId, Guid parentId, List<AdventureMessage> adventureMessage, List<Domain.Entities.Message> orignalList)
        {
            foreach (var message in adventureMessage)
            {
                var messageId = Guid.NewGuid();
                var msg = new Domain.Entities.Message
                {
                    Id = messageId,
                    AdventureId = adventureId,
                    Title = message.Title,
                    ByAnswer = message.ByAnswer,
                    IsQuestion = message.IsQuestion,
                    ParentId = parentId
                };
                orignalList.Add(msg);

                var messages = message.Messages;
                if (messages is not null && messages.Count > 0)
                {
                    HandleChild(adventureId, messageId, message.Messages, orignalList);
                }
            }
        }
    }
}
