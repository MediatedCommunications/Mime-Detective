using System.IO;
using System.Linq;

namespace MimeDetective.Benchmark.Support;

public sealed class BenchmarkFiles {
    public const string DataRootPath = @"MimeDetective(Tests)\MicroTests\Data";

    public static BenchmarkFiles Instance { get; } = new();

    public BenchmarkParameter<string>[] FilePaths { get; }

    public BenchmarkParameter<byte[]>[] FileContents { get; }

    public BenchmarkFiles() {
        string fullPath;

        var basePath = Path.GetFullPath(".");
        for (;;) {
            fullPath = Path.Combine(basePath, DataRootPath);
            if (Directory.Exists(fullPath)) {
                break;
            }

            basePath = Path.GetFullPath(Path.Combine(basePath, ".."));
            if (!Directory.Exists(basePath)) {
                throw new DirectoryNotFoundException();
            }
        }

        FilePaths = Directory
            .EnumerateFiles(fullPath, "*", SearchOption.AllDirectories)
            .OrderByDescending(f => f.Length)
            .Where((_, i) => i % 10 == 0)
            .Select(f => new BenchmarkParameter<string>(Path.GetFileName(f), f))
            .ToArray();

        FileContents = FilePaths
            .Select(f => new BenchmarkParameter<byte[]>(f.Name, File.ReadAllBytes(f.Value)))
            .ToArray();
    }
}
