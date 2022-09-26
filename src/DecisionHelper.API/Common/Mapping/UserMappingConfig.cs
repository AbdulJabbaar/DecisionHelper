using DecisionHelper.Application.User.Commands.Create;
using DecisionHelper.Contracts.User;
using Mapster;

namespace DecisionHelper.API.Common.Mapping
{
    public class UserMappingConfig :IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateUser, CreateUserCommand>()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Email, src => src.Email);
        }
    }
}
