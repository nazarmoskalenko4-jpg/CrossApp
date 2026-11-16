namespace Core.Reports;

public static class EnumerableExtensions
{
    // Власний метод-розширення для Топ-N
    public static IEnumerable<T> TopBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector, int n)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(keySelector);
        
        return source.OrderByDescending(keySelector).Take(n);
    }
}