using FileDetection.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace FileDetection.Engine
{

    /// <summary>
    /// Handles evaluating a <see cref="Definition"/> against content and scoring it.
    /// </summary>
    internal class DefinitionMatchEvaluator
    {
        public DefinitionMatchEvaluatorOptions Options { get; init; } = new();

        public virtual DefinitionMatch? Match(
            ContentDetectionEngineCache DefinitionMatcherCache, ImmutableArray<byte> Content, ImmutableArray<StringSegment> ContentStrings)
        {
            var ret = default(DefinitionMatch?);

            var Valid = true;
            var AllMatches = 0m;
            var GoodMatches = 0m;

            var PrefixSegmentMatches = new Dictionary<PrefixSegment, SegmentMatch>();
            var StringSegmentMatches = new Dictionary<StringSegment, SegmentMatch>();

            if (Valid && Options.Include_Segments_Prefix)
            {



                foreach (var item in DefinitionMatcherCache.Prefixes)
                {

                    AllMatches += 1;

                    var Result = item.Match(Content);
                    PrefixSegmentMatches[item.Segment] = Result;

                    if (Result == NoSegmentMatch.Instance)
                    {
                        if (Options.Include_Matches_Partial == false && Options.Include_Matches_Failed == false)
                        {
                            Valid = false;
                            break;
                        }
                    } else
                    {
                        GoodMatches += 1;
                    }

                }
            }

            if (Valid && Options.Include_Segments_Strings)
            {
                foreach (var item in DefinitionMatcherCache.Strings)
                {
                    
                    AllMatches += 1;

                    var Result = ContentStrings
                        .Select(x => item.Match(x.Pattern))
                        .Where(x => x != NoSegmentMatch.Instance)
                        .FirstOrDefault() 
                        ?? NoSegmentMatch.Instance
                        ;

                    StringSegmentMatches[item.Segment] = Result;

                    if (Result == NoSegmentMatch.Instance)
                    {
                        if (Options.Include_Matches_Partial == false && Options.Include_Matches_Failed == false)
                        {
                            Valid = false;
                            break;
                        }
                    } else
                    {
                        GoodMatches += 1;
                    }

                }
            }

            if (GoodMatches == 0 && !Options.Include_Matches_Failed)
            {
                Valid = false;
            }


            if (Valid)
            {
                var Percentage = AllMatches == 0
                    ? 1
                    : GoodMatches / AllMatches
                    ;

                var Points = GetPoints(PrefixSegmentMatches, StringSegmentMatches);

                ret = new DefinitionMatch(DefinitionMatcherCache.Definition)
                {
                    PrefixSegmentMatches = PrefixSegmentMatches.ToImmutableDictionary(),
                    StringSegmentMatches = StringSegmentMatches.ToImmutableDictionary(),

                    Percentage = Percentage,
                    Points = Points,
                };
            }

            return ret;
        }

        protected virtual long GetPoints(IDictionary<PrefixSegment, SegmentMatch> PrefixSegmentMatches, IDictionary<StringSegment, SegmentMatch> StringSegmentMatches)
        {
            var ret = 0;
            var MatchSet = new[]
            {
                PrefixSegmentMatches.Values,
                StringSegmentMatches.Values
            };

            var Matches = MatchSet.SelectMany(x => x);

            var Multiplier_Prefix_StartOfFile = 1000;
            var Multiplier_Prefix_BeginningOfFile = 5;
            var Multiplier_String_Anywhere = 1;


            foreach (var Match in Matches)
            {
                var PointsToAdd = 0;
                
                if(Match is NoSegmentMatch V1)
                {

                } else if (Match is PrefixSegmentMatch V2)
                {
                    var Multiplier = V2.Segment.Start == 0
                        ? Multiplier_Prefix_StartOfFile
                        : Multiplier_Prefix_BeginningOfFile
                        ;
                    
                    PointsToAdd = Multiplier * V2.Segment.Pattern.Length;

                } else if (Match is StringSegmentMatch V3)
                {
                    var Multiplier = Multiplier_String_Anywhere;

                    PointsToAdd = Multiplier * V3.Segment.Pattern.Length;
                }
                ret += PointsToAdd;
            }



            return ret;
        }
    }

}
