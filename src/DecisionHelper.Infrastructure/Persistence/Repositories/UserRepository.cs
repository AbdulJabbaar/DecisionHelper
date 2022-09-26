using DecisionHelper.Application.Dependencies;
using DecisionHelper.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DecisionHelper.Infrastructure.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly DecisionHelperDbContext _dbContext;
        public UserRepository(DecisionHelperDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUserByEmail(string email, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.Where(u => u.Email == email).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
