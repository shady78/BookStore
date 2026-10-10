using Microsoft.OpenApi;

namespace BookStore.API.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter jwt token only. Example: eyjdfddfd..."
                });

                options.AddSecurityRequirement(d => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", d)] = new List<string>()
                });
            });
            return services;
        } 
    }
}
