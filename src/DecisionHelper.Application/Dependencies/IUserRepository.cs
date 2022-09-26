namespace DecisionHelper.Application.Dependencies
{
    public interface IUserRepository : IRepository<Domain.Entities.User>
    {
        Task<Domain.Entities.User> GetUserByEmail(string email, CancellationToken cancellationToken = default);
    }
}
