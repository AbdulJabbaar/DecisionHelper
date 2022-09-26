using DecisionHelper.API.Controllers.Base;
using DecisionHelper.Application.Adventure.Commands.Create;
using DecisionHelper.Application.Adventure.Queries.GeteAll;
using DecisionHelper.Contracts.Adventure;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DecisionHelper.API.Controllers
{
    [Route("api/[controller]")]
    public class AdventuresController : ApiController
    {
        private readonly ISender _sender;

        public AdventuresController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdventure request, CancellationToken cancellationToken = default)
        {
            var command = new CreateAdventureCommand(request.Name);
            var adventureResult = await _sender.Send(command, cancellationToken);

            return adventureResult.Match(
                adventureResult => Ok(adventureResult),
                errors => Problem(errors));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var query = new GetAllAdventureQuery();

            return Ok(await _sender.Send(query, cancellationToken));
        }
    }
}
