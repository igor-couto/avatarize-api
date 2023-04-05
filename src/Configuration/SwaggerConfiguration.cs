using Microsoft.OpenApi.Models;

namespace Avatarize.Configuration;

public static class SwaggerConfiguration
{
    private const string Title = $"Avatarize API v{ApiVersioningConfiguration.MajorVersion}.{ApiVersioningConfiguration.MinorVerion}";

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc($"v{ApiVersioningConfiguration.MajorVersion}",
                new OpenApiInfo
                {
                    Title = Title,
                    Description = "Generate user avatars using hash visualization techniques.",
                    Version = $"v{ApiVersioningConfiguration.MajorVersion}",
                    License = new OpenApiLicense { Name = "GNU Affero General Public License", Url = new Uri("https://github.com/igor-couto/avatarize-api/blob/main/LICENSE") },
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
            options.SwaggerEndpoint($"/swagger/v{ApiVersioningConfiguration.MajorVersion}/swagger.json", Title);
            options.DefaultModelsExpandDepth(-1);
            options.DisplayRequestDuration();
        });
    }
}
