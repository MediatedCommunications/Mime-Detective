using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeDetective.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MimeDetective.Tests;

[TestClass]
public abstract class MicroTests {
    private static IContentInspector? _getEngineResult;
    private static readonly string _relativeRoot = Path.Combine("MicroTests", "Data");

    protected virtual string RelativeRoot => _relativeRoot;

    protected static IContentInspector GetEngine() {
        var isDebug = false;

        if (_getEngineResult == default && isDebug) {
            var inspector = new ContentInspectorBuilder {
                Definitions = { Default.FileTypes.Email.EML() },
                Parallel = false
            };
            _getEngineResult = inspector.Build();
        }


        if (_getEngineResult == default) {
            var definitions = Default.All();

            _getEngineResult = new ContentInspectorBuilder {
                Definitions = definitions,
                Parallel = true
            }.Build();
            ;
        }

        return _getEngineResult;
    }

    [TestMethod]
    public void A_GenerateTests() {
        Console.WriteLine(GenerateTests());
    }

    protected string GenerateTests() {
        var tret = new StringBuilder();

        var fullPath = Path.GetFullPath(RelativeRoot);

        foreach (var item in Directory.GetFiles(fullPath)) {
            var fn = Path.GetFileName(item);
            var name = fn.Replace(".", "_");

            var content = $$"""
                            [TestMethod]
                            public void {{name}}(){
                                Test("{{fn}}");
                            }
                            """;

            tret.AppendLine(content);
        }

        var ret = tret.ToString();
        return ret;
    }

    protected void Test(string relativeFileName) {
        var fn = Path.Combine(RelativeRoot, relativeFileName);
        var fullPath = Path.GetFullPath(fn);
        var fileName = Path.GetFileName(fullPath);

        var content = ContentReader.Default.ReadFromFile(fullPath);

        var engine = GetEngine();

        var allResults = engine.Inspect(content).ByFileExtension();

        var results = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        if (allResults.Length > 0) {
            var maxPoints = allResults.First().Points;
            results.UnionWith(
                from x in allResults
                where x.Points == maxPoints
                select x.Extension
            );
        }

        var dotIndex = fileName.LastIndexOf('.');
        var expected = dotIndex >= 0
            ? fileName[dotIndex..].ToLowerInvariant()
            : string.Empty;

        var isValid = results.Contains(expected);

        if (!isValid) {
            Assert.AreNotEqual(expected, string.Join(",", results), StringComparer.OrdinalIgnoreCase);
        }
    }
}
