using CommunityToolkit.Diagnostics;

namespace JobMagnet.Domain.Aggregates.Contact;

public record ContactTypeAlias
{
    public const int MaxAliasLength = 20;
    public string Alias { get; }

    public ContactTypeAlias(string alias)
    {
        Guard.IsNotNullOrEmpty(alias);
        Guard.IsLessThanOrEqualTo(alias.Length, MaxAliasLength);

        Alias = alias;
    }
}