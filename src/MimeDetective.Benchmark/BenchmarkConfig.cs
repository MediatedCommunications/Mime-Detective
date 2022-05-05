#region

using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#endregion

namespace MimeDetective.Benchmark
{
    public class BenchmarkConfig
    {
        public static IConfig Get()
        {
            return ManualConfig.CreateEmpty()
                               .WithArtifactsPath($"{Path.GetFullPath(@"..\..\..\BenchmarkDotNet.Artifacts")}")
                               .AddJob(Job.Default.WithRuntime(CoreRuntime.Core60).WithPlatform(Platform.X64))
                               .AddLogicalGroupRules(BenchmarkLogicalGroupRule.ByParams)
                               .AddDiagnoser(MemoryDiagnoser.Default)
                               .AddColumnProvider(DefaultColumnProviders.Instance)
                               .AddLogger(ConsoleLogger.Default)
                               .AddExporter(MarkdownExporter.GitHub)
                               .AddExporter(HtmlExporter.Default)
                               .AddAnalyser(GetAnalysers().ToArray());
        }

        private static IEnumerable<IAnalyser> GetAnalysers()
        {
            yield return EnvironmentAnalyser.Default;
            yield return OutliersAnalyser.Default;
            yield return MinIterationTimeAnalyser.Default;
            yield return MultimodalDistributionAnalyzer.Default;
            yield return RuntimeErrorAnalyser.Default;
            yield return ZeroMeasurementAnalyser.Default;
            yield return BaselineCustomAnalyzer.Default;
        }
    }
}