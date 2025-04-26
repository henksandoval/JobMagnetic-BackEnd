using JobMagnet.Integration.Tests.Factories;
using JobMagnet.Integration.Tests.TestContainers;
using Microsoft.Data.SqlClient;
using Respawn;
using Xunit.Abstractions;

namespace JobMagnet.Integration.Tests.Fixtures;

// ReSharper disable once ClassNeverInstantiated.Global
public class JobMagnetTestEmptyDatabaseSetupFixture : IAsyncLifetime
{
    private readonly MsSqlServerTestContainer _msSqlServerTestContainer = new();

    private readonly RespawnerOptions _respawnerOptions = new()
    {
        WithReseed = true
    };

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

    public async Task ResetDatabaseAsync()
    {
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            _testOutputHelper?.WriteLine("Successful connection to the database: {0}", _connectionString);

            var respawn = await Respawner.CreateAsync(connection, _respawnerOptions);
            await respawn.ResetAsync(connection);
        }
        catch (SqlException sqlEx)
        {
            _testOutputHelper?.WriteLine("Error while trying to connect to the database: {0}", sqlEx.Message);
            throw;
        }
        catch (Exception e)
        {
            _testOutputHelper?.WriteLine("Unexpected error: {0}", e.Message);
            throw;
        }
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