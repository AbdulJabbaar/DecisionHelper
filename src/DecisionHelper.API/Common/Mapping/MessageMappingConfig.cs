using DecisionHelper.Application.Message.Command.Create;
using DecisionHelper.Contracts.Message;
using Mapster;

namespace DecisionHelper.API.Common.Mapping
{
    public class MessageMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateMessage, CreateMessageCommand>()
                .Map(dest => dest.ByAnswer, src => src.ByAnswer)
                .Map(dest => dest.IsQuestion, src => src.IsQuestion)
                .Map(dest => dest.ParentId, src => src.ParentId)
                .Map(dest => dest.Title, src => src.Title);
        }
    }
}
