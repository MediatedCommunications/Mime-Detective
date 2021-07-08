using FileDetection.Data.Engine;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDetection
{

    public class FileDetectionEngine
    {
        public DefinitionMatchEvaluator MatchEvaluator { get; init; } = new();
        public ImmutableArray<Definition> Definitions { get; init; } = ImmutableArray<Definition>.Empty;


        public ImmutableArray<DefinitionMatch> Detect(IEnumerable<byte> Content)
        {
            return Detect(Content.ToImmutableArray());
        }

        public ImmutableArray<DefinitionMatch> Detect(ImmutableArray<byte> Content)
        {
            var OriginalContent = Content;

            var First = (byte)'a';
            var Last = (byte)'z';
            var NewFirst = (byte)'A';
            var Delta = First - NewFirst;

            var UpperContent = (
                from x in Content
                let v = x >= First && x <= Last ? x - Delta : x
                select (byte)v
                ).ToImmutableArray();


            IEnumerable<Definition> Source = Definitions.Length > 5000
                ? Definitions.AsParallel()
                : Definitions
                ;


            var ret = (
                from x in Source
                let Match = MatchEvaluator.Match(x, Content, UpperContent)
                where Match is { }
                orderby Match.Points descending
                select Match
                ).ToImmutableArray();


            return ret;
        }

    }
}
