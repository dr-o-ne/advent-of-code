namespace AdventOfCode;

public static partial class Utils
{
    public static List<List<T>> Permutations<T>(List<T> items)
    {
        var acc = new List<List<T>>();

        Permute(items, 0, items.Count - 1, acc);

        return acc;
    }

    public static List<List<T>> Subsets<T>(List<T> items)
    {
        var count = 1 << items.Count;
        var result = new List<List<T>>( count );

        for( int i = 0; i < count; i++ )
        {
            var current = new List<T>();

            for( int j = 0; j < items.Count; j++)
            {
                if ((i & (1 << j)) == 0)
                    continue;

                current.Add(items[j]);
            }

            result.Add( current );
        }

        return result;
    }

    public static IEnumerable<(T, T)> Pairs<T>(List<T> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            for (int j = i + 1; j < items.Count; j++)
            {
                yield return (items[i], items[j]);
            }
        }
    }

    private static void Permute<T>(List<T> items, int left, int right, List<List<T>> acc)
    {
        if (left == right)
        {
            acc.Add([.. items]);
            return;
        }

        for (int i = left; i <= right; i++)
        {
            Swap(items, left, i);
            Permute(items, left + 1, right, acc);
            Swap(items, left, i);
        }
    }

    private static void Swap<T>(List<T> items, int left, int right)
    {
        var temp = items[left];
        items[left] = items[right];
        items[right] = temp;
    }
}
