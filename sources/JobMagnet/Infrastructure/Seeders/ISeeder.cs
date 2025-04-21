namespace JobMagnet.Infrastructure.Seeders;

public interface ISeeder
{
    Task RegisterMasterTablesAsync();
    Task RegisterProfileAsync();
}