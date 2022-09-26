namespace DecisionHelper.Application.Dependencies
{
    public interface IAdventureRepository : IRepository<Domain.Entities.Adventure>
    {
        Task<Domain.Entities.Adventure> GetByName(string name, CancellationToken cancellationToken = default);
    }
}
