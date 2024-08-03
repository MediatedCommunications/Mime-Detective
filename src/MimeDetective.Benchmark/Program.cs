#region

using BenchmarkDotNet.Running;

#endregion

namespace MimeDetective.Benchmark;

public class Program {
    public static void Main(string[] args) {
        if (args.Length > 0) {
            _ = BenchmarkSwitcher
                .FromAssembly(typeof(Program).Assembly)
                .Run(args, BenchmarkConfig.Get());
            return;
        }

        _ = BenchmarkRunner.Run(
            [
                typeof(DefinitionBuilderBenchmarks),
                typeof(InspectorBuilderBenchmarks),
                typeof(PreloadedFileInspectorBenchmarks)
            ],
            BenchmarkConfig.Get());
    }
}
