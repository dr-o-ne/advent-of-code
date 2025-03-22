using System.Collections;

namespace AdventOfCode.Common;

public sealed class Grid2D<T> : IEnumerable<(int y, int x, T value)>
{
    private readonly T[,] _inner;
    public int Rows { get; }
    public int Columns { get; }

    public Grid2D(string input)
    {
        _inner = Utils.ParseMatrix<T>(input);
        Rows = _inner.GetLength(0);
        Columns = _inner.GetLength(1);
    }

    public T this[int y, int x]
    {
        get => _inner[y, x];
        set => _inner[y, x] = value;
    }

    public IEnumerator<(int y, int x, T value)> GetEnumerator()
    {
        for (int y = 0; y < Rows; y++)
            for (int x = 0; x < Columns; x++)
                yield return (y, x, _inner[y, x]);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
