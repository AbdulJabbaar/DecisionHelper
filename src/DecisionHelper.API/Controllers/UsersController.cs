using DecisionHelper.API.Controllers.Base;
using DecisionHelper.Application.User.Commands.Create;
using DecisionHelper.Application.User.Queries.Get;
using DecisionHelper.Application.User.Queries.GetAll;
using DecisionHelper.Contracts.User;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DecisionHelper.API.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : ApiController
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;
        public UsersController(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUser request, CancellationToken cancellationToken = default)
        {
            var command = _mapper.Map<CreateUserCommand>(request);
            var userResult = await _sender.Send(command, cancellationToken);

            return userResult.Match(
                userResult => Ok(userResult),
                errors => Problem(errors));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var getAllUserQuery = new GetAllUserQuery();

            return Ok(await _sender.Send(getAllUserQuery, cancellationToken));
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById(Guid userId, CancellationToken cancellationToken = default)
        {
            var getUserQuery = new GetUserQuery(userId);
            var userResult = await _sender.Send(getUserQuery, cancellationToken);

            return userResult.Match(
                userResult => Ok(userResult),
                errors => Problem(errors));
        }

        
    }
}
