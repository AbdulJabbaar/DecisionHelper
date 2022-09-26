using DecisionHelper.API.Controllers.Base;
using DecisionHelper.Application.UserChoice.Commands.Create;
using DecisionHelper.Application.UserChoice.Queries.Get;
using DecisionHelper.Contracts.UserChoice;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DecisionHelper.API.Controllers
{
    [Route("api/user/{userId}/[controller]")]
    public class UserChoicesController : ApiController
    {
        private readonly ISender _sender;

        public UserChoicesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserChoices(Guid userId, CancellationToken cancellationToken = default)
        {
            var query = new GetUserChoiceQuery(userId);
            var userChoiceResult = await _sender.Send(query, cancellationToken);

            return userChoiceResult.Match(
                userChoiceResult => Ok(userChoiceResult),
                errors => Problem(errors));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserChoice(Guid userId, CreateUserChoice request, CancellationToken cancellationToken = default)
        {
            var command = new CreateUserChoiceCommand(userId, request.MessageId, (Domain.Enums.Answer)request.Answer);
            var createUserChoice = await _sender.Send(command, cancellationToken);

            return createUserChoice.Match(
                createUserChoice => Ok(createUserChoice),
                errors => Problem(errors));
        }
    }
}
