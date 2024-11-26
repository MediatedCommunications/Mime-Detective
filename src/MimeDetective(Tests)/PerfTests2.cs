using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeDetective.Engine;
using MimeDetective.Storage;
using System.Collections.Generic;
using System.Linq;

namespace MimeDetective.Tests;

[TestClass]
public class PerfTests2 {
    [TestMethod]
    public void Foo() {
        var maxLength = 5;

        var data = new MimeDetective.Definitions.ExhaustiveBuilder() {
            UsageType = Definitions.Licensing.UsageType.PersonalNonCommercial
        }.Build();

        var sortedData = (
            from x in data
            let Values = x.Signature.Prefix.OrderBy(p => p.Start).ToList()
            let ItemStart = Values.FirstOrDefault()?.Start ?? 0
            let ItemEnd = Values.LastOrDefault()?.ExclusiveEnd() ?? 0
            let v = new {
                Signature = x,
                Start = ItemStart,
                End = ItemEnd
            }
            group v by v.Start
        ).ToDictionary(x => x.Key, x => x.ToList());

        var byStart = sortedData.OrderByDescending(x => x.Value.Count).ToList();

        var start = byStart.FirstOrDefault();
        var startIndex = start.Key;

        var byEnd = (
            from x in start.Value
            let v = x
            group v by v.End
        ).ToDictionary(x => x.Key, x => x.ToList()).OrderByDescending(x => x.Value.Count).ToList();

        var end = (
            from x in byEnd where x.Key < startIndex + maxLength
            select x
        ).FirstOrDefault();
        var inclusiveEndIndex = end.Key;

        var exclusiveEndIndex = 1 + inclusiveEndIndex;
        var length = exclusiveEndIndex - startIndex;

        var tree = new ByteTree<Definition>();
        foreach (var item in data) {
            var key = item.Signature.Prefix.TryGetRange(startIndex, length);

            tree.Add(key, item);

        }

        var exe = System.IO.File.ReadAllBytes($@"C:\Windows\System32\Notepad.exe");
        var content = GetRange(exe, startIndex, length);

        var testCount = 10000;

        var sw1 = System.Diagnostics.Stopwatch.StartNew();
        for (var i = 0; i < testCount; i++) {
            var matches = tree.Find(content);
        }
        sw1.Stop();


        var searcher = new MimeDetective.ContentInspectorBuilder() {
            Definitions = data,
            MatchEvaluatorOptions = new() {
                IncludeSegmentsStrings = false
            },
        }.Build();

        var sw2 = System.Diagnostics.Stopwatch.StartNew();

        for (var i = 0; i < testCount; i++) {
            var matches = searcher.Inspect(exe);
        }
        sw2.Stop();

    }

    private static byte[] GetRange(byte[] content, int startIndex, int length) {
        var ret = new List<byte>();

        for (var i = 0; i < length; i++) {
            var position = startIndex + i;

            if (position >= 0 && position < content.Length) {
                ret.Add(content[position]);
            }
        }


        return ret.ToArray();
    }

    [TestMethod]
    public void ByteTree_Test1() {
        var tree = new ByteTree<int>();

        tree.Add(new byte?[] { 0, 0, 0 }, 0);
        tree.Add(new byte?[] { 0, 0, 1 }, 1);
        tree.Add(new byte?[] { 0, 1, 0 }, 2);
        tree.Add(new byte?[] { 0, 1, 1 }, 3);
        tree.Add(new byte?[] { 1, 0, 0 }, 4);
        tree.Add(new byte?[] { 1, 0, 1 }, 5);
        tree.Add(new byte?[] { 1, 1, 0 }, 6);
        tree.Add(new byte?[] { 1, 1, 1 }, 7);

        tree.Add(new byte?[] { 4, 5, 6 }, 456);
        tree.Add(new byte?[] { 4, 5, null }, 450);
        tree.Add(new byte?[] { 4, null, 6 }, 406);
        tree.Add(new byte?[] { 4, null, null }, 400);


        {
            var test = tree.Find();
            Assert.AreEqual(12, test.Count);
        }

        {
            var test = tree.Find(0, 0, 0);
            Assert.AreEqual(1, test.Count);
            Assert.IsTrue(test.Contains(0));
        }

        {
            var test = tree.Find(0, 0);
            Assert.AreEqual(2, test.Count);
            Assert.IsTrue(test.Contains(0));
            Assert.IsTrue(test.Contains(1));
        }

        {
            var test = tree.Find(4, 5, 6);
            Assert.AreEqual(4, test.Count);
            Assert.IsTrue(test.Contains(456));
            Assert.IsTrue(test.Contains(450));
            Assert.IsTrue(test.Contains(406));
            Assert.IsTrue(test.Contains(400));
        }

        {
            var test = tree.Find(4, 6, 6);
            Assert.AreEqual(2, test.Count);
            Assert.IsTrue(test.Contains(406));
            Assert.IsTrue(test.Contains(400));
        }

        {
            var test = tree.Find(4, 9, 9);
            Assert.AreEqual(1, test.Count);
            Assert.IsTrue(test.Contains(400));
        }

    }

}