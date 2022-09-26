using DecisionHelper.Contracts.Enums;

namespace DecisionHelper.Contracts.Message
{
    public class CreateMessage
    {
        public string Title { get; set; } = null!;
        public bool IsQuestion { get; set; }
        public Guid? ParentId { get; set; }
        public Answer? ByAnswer { get; set; }
    }
}
