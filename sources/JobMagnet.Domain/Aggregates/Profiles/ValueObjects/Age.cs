using CommunityToolkit.Diagnostics;

public record Age
{
    private readonly ushort _value;
    
    public int Value => _value;
    
    private Age(ushort value)
    {
        _value = value;
    }
    
    public static Age FromYears(int years)
    {
        Guard.IsGreaterThanOrEqualTo(years, 0);
        Guard.IsLessThanOrEqualTo(years, 120);

        return new Age((ushort)years);
    }
    
    public bool IsAdult => Value >= 18;
    public bool IsSenior => Value >= 65;
    public bool IsMinor => Value < 18;
    
    public static implicit operator int(Age age) => age.Value;
}