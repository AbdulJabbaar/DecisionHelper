using ErrorOr;

namespace DecisionHelper.Domain.Errors
{
    public static partial class Errors
    {
        public static class Adventure
        {
            public static Error DuplicateName =>
                Error.Conflict(code: "Adventure.DuplicateName", description: "Adventure name already exists");
            public static Error JourneyAlreadyExists =>
                Error.Conflict(code: "Adventure.JourneyAlreadyExists", description: "Adventure already has Journey defined");
            public static Error NotFound =>
                Error.NotFound(code: "Adventure.InvalidAdventureId", description: "Adventure not exists against specified id");
        }
    }
}
