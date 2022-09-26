using DecisionHelper.Domain.Enums;

namespace DecisionHelper.Domain.Entities
{
    public class UserChoice
    {
        public Guid Id { get; set; }
        public Answer Answer { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid MessageId { get; set; }
        public Message? Message { get; set; }
        public Guid AdventureId { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
