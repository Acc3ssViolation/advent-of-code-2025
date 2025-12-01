using System;
using System.Collections;
using System.Collections.Generic;

namespace Advent.Shared
{
    public struct FixedList<T> : IEnumerable<T>
    {
        public T[] data;
        public int length;

        public void Add(T item)
        {
            data[length++] = item;
        }

        public void Clear()
        {
            length = 0;
        }

        public readonly Span<T> AsSpan()
        {
            return new Span<T>(data, 0, length);
        }

        public readonly IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < length; i++)
            {
                yield return data[i];
            }
        }

        readonly IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static FixedList<T> Create(T[] array)
        {
            return new FixedList<T> { data = array };
        }
    }
}
