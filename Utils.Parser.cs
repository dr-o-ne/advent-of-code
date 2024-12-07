using System.ComponentModel;

namespace AdventOfCode;

public static partial class Utils
{
    public static List<T> ParseList<T>(string input, char separator) =>
        input
            .Split(separator, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => (T)ConvertToType(x, typeof(T)))
            .ToList();

    private static object ConvertToType(string input, Type targetType)
    {
        var converter = TypeDescriptor.GetConverter(targetType);
        if (converter != null && converter.IsValid(input))
        {
            return converter.ConvertFromString(input)!;
        }

        throw new InvalidOperationException($"Cannot convert '{input}' to type {targetType.Name}");
    }
}
