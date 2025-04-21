namespace JobMagnet.Infrastructure.Seeders;

public interface ISeeder
{
    Task RegisterMasterTablesAsync(CancellationToken cancellationToken);
    Task RegisterProfileAsync(CancellationToken cancellationToken);
}