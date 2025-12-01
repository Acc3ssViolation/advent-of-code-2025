using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.Benchmark
{
    [SimpleJob]
    public class SingleIntParseBenchmark
    {
        private string[] _numbers = Array.Empty<string>();

        [GlobalSetup]
        public void Setup()
        {
            _numbers = new string[10000];
            var random = Random.Shared;
            for (var i = 0; i < _numbers.Length; i++)
            {
                _numbers[i] = random.Next(int.MinValue, int.MaxValue).ToString(CultureInfo.InvariantCulture);
            }
        }

        [Benchmark]
        public void Int_Parse()
        {
            for (var i = 0; i < _numbers.Length; i++)
            {
                int.Parse(_numbers[i]);
            }
        }


        [Benchmark]
        public void Int_Parse_Limited()
        {
            for (var i = 0; i < _numbers.Length; i++)
            {
                int.Parse(_numbers[i], NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
            }
        }

        [Benchmark]
        public void ParseUtils_ToInt()
        {
            for (var i = 0; i < _numbers.Length; i++)
            {
                _numbers[i].ToInt();
            }
        }
    }
}
