namespace Avatarize.Configuration;

public static class CacheConfiguration
{
    public static void AddCache(this IServiceCollection services)
    {
        services.AddOutputCache(options =>
        {
            options.AddPolicy("Expire in 10 minutes", builder => 
                builder.Expire(TimeSpan.FromMinutes(10)));
        });
    }
}