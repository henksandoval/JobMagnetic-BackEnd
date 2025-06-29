namespace JobMagnet.Shared.Utils;

public static class EnumerableExtensions
{
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
    {
        var index = 0;
        foreach (var item in source) yield return (item, index++);
    }
}