using DecisionHelper.API.Common.Errors;
using DecisionHelper.API.Common.Mapping;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;

namespace DecisionHelper.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            // Add services to the container.
            services.AddControllers();
            services.AddTransient<ProblemDetailsFactory, DecisionHelperProblemDetailsFactory>();
            
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Decision Helper API", Version = "v1" });
                c.CustomSchemaIds(x => x.ToString());
            });
            services.AddMappings();

            return services;
        }
    }
}