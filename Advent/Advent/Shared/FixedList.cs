using System;
using System.Collections;
using System.Collections.Generic;

namespace Advent.Shared
{
    public sealed class FixedList<T> : IEnumerable<T>
    {
        private readonly Memory<T> _data;
        public int Count { get; private set; }

        public T this[int index] => _data.Span[index];

        public FixedList(Memory<T> array)
        {
            _data = array;
        }

        public void Add(T item)
        {
            _data.Span[Count++] = item;
        }

        public void Sort()
        {
            _data.Span[..Count].Sort();
        }

        public void Remove(T item)
        {
            var span = _data.Span;
            for (var i = 0; i < Count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(span[i], item))
                {
                    for (var k = i; k < Count - 1; k++)
                        span[k] = span[k + 1];
                    Count--;
                    return;
                }
            }
        }

        public void Clear()
        {
            Count = 0;
        }

        public Span<T> AsSpan()
        {
            return _data.Span;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return _data.Span[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
