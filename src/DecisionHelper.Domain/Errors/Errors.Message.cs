using ErrorOr;

namespace DecisionHelper.Domain.Errors
{
    public static partial class Errors
    {
        public static class Message
        {
            public static Error NotFound =>
                Error.NotFound(code: "Message.NotFound", description: "Message not exists against specified id");
            public static Error InvalidParentMessageId =>
                Error.NotFound(code: "Message.InvalidParentMessageId", description: "Invalid message parent id");
        }
    }
}
