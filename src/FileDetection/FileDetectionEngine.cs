using FileDetection.Engine;
using FileDetection.Storage;
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
        public DefinitionMatchEvaluatorOptions MatchEvaluatorOptions { get; init; } = new();
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

            var NoContentStrings = ImmutableArray<StringSegment>.Empty;


            var Source1 = Definitions.AsParallel();

            var MatchEvaluator1 = new DefinitionMatchEvaluator()
            {
                Options = MatchEvaluatorOptions with
                {
                    Include_Segments_Strings = false
                }
            };

            var ret = (
                from x in Source1
                let Match = MatchEvaluator1.Match(
                    x, 
                    StringSegmentCache, 
                    Content, 
                    NoContentStrings
                    )
                where Match is { }
                orderby Match.Points descending
                select Match
                ).ToImmutableArray();


            if (MatchEvaluatorOptions.Include_Segments_Strings) {

                var Source2 = (
                    from x in ret
                    select x.Definition
                    ).ToImmutableArray();

                var NeedingStrings = (
                    from x in Source2
                    where x.Signature.Strings.Length > 0
                    select x
                    ).ToImmutableArray();

                if(Source2.Length >= 2 && NeedingStrings.Length >= 1)
                {
                    var MatchEvaluator2 = new DefinitionMatchEvaluator()
                    {
                        Options = MatchEvaluatorOptions
                    };

                    var ContentStrings = StringSegmentExtrator.ExtractStrings(Content);

                    ret = (
                        from x in Source2
                        .AsParallel()
                        .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                        //.WithMergeOptions(ParallelMergeOptions.NotBuffered)
                        let Match = MatchEvaluator2.Match(
                            x,
                            StringSegmentCache,
                            Content,
                            ContentStrings
                            )
                        where Match is { }
                        orderby Match.Points descending
                        select Match
                        ).ToImmutableArray();

                } else
                {

                }

            }

            return ret;
        }

    }
}
