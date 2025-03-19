extern alias JobMagnetHost;
using JobMagnet.Context;
using JobMagnet.Integration.Tests.Factories;
using JobMagnet.Integration.Tests.TestContainers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace JobMagnet.Integration.Tests.Fixtures;

public class JobMagnetTestSetupFixture : IAsyncLifetime
{
    private readonly MsSqlServerTestContainer _msSqlServerTestContainer;
    private HostWebApplicationFactory<Program> _webApplicationFactory;

    public JobMagnetTestSetupFixture()
    {
        _msSqlServerTestContainer = new MsSqlServerTestContainer();
        _webApplicationFactory = null!;
    }

    public async Task InitializeAsync()
    {
        await _msSqlServerTestContainer.InitializeAsync();
        _webApplicationFactory = new HostWebApplicationFactory<Program>(GetConnectionString());
        await EnsureDatabaseCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _msSqlServerTestContainer?.DisposeAsync()!;
        await _webApplicationFactory.DisposeAsync();
    }

    public IServiceProvider GetProvider()
    {
        return _webApplicationFactory?.Services!;
    }

    public HttpClient GetClient()
    {
        return _webApplicationFactory?.CreateClient()!;
    }

    private string GetConnectionString()
    {
        return new SqlConnectionStringBuilder(_msSqlServerTestContainer.GetConnectionString())
        {
            InitialCatalog = $"JobMagnet-{Guid.NewGuid()}"
        }.ConnectionString;
    }

    private async Task EnsureDatabaseCreatedAsync()
    {
        using var scope = _webApplicationFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<JobMagnetDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
    }
}