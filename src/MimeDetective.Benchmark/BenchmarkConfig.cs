#region

using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

#endregion

namespace MimeDetective.Benchmark;

public class BenchmarkConfig {
    private static readonly IAnalyser[] Analysers = {
        EnvironmentAnalyser.Default, OutliersAnalyser.Default, MinIterationTimeAnalyser.Default,
        MultimodalDistributionAnalyzer.Default, RuntimeErrorAnalyser.Default, ZeroMeasurementAnalyser.Default,
        BaselineCustomAnalyzer.Default
    };

    public static IConfig Get() {
#if DEBUG
        return new DebugQuickInProcessConfig()
#else
        return ManualConfig.CreateEmpty()
#endif
            .WithArtifactsPath($"{Path.GetFullPath(@"..\..\..\BenchmarkDotNet.Artifacts")}")
            .WithCultureInfo(CultureInfo.InvariantCulture)
            .AddLogicalGroupRules(BenchmarkLogicalGroupRule.ByParams)
            .AddDiagnoser(MemoryDiagnoser.Default)
            .AddColumnProvider(DefaultColumnProviders.Instance)
            .AddLogger(ConsoleLogger.Default)
            .AddExporter(MarkdownExporter.GitHub)
            .AddExporter(HtmlExporter.Default)
            .AddAnalyser(Analysers);
    }
}

public class DebugQuickInProcessConfig : DebugConfig {
    public override IEnumerable<Job> GetJobs() {
        return [
            Job.Dry
                .WithToolchain(
                    new InProcessEmitToolchain(
                        TimeSpan.FromHours(1), // 1h should be enough to debug the benchmark
                        true))
        ];
    }
}
