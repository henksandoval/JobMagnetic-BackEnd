using JobMagnet.Infrastructure.ExternalServices.Gemini;
using JobMagnet.Integration.Tests.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace JobMagnet.Integration.Tests.Factories;

public class HostWebApplicationFactory<TProgram>(string? msSqlServerConnectionString)
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:DefaultConnection", msSqlServerConnectionString);
        builder.UseSetting("Logging:LogLevel:Default", "Information");
        builder.ConfigureServices(services =>
        {
            services
                .AddSingleton<IGeminiClient, MockGeminiClient>();
        });
        builder.UseEnvironment("Development");
    }
}