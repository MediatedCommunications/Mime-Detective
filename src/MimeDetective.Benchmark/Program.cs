#region

using BenchmarkDotNet.Running;

#endregion

namespace MimeDetective.Benchmark
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var summaries = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, BenchmarkConfig.Get());
                return;
            }

            var summary = BenchmarkRunner.Run<Benchmarks>(BenchmarkConfig.Get());
        }
    }
}