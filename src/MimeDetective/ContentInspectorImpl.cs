using MimeDetective.Engine;
using MimeDetective.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective;

internal class ContentInspectorImpl(
    ImmutableArray<Definition> definitions,
    DefinitionMatchEvaluatorOptions matchEvaluatorOptions,
    StringSegmentMatcherProvider stringSegmentIndex,
    bool parallel)
    : IContentInspector {
    protected StringSegmentMatcherProvider StringSegmentIndex { get; } = stringSegmentIndex;

    protected DefinitionMatchEvaluatorOptions MatchEvaluatorOptions { get; } = matchEvaluatorOptions;

    protected ImmutableArray<DefinitionMatcher> DefinitionMatchers { get; } =
        GenerateDefinitionMatchers(stringSegmentIndex, definitions);

    protected bool Parallel { get; } = parallel;


    public ImmutableArray<DefinitionMatch> Inspect(ReadOnlySpan<byte> content) {
        var ret = Inspect_Current(content);

        return ret;
    }

    private static ImmutableArray<DefinitionMatcher> GenerateDefinitionMatchers(StringSegmentMatcherProvider stringSegmentIndex,
        ImmutableArray<Definition> definitions) {
        var prefixes =
            (from x in definitions from y in x.Signature.Prefix select y).Distinct(PrefixSegmentEqualityComparer.Instance);
        var strings =
            (from x in definitions from y in x.Signature.Strings select y).Distinct(StringSegmentEqualityComparer.Instance);

        var prefixMatcherCache = prefixes
            .ToImmutableDictionary(
                x => x,
                PrefixSegmentMatcher.Create,
                PrefixSegmentEqualityComparer.Instance
            );

        var stringMatcherCache = strings
            .ToImmutableDictionary(
                x => x,
                stringSegmentIndex.CreateMatcher,
                StringSegmentEqualityComparer.Instance
            );

        var ret = (
            from Definition in definitions
            let PrefixMatchers = (
                from y in Definition.Signature.Prefix
                select prefixMatcherCache[y]
            ).ToImmutableArray()
            let StringMatchers = (
                from y in Definition.Signature.Strings
                select stringMatcherCache[y]
            ).ToImmutableArray()
            select new DefinitionMatcher(Definition) {
                Prefixes = PrefixMatchers,
                Strings = StringMatchers
            }).ToImmutableArray();


        return ret;
    }

    protected ImmutableArray<DefinitionMatch> Inspect_Current(ReadOnlySpan<byte> content) {
        var noContentStrings = ImmutableArray<StringSegment>.Empty;

        var tret = new List<(DefinitionMatch Match, DefinitionMatcher Matcher)>(32);

        var useEmptyShortcut = MatchEvaluatorOptions is { IncludeSegmentsStrings: false, IncludeMatchesEmpty: false };
        var needsStrings = false;

        {
            var matchEvaluator1 = new DefinitionMatchEvaluator {
                Options = MatchEvaluatorOptions with {
                    IncludeSegmentsStrings = false,
                    IncludeMatchesEmpty = true
                }
            };


            //Get an initial list of matches that don't include string matches
            foreach (var matcher in DefinitionMatchers) {
                var match = matchEvaluator1.Match(matcher, content, noContentStrings);
                if (match is null) {
                    continue;
                }

                //If we don't need strings, and we don't want empty, just shortcut.
                if (useEmptyShortcut && match.Type == DefinitionMatchType.Empty) {
                    continue;
                }

                if (matcher.Strings.Length > 0) {
                    needsStrings = true;
                }

                tret.Add((match, matcher));
            }
        }

        if (!useEmptyShortcut && needsStrings) {
            var matchEvaluator2 = new DefinitionMatchEvaluator { Options = MatchEvaluatorOptions };

            var contentStrings = StringSegmentExtrator.ExtractStrings(content);

            var output = 0;
            for (var i = 0; i < tret.Count; ++i) {
                var (_, matcher) = tret[i];

                var match = matchEvaluator2.Match(matcher, content, contentStrings);
                if (match is null) {
                    continue;
                }

                tret[output++] = (match, matcher);
            }

            tret.RemoveRange(output, tret.Count - output);
        }

        tret.Sort(static (a, b) => Comparer<long>.Default.Compare(a.Match.Points, b.Match.Points));

        var ret = new DefinitionMatch[tret.Count];

        for (var i = 0; i < tret.Count; ++i) {
            ret[i] = tret[i].Match;
        }

        return [.. ret];
    }
}
