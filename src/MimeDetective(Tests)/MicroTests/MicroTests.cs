using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MimeDetective.Tests;

[TestClass]
public abstract class MicroTests {
    private static IContentInspector? _getEngineResult;
    protected static IContentInspector GetEngine() {

        var isDebug = false;

        if (_getEngineResult == default && isDebug) {
            var inspector = new ContentInspectorBuilder() {
                Definitions = {
                    Definitions.Default.FileTypes.Email.EML(),
                },
                Parallel = false,
            };
            _getEngineResult = inspector.Build();
        }




        if (_getEngineResult == default) {
            var defintions = Definitions.Default.All();

            _getEngineResult = new ContentInspectorBuilder() {
                Definitions = defintions,
                Parallel = true,
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

        var fullPath = System.IO.Path.GetFullPath(RelativeRoot());

        foreach (var item in System.IO.Directory.GetFiles(fullPath)) {
            var fn = System.IO.Path.GetFileName(item);
            var name = fn.Replace(".", "_");

            var content = $@"
[TestMethod]
public void {name}(){{
    Test(""{fn}"");
}}";

            tret.AppendLine(content);
        }

        var ret = tret.ToString();
        return ret;
    }

    protected void Test(string relativeFileName) {
        var fn = $@"{RelativeRoot()}{relativeFileName}";
        var fullPath = System.IO.Path.GetFullPath(fn);
        var fileName = System.IO.Path.GetFileName(fullPath);

        var content = ContentReader.Default.ReadFromFile(fullPath);

        var engine = GetEngine();

        var allResults = engine.Inspect(content).ByFileExtension();

        var results = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

        if (allResults.Length > 0) {
            var maxPoints = allResults.First().Points;
            results.UnionWith(
                from x in allResults
                where x.Points == maxPoints
                select x.Extension
            );
        }

        var expected = fileName.Split('.').LastOrDefault()?.ToLowerInvariant() ?? string.Empty;

        var isValid = results.Contains(expected);

        if (!isValid) {
            Assert.AreNotEqual(expected, string.Join(",", results));
        }

    }

    protected virtual string RelativeRoot() {
        return @".\MicroTests\Data\";
    }
}