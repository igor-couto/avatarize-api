using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
namespace FunctionalTests;

public class TestApplication : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        return base.CreateHost(builder);
    }
}