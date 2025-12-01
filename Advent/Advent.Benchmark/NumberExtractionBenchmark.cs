using BenchmarkDotNet.Attributes;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Advent.Benchmark
{
    [SimpleJob]
    public partial class NumberExtractionBenchmark
    {
        private string[] _numbers = Array.Empty<string>();

        [GlobalSetup]
        public void Setup()
        {
            _numbers = new string[10000];
            var random = Random.Shared;
            var sb = new StringBuilder();
            for (var i = 0; i < _numbers.Length; i++)
            {
                for (var n = 0; n < 100; n++)
                {
                    var num = random.Next(int.MinValue, int.MaxValue).ToString(CultureInfo.InvariantCulture);
                    sb.Append(num);
                    sb.Append(", ");
                }
                _numbers[i] = sb.ToString();
                sb.Clear();
            }
        }

        [Benchmark]
        public void ExtractLongs()
        {
            for (var i =0; i < _numbers.Length;i++)
            {
                var values = _numbers[i].ExtractLongs();
            }
        }


        [Benchmark]
        public void Regex_Parse()
        {
            for (var i = 0; i < _numbers.Length; i++)
            {
                var values = NumberRegex().Matches(_numbers[i]).OfType<Match>().Select(m => long.Parse(m.Value, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture)).ToList();
            }
        }

        [GeneratedRegex(@"-?\d+")]
        private static partial Regex NumberRegex();
    }
}
