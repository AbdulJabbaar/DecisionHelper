using DecisionHelper.Application.Dependencies;
using DecisionHelper.Domain.Errors;
using ErrorOr;
using MediatR;

namespace DecisionHelper.Application.Adventure.Commands.Create
{
    public class CreateAdventureCommandHandler : IRequestHandler<CreateAdventureCommand, ErrorOr<AdventureResult>>
    {
        private readonly IAdventureRepository _adventureRepository;

        public CreateAdventureCommandHandler(IAdventureRepository adventureRepository)
        {
            _adventureRepository = adventureRepository;
        }

        public async Task<ErrorOr<AdventureResult>> Handle(CreateAdventureCommand request, CancellationToken cancellationToken = default)
        {
            var existingAdventure = await _adventureRepository.GetByName(request.Name, cancellationToken);
            if (existingAdventure is not null)
            {
                return Errors.Adventure.DuplicateName;
            }

            var adventure = await _adventureRepository.AddAsync(
                new Domain.Entities.Adventure
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name
                });
            return new AdventureResult(adventure);
        }
    }
}
