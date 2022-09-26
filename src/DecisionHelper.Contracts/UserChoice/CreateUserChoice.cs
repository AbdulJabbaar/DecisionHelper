using DecisionHelper.Contracts.Enums;

namespace DecisionHelper.Contracts.UserChoice
{
    public class CreateUserChoice
    {
        public Answer Answer { get; set; }
        public Guid MessageId { get; set; }
    }
}
