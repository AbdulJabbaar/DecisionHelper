using DecisionHelper.Application.Dependencies;
using MediatR;

namespace DecisionHelper.Application.Adventure.Queries.GeteAll
{
    public class GetAllAdventureQueryHandler : IRequestHandler<GetAllAdventureQuery, IEnumerable<AdventureResult>>
    {
        private readonly IAdventureRepository _adventureRepository;

        public GetAllAdventureQueryHandler(IAdventureRepository adventureRepository)
        {
            _adventureRepository = adventureRepository;
        }

        public async Task<IEnumerable<AdventureResult>> Handle(GetAllAdventureQuery request, CancellationToken cancellationToken = default)
        {
            var adventures = await _adventureRepository.GetAllAsync(cancellationToken);

            return adventures.Select(x => new AdventureResult(x));
        }
    }
}
