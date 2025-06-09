namespace JobMagnet.Host.Extensions.SettingSections;

public class AllowOrigins
{
    public const string Key = nameof(AllowOrigins);
    public string[] Origins { get; set; }
}