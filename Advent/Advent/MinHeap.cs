using System;

namespace Advent
{
    internal class MinHeap<T> where T : IComparable<T>
    {
        private readonly T[] _elements;
        private int _size;

        public MinHeap(int size)
        {
            _elements = new T[size];
        }

        public void Insert(T element)
        {
            _elements[_size++] = element;

            var i = _size - 1;
            var parent = Parent(i);
            while (i > 0 && (_elements[parent].CompareTo(_elements[i]) == 1))
            {
                (_elements[parent], _elements[i]) = (_elements[i], _elements[parent]);

                i = parent;
                parent = Parent(i);
            }
        }

        public T Pop()
        {
            // Swap root with last element, removing the last element
            var result = _elements[0];
            _elements[0] = _elements[_size - 1];
            _size--;

            // Heapify le tree
            var i = 0;
            while (true)
            {
                var left = LeftChild(i);
                var right = RightChild(i);
                var smallest = i;

                if (left < _size && _elements[left].CompareTo(_elements[smallest]) == -1)
                    smallest = left;
                if (right < _size && _elements[right].CompareTo(_elements[smallest]) == -1)
                    smallest = right;

                if (smallest == i)
                    break;

                (_elements[i], _elements[smallest]) = (_elements[smallest], _elements[i]);
                i = smallest;
            }

            return result;
        }

        private static int LeftChild(int i)
            => 2 * i + 1;

        private static int RightChild(int i)
            => 2 * i + 2;

        private static int Parent(int i)
            => (i - 1) / 2;
    }
}
