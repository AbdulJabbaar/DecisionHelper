using DecisionHelper.API.Controllers.Base;
using DecisionHelper.Application.Adventure.Commands.CreateAdventureJourney;
using DecisionHelper.Application.Message.Command.Create;
using DecisionHelper.Application.Message.Queries.UserNextMessage;
using DecisionHelper.Contracts.Message;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DecisionHelper.API.Controllers
{
    [Route("api/adventure/{adventureId}/[controller]")]
    public class AdventureMessagesController : ApiController
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public AdventureMessagesController(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(Guid adventureId, CreateMessage request, CancellationToken cancellationToken = default)
        {
            var command = new CreateMessageCommand(request.Title, request.IsQuestion, adventureId, request.ParentId, ((Domain.Enums.Answer)request.ByAnswer)!);
            var messageResult = await _sender.Send(command, cancellationToken);

            return messageResult.Match(
                messageResult => Ok(messageResult),
                errors => Problem(errors));
        }

        [HttpPost("journey")]
        public async Task<IActionResult> CreateAdventureJourney(Guid adventureId, AdventureJourney request,
            CancellationToken cancellationToken = default)
        {
            var command =
                new CreateAdventureJourneyCommand(adventureId, _mapper.Map<AdventureMessage>(request.Message));
            var adventureJourneyResult = await _sender.Send(command, cancellationToken);

            return adventureJourneyResult.Match(
                adventureJourneyResult => Ok(adventureJourneyResult),
                errors => Problem(errors));
        }

        [HttpGet("user/{userId}/nextmessage")]
        public async Task<IActionResult> GetUserNextMessage(Guid adventureId, Guid userId, CancellationToken cancellationToken = default)
        {
            var query = new GetUserNextMessageQuery(adventureId, userId);
            var nextMessageResult = await _sender.Send(query, cancellationToken);

            return nextMessageResult.Match(
                nextMessageResult => Ok(nextMessageResult),
                errors => Problem(errors));
        }
    }
}
