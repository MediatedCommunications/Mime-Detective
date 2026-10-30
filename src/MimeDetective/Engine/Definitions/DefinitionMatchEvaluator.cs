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
        DefinitionMatcher definitionMatcherCache, ReadOnlySpan<byte> content, ImmutableArray<WildCardRule> contentStrings) {
        var ret = default(DefinitionMatch?);

        var valid = true;
        var allMatches = 0m;
        var goodMatches = 0m;

        var OffsetMatches = new Dictionary<OffsetRule, Match>();
        var WildCardMatches = new Dictionary<WildCardRule, Match>();

        if (valid && Options.IncludeOffsetRules) {
            foreach (var item in definitionMatcherCache.Prefixes) {
                allMatches += 1;

                var result = item.Match(content);
                OffsetMatches[item.Segment] = result;

                if (result == NoMatch.Instance) {
                    if (Options.IncludeMatchesPartial == false && Options.IncludeMatchesFailed == false) {
                        valid = false;
                        break;
                    }
                } else {
                    goodMatches += 1;
                }
            }
        }

        if (valid && Options.IncludeWildcardRules) {
            foreach (var item in definitionMatcherCache.Strings) {
                allMatches += 1;

                var result = contentStrings
                            .Select(x => item.Match(x.Value.AsSpan()))
                            .FirstOrDefault(x => x != NoMatch.Instance)
                        ?? NoMatch.Instance
                    ;

                WildCardMatches[item.Segment] = result;

                if (result == NoMatch.Instance) {
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

            var points = GetPoints(OffsetMatches, WildCardMatches);

            var type = (GoodMatches: goodMatches, AllMatches: allMatches) switch {
                _ when goodMatches == allMatches && goodMatches == 0 => DefinitionMatchType.Empty,
                _ when goodMatches == allMatches && goodMatches != 0 => DefinitionMatchType.Complete,
                _ when goodMatches != allMatches && goodMatches > 0 => DefinitionMatchType.Partial,
                _ => DefinitionMatchType.Failed,
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
                    OffsetMatches = OffsetMatches.ToImmutableDictionary(),
                    WildCardMatches = WildCardMatches.ToImmutableDictionary(),
                    Percentage = percentage,
                    Points = points,
                };
            }
        }

        return ret;
    }

    protected virtual long GetPoints(IDictionary<OffsetRule, Match> OffsetMatches,
        IDictionary<WildCardRule, Match> WildCardMatches) {
        var OffsetRuleStart = 0;

        var ret = 0;
        var matchSet = new[] { OffsetMatches.Values, WildCardMatches.Values, };

        var matches = matchSet.SelectMany(x => x);

        var multiplierPrefixStartOfFile = 1000;
        var multiplierPrefixBeginningOfFile = 5;
        var multiplierStringAnywhere = 1;


        foreach (var match in matches) {
            var pointsToAdd = 0;

            if (match is NoMatch v1) { } else if (match is OffsetMatch v2) {
                var multiplier = multiplierPrefixBeginningOfFile;
                if (v2.Rule.Start == OffsetRuleStart) {
                    multiplier = multiplierPrefixStartOfFile;
                    OffsetRuleStart = v2.Rule.ExclusiveEnd();
                }

                pointsToAdd = multiplier * v2.Rule.Value.Length;
            } else if (match is WildCardMatch v3) {
                var multiplier = multiplierStringAnywhere;

                pointsToAdd = multiplier * v3.Rule.Value.Length;
            }

            ret += pointsToAdd;
        }


        return ret;
    }
}
