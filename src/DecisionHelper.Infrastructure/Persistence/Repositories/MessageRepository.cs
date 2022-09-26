using DecisionHelper.Application.Dependencies;
using DecisionHelper.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DecisionHelper.Infrastructure.Persistence.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        private readonly DecisionHelperDbContext _dbContext;
        public MessageRepository(DecisionHelperDbContext dnContext) : base(dnContext)
        {
            _dbContext = dnContext;
        }

        public async Task<Message> GetParentMessage(Guid messageId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Messages.Include(x => x.Parent).Where(x => x.Id == messageId && x.ParentId != null).Select(x=>x.Parent).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Message>> GetAllMessagesByAdventureId(Guid adventureId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Messages.Where(x => x.AdventureId == adventureId).Select(x=>new Message
            {
                AdventureId = x.AdventureId,
                Id = x.Id,
                Title = x.Title,
                ByAnswer = x.ByAnswer,
                IsQuestion = x.IsQuestion,
                ParentId = x.ParentId
            }).ToListAsync(cancellationToken);
        }

        public async Task<bool> IsAdventureHasMessages(Guid adventureId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Messages.Where(x => x.AdventureId == adventureId).AnyAsync(cancellationToken);
        }

        public async Task<Message> GetUserNextChoice(Guid userId, Guid adventureId, CancellationToken cancellationToken = default)
        {
            var userChoice = await _dbContext.UserChoices.OrderBy(c => c.AddedAt)
                .LastOrDefaultAsync(c => c.UserId == userId && c.AdventureId == adventureId, cancellationToken);
            
            if (userChoice != null)
            {
                return await _dbContext.Messages.FirstOrDefaultAsync(
                    m => m.ParentId == userChoice.MessageId && m.AdventureId == userChoice.AdventureId && m.ByAnswer == userChoice.Answer, cancellationToken);
            }
            else
            {
                return await _dbContext.Messages.FirstOrDefaultAsync(
                    m => m.AdventureId == adventureId && m.ParentId == null && m.IsQuestion, cancellationToken);
            }
        }
    }
}
