#region

using BenchmarkDotNet.Attributes;
using MimeDetective.Definitions;
using MimeDetective.Definitions.Licensing;
using MimeDetective.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

#endregion

namespace MimeDetective.Benchmark
{
    public class Benchmarks
    {
        private const string DATA_ROOT_PATH = @"MimeDetective(Tests)\MicroTests\Data";

        [ParamsSource(nameof(ValuesForTestFile))]
        public string TestFile { get; set; }

        public IEnumerable<string> ValuesForTestFile()
        {
            var fullPath = Path.GetFullPath($@"..\..\..\..\{DATA_ROOT_PATH}");
            return Directory.GetDirectories(fullPath).Select(directory => new DirectoryInfo(directory).EnumerateFiles().MaxBy(f => f.Length)?.Name);
        }

        [Benchmark(Baseline = true)]
        public string Default()
        {
            Console.WriteLine(this.TestFile);
            return this.Inspect(this.TestFile, Definitions.Default.All());
        }

        [Benchmark]
        public string Condensed()
        {
            return this.Inspect(this.TestFile, new CondensedBuilder { UsageType = UsageType.PersonalNonCommercial }.Build());
        }

        [Benchmark]
        public string Exhaustive()
        {
            return this.Inspect(this.TestFile, new ExhaustiveBuilder { UsageType = UsageType.PersonalNonCommercial }.Build());
        }

        private string Inspect(string testFile, ImmutableArray<Definition> definitions)
        {
            var fullPath = Path.GetFullPath($@"..\..\..\..\..\..\..\..\{DATA_ROOT_PATH}");
            var filePath = Directory.GetFiles(fullPath, testFile, SearchOption.AllDirectories).FirstOrDefault();
            if (string.IsNullOrEmpty(filePath))
            {
                return null;
            }

            var inspector = new ContentInspectorBuilder { Definitions = definitions }.Build();

            var results = inspector.Inspect(ContentReader.Default.ReadFromFile(filePath));
            var ext = results.ByFileExtension();
            var mt = results.ByMimeType();

            return $"{testFile}: {ext.MaxBy(e => e.Points)?.Extension}({mt.MaxBy(m => m.Points)?.MimeType})";
        }
    }
}