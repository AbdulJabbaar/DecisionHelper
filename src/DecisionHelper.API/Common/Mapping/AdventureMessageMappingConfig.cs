using DecisionHelper.Application.Adventure.Commands.CreateAdventureJourney;
using DecisionHelper.Contracts.Message;
using Mapster;

namespace DecisionHelper.API.Common.Mapping
{
    public class AdventureMessageMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Message, AdventureMessage>()
                .PreserveReference(true);
        }
    }
}
