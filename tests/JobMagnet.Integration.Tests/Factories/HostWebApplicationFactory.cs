using JobMagnet.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;

namespace JobMagnet.Integration.Tests.Factories;

public class HostWebApplicationFactory<TProgram>(string msSqlServerConnectionString)
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:DefaultConnection", msSqlServerConnectionString);
        builder.UseSetting("Logging:LogLevel:Default", "Trace");
        builder.ConfigureServices(services => { services.AddApplicationServices(); });
        builder.UseEnvironment("Development");
    }
}