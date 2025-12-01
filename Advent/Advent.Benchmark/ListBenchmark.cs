using Advent.Shared;
using BenchmarkDotNet.Attributes;
using System.Collections.Generic;

namespace Advent.Benchmark
{
    [SimpleJob]
    public class ListBenchmark
    {
        [Params(10, 1000, 100000)]
        public int N;

        [Benchmark]
        public void List()
        {
            var list = new List<int>(N);
            for (var i = 0; i < N; i++)
            {
                list.Add(i);
            }
        }

        [Benchmark]
        public void FixedList()
        {
            var list = FixedList<int>.Create(new int[N]);
            for (var i = 0; i < N; i++)
            {
                list.Add(i);
            }
        }
    }
}
