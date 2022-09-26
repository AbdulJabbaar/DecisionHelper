namespace DecisionHelper.Application.Dependencies
{
    public interface IRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<T> AddAsync(T entity);
    }
}
