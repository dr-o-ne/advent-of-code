namespace AdventOfCode.Common;

public record struct Cell2D(int Y, int X)
{
    public Cell2D Add(Vector2D v) => new(Y + v.Y, X + v.X);
};

public record struct Cell2D<T>(int Y, int X, T Value)
{
    public Cell2D<T> Add(Vector2D v) => new(Y + v.Y, X + v.X, Value);
}

public record struct Vector2D(int Y, int X)
{
    public static readonly Vector2D Up = new(-1, 0);
    public static readonly Vector2D Right = new(0, 1);
    public static readonly Vector2D Down = new(1, 0);
    public static readonly Vector2D Left = new(0, -1);

    public static readonly Vector2D[] Directions = { Up, Right, Down, Left };

    public static Vector2D Calculate(Cell2D from, Cell2D to) =>
        new (to.Y - from.Y, to.X - from.X);
}

public sealed class Grid2D<T> : IEnumerable<(int Y, int X, T Value)> where T : IEqualityOperators<T, T, bool>
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

    public Grid2D(int rows, int columns, IEnumerable<Cell2D<T>> cells)
    {
        _inner = new T[rows, columns];
        foreach (var cell in cells)
            _inner[cell.Y, cell.X]  = cell.Value;

        Rows = _inner.GetLength(0);
        Columns = _inner.GetLength(1);
    }

    public Grid2D(int rows, int columns, T defaultValue)
    {
        _inner = new T[rows, columns];

        Rows = _inner.GetLength(0);
        Columns = _inner.GetLength(1);

        for(int y = 0; y < Rows; y++)
            for(int x = 0; x < Columns; x++)
                _inner[y, x] = defaultValue;
    }

    public T this[int y, int x]
    {
        get => _inner[y, x];
        set => _inner[y, x] = value;
    }

    public T this[Cell2D cell]
    {
        get => _inner[cell.Y, cell.X];
        set => _inner[cell.Y, cell.X] = value;
    }

    public bool IsInRange(Cell2D cell) => 
        cell.Y >= 0 &&
        cell.Y < Rows &&
        cell.X >= 0 &&
        cell.X < Columns;

    public string Plot(char defaultValue = ' ')
    {
        var sb = new StringBuilder(Rows * Columns);

        for (int y = 0; y < Rows; y++)
        {
            for (int x = 0; x < Columns; x++)
            {
                if (_inner[y, x] == default)
                    sb.Append(defaultValue);
                else
                    sb.Append(_inner[y, x]);
            }
            if (y != Rows - 1)
                sb.AppendLine();
        }
        
        return sb.ToString();
    }

    public IEnumerator<(int Y, int X, T Value)> GetEnumerator()
    {
        for (int y = 0; y < Rows; y++)
            for (int x = 0; x < Columns; x++)
                yield return (y, x, _inner[y, x]);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
