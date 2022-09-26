using DecisionHelper.Domain.Enums;

namespace DecisionHelper.Domain.Entities
{
    public class Message
    {
        public Guid Id { get; set; }
        public bool IsQuestion { get; set; }
        public string Title { get; set; } = null!;
        public Answer? ByAnswer { get; set; }
        public Guid? ParentId { get; set; }
        public Message? Parent { get; set; }
        public Guid AdventureId { get; set; }
        public Adventure Adventure { get; set; } = null!;
    }
}
