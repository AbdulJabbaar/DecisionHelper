using DecisionHelper.Application.Dependencies;
using DecisionHelper.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DecisionHelper.Infrastructure.Persistence.Repositories
{
    public class UserChoiceRepository : Repository<UserChoice>, IUserChoiceRepository
    {
        private readonly DecisionHelperDbContext _dbContext;
        public UserChoiceRepository(DecisionHelperDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<UserChoice>> GetUserChoices(Guid userId, CancellationToken cancellationToken = default)
        {
            return _dbContext.UserChoices
                .Include(x=>x.Message)
                .Where(x => x.UserId == userId)
                .OrderBy(x=>x.AddedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> HasUserChoice(Guid userId, Guid adventureId, Guid messageId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.UserChoices.AnyAsync(
                x => x.UserId == userId && x.AdventureId == adventureId && x.MessageId == messageId, cancellationToken);
        }
    }
}
