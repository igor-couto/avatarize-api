using Microsoft.OpenApi.Models;

namespace Avatarize;

public static class SwaggerConfiguration
{
    private const string MajorVersion = "0";
    private const string Title = $"Avatarize API v{MajorVersion}";

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc("v0",
                new OpenApiInfo
                {
                    Title = Title,
                    Description = "Generate user avatars using hash visualization techniques.",
                    Version = $"v{MajorVersion}",
                    Contact = new OpenApiContact
                    {
                        Name = "Igor Couto",
                        Email = "igor.fcouto@gmail.com",
                        Url = new Uri("http://igor-couto.github.io/avatarize.io/")
                    }
                });

            config.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Avatarize.xml"));

        });
    }

    public static void UseSwaggerConfiguration(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.DocumentTitle = Title;
            options.SwaggerEndpoint($"/swagger/v{MajorVersion}/swagger.json", Title);
            options.DefaultModelsExpandDepth(-1);
            options.DisplayRequestDuration();
        });
    }
}
