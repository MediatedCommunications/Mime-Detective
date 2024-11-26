using MimeDetective.Engine;
using MimeDetective.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective {


    internal class ContentInspectorImpl
        : IContentInspector {
        public ContentInspectorImpl(
            ImmutableArray<Definition> Definitions,
            DefinitionMatchEvaluatorOptions MatchEvaluatorOptions,
            StringSegmentMatcherProvider StringSegmentIndex,
            bool Parallel
            ) {

            this.StringSegmentIndex = StringSegmentIndex;
            this.MatchEvaluatorOptions = MatchEvaluatorOptions;
            this.Parallel = Parallel;
            this.DefinitionMatchers = GenerateDefinitionMatchers(StringSegmentIndex, Definitions);
        }

        protected StringSegmentMatcherProvider StringSegmentIndex { get; }

        protected DefinitionMatchEvaluatorOptions MatchEvaluatorOptions { get; }
        protected ImmutableArray<DefinitionMatcher> DefinitionMatchers { get; }
        protected bool Parallel { get; }

        private static ImmutableArray<DefinitionMatcher> GenerateDefinitionMatchers(StringSegmentMatcherProvider StringSegmentIndex, ImmutableArray<Definition> Definitions) {

            var Prefixes = (from x in Definitions from y in x.Signature.Prefix select y).Distinct(PrefixSegmentEqualityComparer.Instance);
            var Strings = (from x in Definitions from y in x.Signature.Strings select y).Distinct(StringSegmentEqualityComparer.Instance);

            var PrefixMatcherCache = Prefixes
                .ToImmutableDictionary(
                x => x,
                PrefixSegmentMatcher.Create,
                PrefixSegmentEqualityComparer.Instance
                );

            var StringMatcherCache = Strings
                .ToImmutableDictionary(
                x => x,
                StringSegmentIndex.CreateMatcher,
                StringSegmentEqualityComparer.Instance
                );

            var ret = (
                from Definition in Definitions

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


        public ImmutableArray<DefinitionMatch> Inspect(ReadOnlySpan<byte> Content) {
            var ret = Inspect_Current(Content);

            return ret;
        }

        protected ImmutableArray<DefinitionMatch> Inspect_Current(ReadOnlySpan<byte> Content) {

            var NoContentStrings = ImmutableArray<StringSegment>.Empty;

            var tret = new List<(DefinitionMatch Match, DefinitionMatcher Matcher)>(32);

            var useEmptyShortcut = this.MatchEvaluatorOptions is { Include_Segments_Strings: false, Include_Matches_Empty: false };
            var needsStrings = false;

            {
                var MatchEvaluator1 = new DefinitionMatchEvaluator {
                    Options = MatchEvaluatorOptions with {
                        Include_Segments_Strings = false,
                        Include_Matches_Empty = true,
                    }
                };


                //Get an initial list of matches that don't include string matches
                foreach (var matcher in this.DefinitionMatchers) {
                    var match = MatchEvaluator1.Match(matcher, Content, NoContentStrings);
                    if (match is null)
                        continue;

                    //If we don't need strings, and we don't want empty, just shortcut.
                    if (useEmptyShortcut && match.Type == DefinitionMatchType.Empty)
                        continue;

                    if (matcher.Strings.Length > 0)
                        needsStrings = true;

                    tret.Add((match, matcher));
                }
            }

            if (!useEmptyShortcut && needsStrings) {
                var MatchEvaluator2 = new DefinitionMatchEvaluator { Options = this.MatchEvaluatorOptions };

                var ContentStrings = StringSegmentExtrator.ExtractStrings(Content);

                var output = 0;
                for (var i = 0; i < tret.Count; ++i) {
                    var item = tret[i];

                    var match = MatchEvaluator2.Match(item.Matcher, Content, ContentStrings);
                    if (match is null)
                        continue;

                    tret[output++] = (match, item.Matcher);
                }

                tret.RemoveRange(output, tret.Count - output);
            }

            tret.Sort(static (a, b) => Comparer<long>.Default.Compare(a.Match.Points, b.Match.Points));

            var ret = new DefinitionMatch[tret.Count];

            for (var i = 0; i < tret.Count; ++i)
                ret[i] = tret[i].Match;

            return ret.ToImmutableArray();
        }

    }
}
