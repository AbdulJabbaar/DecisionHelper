namespace DecisionHelper.Application.Dependencies
{
    public interface IMessageRepository : IRepository<Domain.Entities.Message>
    {
        Task<Domain.Entities.Message> GetParentMessage(Guid messageId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Domain.Entities.Message>> GetAllMessagesByAdventureId(Guid adventureId, CancellationToken cancellationToken = default);
        Task<bool> IsAdventureHasMessages(Guid adventureId, CancellationToken cancellationToken = default);
        Task<Domain.Entities.Message> GetUserNextChoice(Guid userId, Guid adventureId, CancellationToken cancellationToken = default);
    }
}
