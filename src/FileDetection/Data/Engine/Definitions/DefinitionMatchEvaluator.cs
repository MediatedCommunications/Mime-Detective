using FileDetection.Data.Engine;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace FileDetection.Data.Engine
{

    /// <summary>
    /// Handles evaluating a <see cref="Definition"/> against content and scoring it.
    /// </summary>
    public class DefinitionMatchEvaluator
    {
        public DefinitionMatchEvaluatorOptions Options { get; init; } = new();

        public virtual DefinitionMatch? Match(Definition Definition, ImmutableDictionary<string, ISegmentMatcher> StringSegmentCache, ImmutableArray<byte> Content, ImmutableArray<StringSegment> Strings)
        {
            var ret = default(DefinitionMatch?);

            var Valid = true;
            var AllMatches = 0m;
            var GoodMatches = 0m;

            var FrontSegmentMatches = new Dictionary<PrefixSegment, SegmentMatch>();
            var AnySegmentMatches = new Dictionary<StringSegment, SegmentMatch>();

            if (Valid && Options.Include_Segments_Beginning)
            {

                foreach (var item in Definition.Signature.Prefix)
                {

                    AllMatches += 1;

                    var Result = item.GetMatch(Content);
                    FrontSegmentMatches[item] = Result;

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

            if (Valid && Options.Include_Segments_Middle)
            {
                foreach (var item in Definition.Signature.Strings)
                {
                    var Key = Convert.ToBase64String(item.Pattern.ToArray());
                    if(!StringSegmentCache.TryGetValue(Key, out var Matcher))
                    {
                        Matcher = StringSegmentMatcher.Create(item);
                    }

                    AllMatches += 1;

                    var Result = Strings
                        .Select(x => Matcher.Match(x.Pattern))
                        .Where(x => x != NoSegmentMatch.Instance)
                        .FirstOrDefault() 
                        ?? NoSegmentMatch.Instance
                        ;

                    AnySegmentMatches[item] = Result;

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

                var Points = GetPoints(FrontSegmentMatches, AnySegmentMatches);

                ret = new DefinitionMatch(Definition)
                {
                    Definition = Definition,
                    FrontSegmentMatches = FrontSegmentMatches.ToImmutableDictionary(),
                    AnySegmentMatches = AnySegmentMatches.ToImmutableDictionary(),

                    Percentage = Percentage,
                    Points = Points,
                };
            }

            return ret;
        }

        protected virtual long GetPoints(IDictionary<PrefixSegment, SegmentMatch> FrontSegmentMatches, IDictionary<StringSegment, SegmentMatch> AnySegmentMatches)
        {
            var ret = 0;
            var MatchSet = new[]
            {
                FrontSegmentMatches.Values,
                AnySegmentMatches.Values
            };

            var Matches = MatchSet.SelectMany(x => x);

            var Multiplier_StartOfFile = 1000;
            var Multiplier_BeginningOfFile = 5;
            var Multiplier_Anywhere = 1;


            foreach (var Match in Matches)
            {
                var PointsToAdd = 0;
                
                if(Match is NoSegmentMatch V1)
                {

                } else if (Match is PrefixSegmentMatch V2)
                {
                    var Multiplier = V2.Segment.Start == 0
                        ? Multiplier_StartOfFile
                        : Multiplier_BeginningOfFile
                        ;
                    
                    PointsToAdd = Multiplier * V2.Segment.Pattern.Length;

                } else if (Match is StringSegmentMatch V3)
                {
                    var Multiplier = Multiplier_Anywhere;

                    PointsToAdd = Multiplier * V3.Segment.Pattern.Length;
                }
                ret += PointsToAdd;
            }



            return ret;
        }
    }

}
