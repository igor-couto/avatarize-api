namespace Avatarize;

using Microsoft.OpenApi.Models;

public static class SwaggerConfiguration
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc("v0",
                new OpenApiInfo
                {
                    Title = "Avatarize API",
                    Description = "Generate user avatars using hash visualization techniques.",
                    Version = "v0",
                    Contact = new OpenApiContact
                    {
                        Name = "Igor Couto",
                        Email = "igor.fcouto@gmail.com",
                        Url = new Uri("http://igor-couto.github.io/avatarize.io/")
                    }
                });
        });
    }

    public static void UseSwaggerConfiguration(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(config =>
        {
            config.SwaggerEndpoint("/swagger/v0/swagger.json", "Avatarize API v1.0.0");
            config.DefaultModelsExpandDepth(-1);
        });
    }
}
