using ErrorOr;

namespace DecisionHelper.Domain.Errors
{
    public static partial class Errors
    {
        public static class User
        {
            public static Error DuplicateEmail =>
                Error.Conflict(code: "User.DuplicateEmail", description: "User email already exists");

            public static Error NotFound =>
                Error.NotFound(code: "User.NotFound", description: "User not exists against specified id");
        }
    }
}
