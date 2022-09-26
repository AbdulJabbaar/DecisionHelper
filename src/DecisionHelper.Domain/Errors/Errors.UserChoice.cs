using ErrorOr;

namespace DecisionHelper.Domain.Errors
{
    public static partial class Errors
    {
        public static class UserChoice
        {
            public static Error DuplicateUserChoice =>
                Error.Conflict(code: "User.DuplicateUserChoice", description: "User choice for the selected adventure already exists");
        }
    }
}
