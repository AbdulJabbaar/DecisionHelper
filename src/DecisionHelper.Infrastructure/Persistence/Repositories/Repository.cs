using DecisionHelper.Application.Dependencies;
using Microsoft.EntityFrameworkCore;

namespace DecisionHelper.Infrastructure.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DecisionHelperDbContext _dbContext;

        public Repository(DecisionHelperDbContext dnContext)
        {
            _dbContext = dnContext;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
