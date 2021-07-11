using FileDetection.Engine;
using FileDetection.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FileDetection {

    public class FileDetectionEngine
    {
        public DefinitionMatchEvaluatorOptions MatchEvaluatorOptions { get; init; } = new();
        public ImmutableArray<Definition> Definitions { get; init; } = ImmutableArray<Definition>.Empty;

        private ImmutableArray<FileDetectionEngineCache>? MatcherCaches_Values;

        private ImmutableArray<FileDetectionEngineCache> MatcherCaches
        {
            get
            {

                if (MatcherCaches_Values == default)
                {
                    var PrefixCache = (
                        from x in Definitions
                        from y in x.Signature.Prefix
                        select y
                        ).Distinct(PrefixSegmentEqualityComparer.Instance)
                        .ToImmutableDictionary(
                        x => x, 
                        x => PrefixSegmentMatcher.Create(x),
                        PrefixSegmentEqualityComparer.Instance
                        );

                    var StringCache = (
                        from x in Definitions
                        from y in x.Signature.Strings
                        select y
                        )
                        .Distinct(StringSegmentEqualityComparer.Instance)
                        .ToImmutableDictionary(
                        x => x, 
                        x => StringSegmentMatcher.Create(x),
                        StringSegmentEqualityComparer.Instance
                        );

                    MatcherCaches_Values = (
                        from Definition in Definitions
                        let Prefixes = (
                            from y in Definition.Signature.Prefix
                            select PrefixCache[y]
                        ).ToImmutableArray()
                        let Strings = (
                        from y in Definition.Signature.Strings
                        select StringCache[y]
                        ).ToImmutableArray()
                        select new FileDetectionEngineCache(Definition) {
                            Prefixes = Prefixes,
                            Strings = Strings,
                        }).ToImmutableArray();
               
                }

                var ret = MatcherCaches_Values ?? ImmutableArray<FileDetectionEngineCache>.Empty;

                return ret;
            }
        }

        public void WarmUp()
        {
            _ = MatcherCaches;
        }

        public ImmutableArray<DefinitionMatch> Detect(IEnumerable<byte> Content)
        {
            return Detect(Content.ToImmutableArray());
        }

        public ImmutableArray<DefinitionMatch> Detect(ImmutableArray<byte> Content)
        {

            LicenseValidator.ThrowIfUnlicensed(this);

            var ret = Detect_v1(Content, Definitions);

            return ret;
        }

        
        protected ImmutableArray<DefinitionMatch> Detect_v1(ImmutableArray<byte> Content, ImmutableArray<Definition> Definitions)
        {

            var NoContentStrings = ImmutableArray<StringSegment>.Empty;


            var Source1 = Definitions.AsParallel();

            var MatchEvaluator1 = new DefinitionMatchEvaluator()
            {
                Options = MatchEvaluatorOptions with
                {
                    Include_Segments_Strings = false
                }
            };

            var MatcherCaches = this.MatcherCaches;


            var tret = (
                from MatcherCache in MatcherCaches
                .AsParallel()
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .WithMergeOptions(ParallelMergeOptions.FullyBuffered)

                let Match = MatchEvaluator1.Match(
                    MatcherCache, 
                    Content, 
                    NoContentStrings
                    )
                where Match is { }
                orderby Match.Points descending
                select new {
                    Match,
                    MatcherCache
                }
                ).ToImmutableArray();


            if (MatchEvaluatorOptions.Include_Segments_Strings) {

                var Source2 = tret;

                var NeedsStrings = (
                    from x in Source2
                    where x.MatcherCache.Strings.Length > 0
                    select x
                    ).Any()
                    ;
                if(Source2.Length >= 2 && NeedsStrings)
                {
                    var MatchEvaluator2 = new DefinitionMatchEvaluator()
                    {
                        Options = MatchEvaluatorOptions
                    };

                    var ContentStrings = StringSegmentExtrator.ExtractStrings(Content);


                    tret = (
                        from x in Source2
                        .AsParallel()
                        .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                        .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
                        let Match = MatchEvaluator2.Match(
                            x.MatcherCache,
                            Content,
                            ContentStrings
                            )
                        where Match is { }
                        orderby Match.Points descending
                        select new {
                            Match = Match,
                            MatcherCache = x.MatcherCache,
                        }
                        ).ToImmutableArray();

                } else
                {

                }

            }

            var ret = (
                from x in tret
                select x.Match
                ).ToImmutableArray();

            return ret;
        }

    }
}
