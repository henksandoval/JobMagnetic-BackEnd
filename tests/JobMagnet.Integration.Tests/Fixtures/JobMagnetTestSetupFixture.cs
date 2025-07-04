extern alias JobMagnetHost;
using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Aggregates.Skills;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Integration.Tests.Factories;
using JobMagnet.Integration.Tests.TestContainers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Xunit.Abstractions;

namespace JobMagnet.Integration.Tests.Fixtures;

// ReSharper disable once ClassNeverInstantiated.Global
public class JobMagnetTestSetupFixture : IAsyncLifetime
{
    private readonly MsSqlServerTestContainer _msSqlServerTestContainer = new();
    public IReadOnlyList<SkillType> SeededSkillTypes { get; private set; } = null!;
    public IReadOnlyList<ContactType> SeededContactTypes { get; private set; } = null!;

    private readonly RespawnerOptions _respawnerOptions = new()
    {
        WithReseed = true,
        TablesToIgnore = [
            "SkillTypes",
            "SkillTypeAliases",
            "SkillCategories",
            "ContactTypes",
            "ContactTypeAliases"
        ]
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
        await EnsureDatabaseCreatedAsync();
        await LoadSeedDataAsync();
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

    public IServiceProvider GetProvider() => _webApplicationFactory?.Services!;

    public HttpClient GetClient() => _webApplicationFactory?.CreateClient()!;

    private void SetConnectionString()
    {
        _connectionString = new SqlConnectionStringBuilder(_msSqlServerTestContainer.GetConnectionString())
        {
            InitialCatalog = $"JobMagnetTestDb_{Guid.NewGuid()}"
        }.ConnectionString;

        _testOutputHelper?.WriteLine("Successful connection to the database: {0}", _connectionString);
    }

    private async Task EnsureDatabaseCreatedAsync()
    {
        using var scope = _webApplicationFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<JobMagnetDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
    }

    private async Task LoadSeedDataAsync()
    {
        _testOutputHelper?.WriteLine("Loading seed data from database into fixture");
        await using var scope = GetProvider().CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<JobMagnetDbContext>();

        SeededSkillTypes = await context.SkillTypes.Include(st => st.Category).ToListAsync();
        SeededContactTypes = await context.ContactTypes.Include(ct => ct.Aliases).ToListAsync();
    }
}