namespace Advent
{
    internal class UnionFind<T>
    {
        private readonly T[] _elements;
        private readonly int[] _parent;
        private readonly int[] _rank;

        public UnionFind(T[] elements)
        {
            _elements = elements;
            _parent = new int[_elements.Length];
            _rank = new int[_elements.Length];

            for (var i = 0; i < _elements.Length; i++)
            {
                _parent[i] = i;
                _rank[i] = 0;
            }
        }

        public int FindSetIndex(int elementIndex)
        {
            while (_parent[elementIndex] != elementIndex)
            {
                _parent[elementIndex] = _parent[_parent[elementIndex]];
                elementIndex = _parent[elementIndex];
            }
            return elementIndex;
        }

        public int Union(int a, int b)
        {
            a = FindSetIndex(a);
            b = FindSetIndex(b);

            if (a == b)
                return a;

            if (_rank[a] < _rank[b])
                (a, b) = (b, a);

            _parent[b] = a;
            if (_rank[a] == _rank[b])
                _rank[a]++;

            return a;
        }
    }
}
