using CommunityToolkit.Diagnostics;

namespace JobMagnet.Domain.Aggregates.Profiles.ValueObjects;

public sealed record BirthDate
{
    public const int MinimumAge = 13;
    public const int MaximumAge = 120;
    
    public DateOnly? Value { get; }
    
    public BirthDate(DateOnly? value)
    {
        if (value.HasValue)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var age = CalculateAge(value.Value, today);
            
            Guard.IsGreaterThanOrEqualTo(age, MinimumAge, nameof(value));
            Guard.IsLessThanOrEqualTo(age, MaximumAge, nameof(value));
        }
        
        Value = value;
    }
    
    public Age? GetAge()
    {
        if (!Value.HasValue) return null;
        var years = CalculateAge(Value.Value, DateOnly.FromDateTime(DateTime.UtcNow));
        return Age.FromYears(years);
    }
    
    private static int CalculateAge(DateOnly birthDate, DateOnly currentDate)
    {
        var age = currentDate.Year - birthDate.Year;
        if (currentDate < birthDate.AddYears(age))
            age--;
        return age;
    }
    
    public static BirthDate Empty => new((DateOnly?) null);
}