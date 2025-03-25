extern alias JobMagnetHost;
using JobMagnet.Infrastructure.Context;
using JobMagnet.Integration.Tests.Factories;
using JobMagnet.Integration.Tests.TestContainers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Xunit.Abstractions;

namespace JobMagnet.Integration.Tests.Fixtures;

// ReSharper disable once ClassNeverInstantiated.Global
public class JobMagnetTestSetupFixture : IAsyncLifetime
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
        _testOutputHelper?.WriteLine("Inicializando JobMagnetTestSetupFixture...");
        await _msSqlServerTestContainer.InitializeAsync();
        SetConnectionString();
        _webApplicationFactory = new HostWebApplicationFactory<Program>(_connectionString);
        await EnsureDatabaseCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _msSqlServerTestContainer?.DisposeAsync()!;
        await _webApplicationFactory.DisposeAsync();
    }

    public void SetTestOutputHelper(ITestOutputHelper? testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    public async Task ResetDatabaseAsync()
    {
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            _testOutputHelper?.WriteLine("Conexión a la base de datos exitosa: {0}", _connectionString);

            var respawn = await Respawner.CreateAsync(connection, _respawnerOptions);
            await respawn.ResetAsync(connection);
        }
        catch (SqlException sqlEx)
        {
            _testOutputHelper?.WriteLine("Error al intentar conectar a la base de datos: {0}", sqlEx.Message);
            throw;
        }
        catch (Exception e)
        {
            _testOutputHelper?.WriteLine("Error inesperado: {0}", e.Message);
            throw;
        }
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

    private async Task EnsureDatabaseCreatedAsync()
    {
        using var scope = _webApplicationFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<JobMagnetDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
    }
}