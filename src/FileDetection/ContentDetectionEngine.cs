using FileDetection.Engine;
using FileDetection.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FileDetection {

    internal class ContentDetectionEngine : IContentDetectionEngine
    {
        public ContentDetectionEngine(ImmutableArray<Definition> Definitions, DefinitionMatchEvaluatorOptions MatchEvaluatorOptions, bool Parallel) {
            this.MatchEvaluatorOptions = MatchEvaluatorOptions;
            this.MatcherCaches = GenerateMatcherCaches(Definitions);
            this.Parallel = Parallel;
        }

        protected DefinitionMatchEvaluatorOptions MatchEvaluatorOptions { get; }
        protected ImmutableArray<ContentDetectionEngineCache> MatcherCaches { get; }
        protected bool Parallel { get; }

        private static ImmutableArray<ContentDetectionEngineCache> GenerateMatcherCaches(ImmutableArray<Definition> Definitions) {

            var ExtensionCache = (
                from x in Definitions
                from y in x.File.Extensions
                select y.ToLower()
                )
                .Distinct(StringComparer.InvariantCultureIgnoreCase)
                .ToImmutableDictionary(x => x, x => x, StringComparer.InvariantCultureIgnoreCase);

            var MimeTypeCache = (
                from x in Definitions
                let v = x.File.MimeType?.ToLower()
                where v is { }
                select v
                )
                .Distinct(StringComparer.InvariantCultureIgnoreCase)
                .ToImmutableDictionary(x => x, x => x, StringComparer.InvariantCultureIgnoreCase);

            var DescriptionCache = (
                from x in Definitions
                let v = x.File.Description
                where v is { }
                select v
                )
                .Distinct(StringComparer.InvariantCultureIgnoreCase)
                .ToImmutableDictionary(
                x => x, 
                x => x, 
                StringComparer.InvariantCultureIgnoreCase
                );

            var PrefixCache = (
                from x in Definitions
                from y in x.Signature.Prefix
                select y
                )
                .Distinct(PrefixSegmentEqualityComparer.Instance)
                .ToImmutableDictionary(
                x => x, 
                x => x,
                PrefixSegmentEqualityComparer.Instance
                );

            var PrefixMatcherCache = PrefixCache.Keys
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
                x => x,
                StringSegmentEqualityComparer.Instance
                );

            var StringMatcherCache = StringCache.Keys
                .ToImmutableDictionary(
                x => x,
                x => StringSegmentMatcher.Create(x),
                StringSegmentEqualityComparer.Instance
                );

            var ret = (
                from Definition in Definitions

                let Description = Definition.File.Description is { } V1 ? DescriptionCache[V1] : default

                let Extensions = (
                    from y in Definition.File.Extensions
                    select ExtensionCache[y]
                ).ToImmutableArray()

                let MimeType = Definition.File.MimeType is { } V1 ? MimeTypeCache[V1] : default

                

                let Prefixes = (
                    from y in Definition.Signature.Prefix
                    select PrefixCache[y]
                    ).ToImmutableArray()

                let Strings = (
                    from y in Definition.Signature.Strings
                    select StringCache[y]
                    ).ToImmutableArray()

                let PrefixMatchers = (
                    from y in Prefixes
                    select PrefixMatcherCache[y]
                ).ToImmutableArray()

                let StringMatchers = (
                    from y in Strings
                    select StringMatcherCache[y]
                    ).ToImmutableArray()

                    let NewDef = new Definition() {
                        File = new FileType() {
                            Description = Description,
                            Extensions = Extensions,
                            MimeType = MimeType,
                        },
                        Meta = Definition.Meta,
                        Signature = new Signature() {
                            Prefix = Prefixes,
                            Strings = Strings,
                        }
                    }

                select new ContentDetectionEngineCache(NewDef) {
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

        private static ParallelQuery<T> ApplyParallel<T>(IEnumerable<T> Source, bool Parallel) {
            var ret = Source.AsParallel();

            if (Parallel) {
                ret = ret
                    .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                    .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
                    ;
            } else {
                ret = ret.WithExecutionMode(ParallelExecutionMode.Default)
                    .WithDegreeOfParallelism(1)
                    ;
            }

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

            var MatcherCaches = this.MatcherCaches;


            var tret = (
                from MatcherCache in ApplyParallel(MatcherCaches, Parallel)

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
                        from x in ApplyParallel(Source2, Parallel)
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
