using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using MimeDetective.Storage;
using MimeDetective.Engine;

namespace MimeDetective.Tests
{


    [TestClass]
    public class PerfTests2 {
        [TestMethod]
        public void Foo() {
            var MaxLength = 5;

            var Data = new MimeDetective.Definitions.ExhaustiveBuilder() {
                UsageType = Definitions.Licensing.UsageType.PersonalNonCommercial,
            }.Build();

            var SortedData = (
                from x in Data
                let Values = x.Signature.Prefix.OrderBy(p => p.Start).ToList()
                let ItemStart = Values.FirstOrDefault()?.Start ?? 0
                let ItemEnd = Values.LastOrDefault()?.ExclusiveEnd() ?? 0
                let v = new {
                    Signature = x,
                    Start = ItemStart,
                    End = ItemEnd,
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

            var EXE = System.IO.File.ReadAllBytes($@"C:\Windows\System32\Notepad.exe");
            var Content = GetRange(EXE, StartIndex, Length);

            var TestCount = 10000;

            var Searcher = new MimeDetective.ContentInspectorBuilder() {
                Definitions = Data,
                MatchEvaluatorOptions = new() {
                    IncludeSegmentsStrings = false,
                },
            }.Build();

            var SW2 = System.Diagnostics.Stopwatch.StartNew();

            for (var i = 0; i < TestCount; i++) {
                var Matches = Searcher.Inspect(EXE);
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


            return [.. ret,];
        }

    }

    
}
