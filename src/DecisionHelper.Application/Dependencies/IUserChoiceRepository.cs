namespace DecisionHelper.Application.Dependencies
{
    public interface IUserChoiceRepository : IRepository<Domain.Entities.UserChoice>
    {
        Task<List<Domain.Entities.UserChoice>> GetUserChoices(Guid userId, CancellationToken cancellationToken = default);
        Task<bool> HasUserChoice(Guid userId, Guid adventureId, Guid messageId, CancellationToken cancellationToken = default);
    }
}
