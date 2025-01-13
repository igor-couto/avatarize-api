using Asp.Versioning;

namespace Avatarize.Configuration;

public static class ApiVersioningConfiguration
{
    public const string MajorVersion = "0";
    public const string MinorVerion = "0";

    public static void AddVersioning(this IServiceCollection services)
    {
        services
        .AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.DefaultApiVersion = new ApiVersion(int.Parse(MajorVersion), int.Parse(MinorVerion));
            options.AssumeDefaultVersionWhenUnspecified = true;
        });
    }
}