﻿namespace JobMagnet.Shared.Utils;

public static class DateExtensions
{
    public static ushort GetAge(this DateOnly? dateOfBirth)
    {
        if (dateOfBirth is null)
            return 0;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - dateOfBirth.Value.Year;

        if (dateOfBirth > today.AddYears(-age))
            age--;

        return (ushort)age;
    }

    public static DateOnly ToDateOnly(this DateTime dateTime)
    {
        return DateOnly.FromDateTime(dateTime);
    }

    public static DateOnly ToDateOnly(this DateTime? dateTime)
    {
        return dateTime.HasValue ? DateOnly.FromDateTime(dateTime.Value) : default;
    }
}