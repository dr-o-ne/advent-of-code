namespace AdventOfCode;

public static partial class Utils
{
    public static List<T> ParseList<T>(string input, string separator) =>
        ParseList(input, separator, ConvertToType<T>);

    public static List<T> ParseList<T>(string input, string separator, Func<string, T> converter) =>
        input
            .Split(separator, StringSplitOptions.RemoveEmptyEntries)
            .Select(converter)
            .ToList();

    public static T[,] ParseMatrix<T>(string input)
    {
        var rows = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var result = new T[rows.Length, rows[0].Length];

        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[0].Length; x++)
            {
                var value = rows[y][x].ToString();
                result[y, x] = ConvertToType<T>(value);
            }
        }

        return result;
    }

    private static T ConvertToType<T>(string input) =>
        (T)Convert.ChangeType(input, typeof(T));
}
