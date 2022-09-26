using DecisionHelper.Application.Dependencies;
using DecisionHelper.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DecisionHelper.Infrastructure.Persistence.Repositories
{
    public class AdventureRepository : Repository<Adventure>, IAdventureRepository
    {
        private readonly DecisionHelperDbContext _dbContext;
        public AdventureRepository(DecisionHelperDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Adventure> GetByName(string name, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Adventures.Where(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
