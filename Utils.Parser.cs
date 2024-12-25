namespace AdventOfCode;

public static partial class Utils
{
    public static List<T> ParseList<T>(string input, char separator) =>
        input
            .Split(separator, StringSplitOptions.RemoveEmptyEntries)
            .Select(ConvertToType<T>)
            .ToList();

    public static T[,] ParseMatrix<T>(string input)
    {
        var rows = input.Split(Environment.NewLine);
        var result = new T[rows.Length, rows[0].Length];

        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[0].Length; x++)
            {
                result[y, x] = ConvertToType<T>(rows[y][x]);
            }
        }

        return result;
    }

    private static T ConvertToType<T>(object input) =>
        (T)Convert.ChangeType(input, typeof(T));
}
