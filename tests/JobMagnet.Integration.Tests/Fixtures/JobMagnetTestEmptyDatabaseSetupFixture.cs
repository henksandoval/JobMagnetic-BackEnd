using JobMagnet.Integration.Tests.Factories;
using JobMagnet.Integration.Tests.TestContainers;
using Microsoft.Data.SqlClient;
using Xunit.Abstractions;

namespace JobMagnet.Integration.Tests.Fixtures;

// ReSharper disable once ClassNeverInstantiated.Global
public class JobMagnetTestEmptyDatabaseSetupFixture : IAsyncLifetime
{
    private readonly MsSqlServerTestContainer _msSqlServerTestContainer = new();

    private string? _connectionString;
    private ITestOutputHelper? _testOutputHelper;
    private HostWebApplicationFactory<Program> _webApplicationFactory = null!;

    public async Task InitializeAsync()
    {
        _testOutputHelper?.WriteLine("Starting JobMagnetTestSetupFixture...");
        await _msSqlServerTestContainer.InitializeAsync();
        SetConnectionString();
        _webApplicationFactory = new HostWebApplicationFactory<Program>(_connectionString);
    }

    public async Task DisposeAsync()
    {
        await _msSqlServerTestContainer?.DisposeAsync()!;
        await _webApplicationFactory.DisposeAsync();
    }

    public ITestOutputHelper SetTestOutputHelper(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        return _testOutputHelper;
    }

    public IServiceProvider GetProvider()
    {
        return _webApplicationFactory?.Services!;
    }

    public HttpClient GetClient()
    {
        return _webApplicationFactory?.CreateClient()!;
    }

    private void SetConnectionString()
    {
        _connectionString = new SqlConnectionStringBuilder(_msSqlServerTestContainer.GetConnectionString())
        {
            InitialCatalog = $"JobMagnetTestDb_{Guid.NewGuid()}"
        }.ConnectionString;

        _testOutputHelper?.WriteLine(_connectionString);
    }
}