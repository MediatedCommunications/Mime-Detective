using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MimeDetective.Storage;
using System.Security.Cryptography;
using System.Net.Http.Headers;
using MimeDetective.Engine;

namespace MimeDetective.Tests
{


    [TestClass]
    public class PerfTests2 {
        [TestMethod]
        public void Foo() {
            var MaxLength = 5;

            var Data = new MimeDetective.Definitions.ExhaustiveBuilder() {
                UsageType = Definitions.Licensing.UsageType.PersonalNonCommercial
            }.Build();

            var SortedData = (
                from x in Data
                let Values = x.Signature.Prefix.OrderBy(x => x.Start).ToList()
                let ItemStart = Values.FirstOrDefault()?.Start ?? 0
                let ItemEnd = Values.LastOrDefault()?.ExclusiveEnd() ?? 0
                let v = new {
                    Signature = x,
                    Start = ItemStart,
                    End = ItemEnd
                }
                group v by v.Start
                ).ToDictionary(x => x.Key, x => x.ToList());

            var ByStart = SortedData.OrderByDescending(x => x.Value.Count).ToList();
                
            var Start = ByStart.FirstOrDefault();
            var StartIndex = Start.Key;

            var ByEnd = (
                from x in Start.Value
                let v = x
                group v by v.End
                ).ToDictionary(x => x.Key, x => x.ToList()).OrderByDescending(x => x.Value.Count).ToList();

            var End = (
                from x in ByEnd where x.Key < StartIndex + MaxLength
                select x
                ).FirstOrDefault();
            var InclusiveEndIndex = End.Key;

            var ExclusiveEndIndex = 1 + InclusiveEndIndex;
            var Length = ExclusiveEndIndex - StartIndex ;

            var Tree = new ByteTree<Definition>();
            foreach (var item in Data) {
                var Key = item.Signature.Prefix.TryGetRange(StartIndex, Length);

                Tree.Add(Key, item);

            }

            var EXE = System.IO.File.ReadAllBytes($@"C:\Windows\System32\Notepad.exe");
            var Content = GetRange(EXE, StartIndex, Length);

            var TestCount = 10000;

            var SW1 = System.Diagnostics.Stopwatch.StartNew();
            for (var i = 0; i < TestCount; i++) {
                var Matches = Tree.Find(Content);
            }
            SW1.Stop();


            var Searcher = new MimeDetective.ContentInspectorBuilder() {
                Definitions = Data,
                MatchEvaluatorOptions = new() {
                    Include_Segments_Strings = false
                },
            }.Build();

            var SW2 = System.Diagnostics.Stopwatch.StartNew();

            for (var i = 0; i < TestCount; i++) {
                var Matches = Searcher.Detect(EXE);
            }
            SW2.Stop();

        }

        private static byte[] GetRange(byte[] Content, int StartIndex, int Length) {
            var ret = new List<byte>();

            for (var i = 0; i < Length; i++) {
                var Position = StartIndex + i;

                if(Position >= 0 && Position < Content.Length) {
                    ret.Add(Content[Position]);
                }
            }


            return ret.ToArray();
        }

        [TestMethod]
        public void ByteTree_Test1() {
            var Tree = new ByteTree<int>();

            Tree.Add(new byte?[] { 0, 0, 0 }, 0);
            Tree.Add(new byte?[] { 0, 0, 1 }, 1);
            Tree.Add(new byte?[] { 0, 1, 0 }, 2);
            Tree.Add(new byte?[] { 0, 1, 1 }, 3);
            Tree.Add(new byte?[] { 1, 0, 0 }, 4);
            Tree.Add(new byte?[] { 1, 0, 1 }, 5);
            Tree.Add(new byte?[] { 1, 1, 0 }, 6);
            Tree.Add(new byte?[] { 1, 1, 1 }, 7);

            Tree.Add(new byte?[] { 4, 5, 6 }, 456);
            Tree.Add(new byte?[] { 4, 5, null }, 450);
            Tree.Add(new byte?[] { 4, null, 6 }, 406);
            Tree.Add(new byte?[] { 4, null, null }, 400);


            {
                var Test = Tree.Find();
                Assert.AreEqual(12, Test.Count);
            }

            {
                var Test = Tree.Find(0, 0, 0);
                Assert.AreEqual(1, Test.Count);
                Assert.IsTrue(Test.Contains(0));
            }

            {
                var Test = Tree.Find(0, 0);
                Assert.AreEqual(2, Test.Count);
                Assert.IsTrue(Test.Contains(0));
                Assert.IsTrue(Test.Contains(1));
            }

            {
                var Test = Tree.Find(4, 5, 6);
                Assert.AreEqual(4, Test.Count);
                Assert.IsTrue(Test.Contains(456));
                Assert.IsTrue(Test.Contains(450));
                Assert.IsTrue(Test.Contains(406));
                Assert.IsTrue(Test.Contains(400));
            }

            {
                var Test = Tree.Find(4, 6, 6);
                Assert.AreEqual(2, Test.Count);
                Assert.IsTrue(Test.Contains(406));
                Assert.IsTrue(Test.Contains(400));
            }

            {
                var Test = Tree.Find(4, 9, 9);
                Assert.AreEqual(1, Test.Count);
                Assert.IsTrue(Test.Contains(400));
            }

        }

    }

    
}
