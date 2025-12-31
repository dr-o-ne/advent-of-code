using System.Collections.Immutable;

namespace AdventOfCode.Common;

public sealed class UnionFind
{
    private readonly int[] _root;
    private readonly int[] _size;
    private int _components;

    public UnionFind(int size)
    {
        _root = new int[size];
        _size = new int[size];

        for(int i = 0; i < size; i++)
        {
            _root[i] = i;
            _size[i] = 1;
        }

        _components = size;
    }

    public int Find( int x )
    {
        int root = x;
        while (root != _root[root])
            root = _root[root];

        int current = x;
        while( current != root)
        {
            int temp = _root[current];
            _root[current] = root;
            current = temp;
        }

        return root;
    }

    public void Union( int x, int y ) {
        int rootX = Find(x);
        int rootY = Find(y);

        if (rootX == rootY)
            return;

        if( _size[rootX] < _size[rootY] )
        {
            _root[rootX] = rootY;
            _size[rootY] += _size[rootX];
            _size[rootX] = 0;

        }
        else
        {
            _root[rootY] = rootX;
            _size[rootX] += _size[rootY];
            _size[rootY] = 0;
        }

        _components--;
    }

    public bool Connected( int x, int y ) =>
        Find(x) == Find(y);

    public int ComponentSize( int x ) => 
        _size[Find(x)];

    public ImmutableArray<int> AllComponentSizes =>
        [.. _size];

    public int Components => 
        _components;

}
