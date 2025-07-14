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

    public virtual bool Equals(ContactTypeAlias? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return string.Equals(Alias, other.Alias, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode() =>
        Alias.GetHashCode(StringComparison.OrdinalIgnoreCase);
}