using System.Net;
using System.Threading.RateLimiting;

namespace Avatarize.Configuration;

public static class RateLimitConfiguration
{
    public static void AddRateLimit(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = (int) HttpStatusCode.TooManyRequests;
            options.AddPolicy("Get Avatar Rate Limit", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: 
                        httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
                    factory:
                        partition => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 10,
                            Window = TimeSpan.FromSeconds(1)
                        }));
        });
    }

    public static void UseRateLimit(this WebApplication app)
    {
        app.UseRateLimiter();
    }
}