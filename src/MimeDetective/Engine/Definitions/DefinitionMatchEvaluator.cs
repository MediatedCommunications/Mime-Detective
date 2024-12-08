using MimeDetective.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective.Engine;

/// <summary>
///     Handles evaluating a <see cref="Definition" /> against content and scoring it.
/// </summary>
internal class DefinitionMatchEvaluator {
    public DefinitionMatchEvaluatorOptions Options { get; init; } = new();

    public virtual DefinitionMatch? Match(
        DefinitionMatcher definitionMatcherCache, ReadOnlySpan<byte> content, ImmutableArray<StringSegment> contentStrings) {
        var ret = default(DefinitionMatch?);

        var valid = true;
        var allMatches = 0m;
        var goodMatches = 0m;

        var prefixSegmentMatches = new Dictionary<PrefixSegment, SegmentMatch>();
        var stringSegmentMatches = new Dictionary<StringSegment, SegmentMatch>();

        if (valid && Options.IncludeSegmentsPrefix) {
            foreach (var item in definitionMatcherCache.Prefixes) {
                allMatches += 1;

                var result = item.Match(content);
                prefixSegmentMatches[item.Segment] = result;

                if (result == NoSegmentMatch.Instance) {
                    if (Options.IncludeMatchesPartial == false && Options.IncludeMatchesFailed == false) {
                        valid = false;
                        break;
                    }
                } else {
                    goodMatches += 1;
                }
            }
        }

        if (valid && Options.IncludeSegmentsStrings) {
            foreach (var item in definitionMatcherCache.Strings) {
                allMatches += 1;

                var result = contentStrings
                            .Select(x => item.Match(x.Pattern.AsSpan()))
                            .FirstOrDefault(x => x != NoSegmentMatch.Instance)
                        ?? NoSegmentMatch.Instance
                    ;

                stringSegmentMatches[item.Segment] = result;

                if (result == NoSegmentMatch.Instance) {
                    if (Options.IncludeMatchesPartial == false && Options.IncludeMatchesFailed == false) {
                        valid = false;
                        break;
                    }
                } else {
                    goodMatches += 1;
                }
            }
        }

        if (goodMatches == 0 && Options.IncludeMatchesEmpty == false) {
            valid = false;
        }


        if (valid) {
            var percentage = allMatches == 0
                    ? 1
                    : goodMatches / allMatches
                ;

            var points = GetPoints(prefixSegmentMatches, stringSegmentMatches);

            var type = (GoodMatches: goodMatches, AllMatches: allMatches) switch {
                _ when goodMatches == allMatches && goodMatches == 0 => DefinitionMatchType.Empty,
                _ when goodMatches == allMatches && goodMatches != 0 => DefinitionMatchType.Complete,
                _ when goodMatches != allMatches && goodMatches > 0 => DefinitionMatchType.Partial,
                _ => DefinitionMatchType.Failed
            };

            valid = false
                || (type == DefinitionMatchType.Complete && Options.IncludeMatchesComplete)
                || (type == DefinitionMatchType.Partial && Options.IncludeMatchesPartial)
                || (type == DefinitionMatchType.Empty && Options.IncludeMatchesEmpty)
                || (type == DefinitionMatchType.Failed && Options.IncludeMatchesFailed)
                ;

            if (valid) {
                ret = new() {
                    Definition = definitionMatcherCache.Definition,
                    Type = type,
                    PrefixSegmentMatches = prefixSegmentMatches.ToImmutableDictionary(),
                    StringSegmentMatches = stringSegmentMatches.ToImmutableDictionary(),
                    Percentage = percentage,
                    Points = points
                };
            }
        }

        return ret;
    }

    protected virtual long GetPoints(IDictionary<PrefixSegment, SegmentMatch> prefixSegmentMatches,
        IDictionary<StringSegment, SegmentMatch> stringSegmentMatches) {
        var prefixSegmentStart = 0;

        var ret = 0;
        var matchSet = new[] { prefixSegmentMatches.Values, stringSegmentMatches.Values };

        var matches = matchSet.SelectMany(x => x);

        var multiplierPrefixStartOfFile = 1000;
        var multiplierPrefixBeginningOfFile = 5;
        var multiplierStringAnywhere = 1;


        foreach (var match in matches) {
            var pointsToAdd = 0;

            if (match is NoSegmentMatch v1) { } else if (match is PrefixSegmentMatch v2) {
                var multiplier = multiplierPrefixBeginningOfFile;
                if (v2.Segment.Start == prefixSegmentStart) {
                    multiplier = multiplierPrefixStartOfFile;
                    prefixSegmentStart = v2.Segment.ExclusiveEnd();
                }

                pointsToAdd = multiplier * v2.Segment.Pattern.Length;
            } else if (match is StringSegmentMatch v3) {
                var multiplier = multiplierStringAnywhere;

                pointsToAdd = multiplier * v3.Segment.Pattern.Length;
            }

            ret += pointsToAdd;
        }


        return ret;
    }
}
