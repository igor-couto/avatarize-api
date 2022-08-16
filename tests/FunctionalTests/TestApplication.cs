using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Avatarize;

namespace IntegrationTests;

public class TestApplication : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        //builder.ConfigureServices(services =>
        //{
        //    services.AddHealthChecks();
        //    services.AddSingleton(new AssetsService());
        //    services.AddTransient<AvatarGenerationService>();
        //    services.AddTransient<HashService>();
        //    services.AddTransient<ImageService>();
        //});

        return base.CreateHost(builder);
    }
}