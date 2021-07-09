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

        private ImmutableDictionary<string, ISegmentMatcher>? __StringSegmentCache;

        private ImmutableDictionary<string, ISegmentMatcher> StringSegmentCache
        {
            get
            {
                if(__StringSegmentCache == default)
                {
                    __StringSegmentCache = (
                        from x in Definitions
                        from y in x.Signature.Strings
                        let Key = Convert.ToBase64String(y.Pattern.AsSpan())
                        group y by Key
                        ).ToImmutableDictionary(x => x.Key, x => StringSegmentMatcher.Create(x.First()));
                }

                return __StringSegmentCache;
            }
        }

        public void WarmUp()
        {
            _ = StringSegmentCache;
        }

        public ImmutableArray<DefinitionMatch> Detect(IEnumerable<byte> Content)
        {
            return Detect(Content.ToImmutableArray());
        }

        public ImmutableArray<DefinitionMatch> Detect(ImmutableArray<byte> Content)
        {

            var SW1 = System.Diagnostics.Stopwatch.StartNew();
            var ret = Detect_v1(Content, Definitions);
            SW1.Stop();

            return ret;
        }

        
        protected ImmutableArray<DefinitionMatch> Detect_v1(ImmutableArray<byte> Content, ImmutableArray<Definition> Definitions)
        {
            var StringSegmentCache = this.StringSegmentCache;

            var ContentStrings = StringSegmentExtrator.ExtractStrings(Content);

            IEnumerable<Definition> Source = Definitions.Length > 5000
                ? Definitions.AsParallel()
                : Definitions
                ;

            

            var ret = (
                from x in Source
                let Match = MatchEvaluator.Match(x, StringSegmentCache, Content, ContentStrings)
                where Match is { }
                orderby Match.Points descending
                select Match
                ).ToImmutableArray();


            return ret;
        }

    }
}
