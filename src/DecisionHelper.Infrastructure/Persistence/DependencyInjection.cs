using DecisionHelper.Application.Dependencies;
using DecisionHelper.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DecisionHelper.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DecisionHelperDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(nameof(DecisionHelperDbContext))));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAdventureRepository, AdventureRepository>();
            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddTransient<IUserChoiceRepository, UserChoiceRepository>();

            return services;
        }
    }
}
