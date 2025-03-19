namespace JobMagnet.Integration.Tests.TestContainers;

using Testcontainers.MsSql;

public class MsSqlServerTestContainer : IAsyncLifetime
{
    private const string MssqlServerImage = "mcr.microsoft.com/mssql/server:2022-latest";
    private const ushort DefaultMsSqlPort = 1433;
    private MsSqlContainer _container = null!;

    public async Task InitializeAsync()
    {
        _container = new MsSqlBuilder()
            .WithImage(MssqlServerImage)
            .WithPortBinding(DefaultMsSqlPort, true)
            .Build();
        await _container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        try
        {
            await _container.StopAsync();
            await _container.DisposeAsync().AsTask();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public string GetConnectionString()
    {
        var connectionString = _container.GetConnectionString();
        return connectionString;
    }
}