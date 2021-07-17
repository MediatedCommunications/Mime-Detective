using MimeDetective.Definitions;
using MimeDetective.Engine;
using MimeDetective.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MimeDetective {


    internal class ContentDetectionEngine : IContentDetectionEngine
    {
        public ContentDetectionEngine(
            ImmutableArray<Definition> Definitions, 
            DefinitionMatchEvaluatorOptions MatchEvaluatorOptions,
            PrefixSegmentFilterProvider PrefixFilterProvider,
            StringSegmentMatcherProvider StringSegmentIndex,
            bool Parallel
            ) {
            this.StringSegmentIndex = StringSegmentIndex;
            this.MatchEvaluatorOptions = MatchEvaluatorOptions;
            this.Parallel = Parallel;
            this.DefinitionMatchers = GenerateDefinitionMatchers(StringSegmentIndex, Definitions);

        }

        protected StringSegmentMatcherProvider StringSegmentIndex { get; set; }

        protected DefinitionMatchEvaluatorOptions MatchEvaluatorOptions { get; }
        protected ImmutableArray<DefinitionMatcher> DefinitionMatchers { get; }
        protected bool Parallel { get; }

        private static ImmutableArray<DefinitionMatcher> GenerateDefinitionMatchers(StringSegmentMatcherProvider StringSegmentIndex, ImmutableArray<Definition> Definitions) {

            var Deduplicate = Definitions.Deduplicate();

            var PrefixMatcherCache = Deduplicate.Prefixes.Keys
                .ToImmutableDictionary(
                x => x,
                x => PrefixSegmentMatcher.Create(x),
                PrefixSegmentEqualityComparer.Instance
                );

            var StringMatcherCache = Deduplicate.Strings.Keys
                .ToImmutableDictionary(
                x => x,
                x => StringSegmentIndex.CreateMatcher(x),
                StringSegmentEqualityComparer.Instance
                );

            var ret = (
                from Definition in Deduplicate.Definitions

                let PrefixMatchers = (
                    from y in Definition.Signature.Prefix
                    select PrefixMatcherCache[y]
                ).ToImmutableArray()

                let StringMatchers = (
                    from y in Definition.Signature.Strings
                    select StringMatcherCache[y]
                    ).ToImmutableArray()

                select new DefinitionMatcher(Definition) {
                    Prefixes = PrefixMatchers,
                    Strings = StringMatchers,
                }).ToImmutableArray();


            return ret;
        }


        public ImmutableArray<DefinitionMatch> Detect(ImmutableArray<byte> Content)
        {
            var ret = Detect_v1(Content);

            return ret;
        }
        
        protected ImmutableArray<DefinitionMatch> Detect_v1(ImmutableArray<byte> Content)
        {

            var NoContentStrings = ImmutableArray<StringSegment>.Empty;


            var MatchEvaluator1 = new DefinitionMatchEvaluator()
            {
                Options = MatchEvaluatorOptions with
                {
                    Include_Segments_Strings = false
                }
            };

            var MatcherCaches = this.DefinitionMatchers;


            var tret = (
                from MatcherCache in MatcherCaches.AsParallel(Parallel)

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
                        from x in Source2.AsParallel(Parallel)
                        let Match = MatchEvaluator2.Match(
                            x.MatcherCache,
                            Content,
                            ContentStrings
                            )
                        where Match is { }
                        orderby Match.Points descending
                        select new {
                            Match,
                            x.MatcherCache,
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
