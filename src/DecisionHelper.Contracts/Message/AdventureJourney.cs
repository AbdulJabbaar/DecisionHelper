using DecisionHelper.Contracts.Enums;

namespace DecisionHelper.Contracts.Message
{
    public class AdventureJourney
    {
        public Message Message { get; set; } = null!;
    }

    public class Message
    {
        public bool IsQuestion { get; set; }
        public string Title { get; set; } = null!;
        public Answer? ByAnswer { get; set; }
        public List<Message> Messages { get; set; }
    }
}
