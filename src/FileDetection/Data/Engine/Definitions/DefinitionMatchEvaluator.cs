using FileDetection.Data.Engine;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FileDetection.Data.Engine
{

    /// <summary>
    /// Handles evaluating a <see cref="Definition"/> against content and scoring it.
    /// </summary>
    public class DefinitionMatchEvaluator
    {
        public DefinitionMatchEvaluatorOptions Options { get; init; } = new();


        public virtual DefinitionMatch? Match(Definition Definition, ImmutableArray<byte> Content, ImmutableArray<byte> UpperContent)
        {
            var ret = default(DefinitionMatch?);

            var Valid = true;
            var AllMatches = 0m;
            var GoodMatches = 0m;

            var FrontSegmentMatches = new Dictionary<BeginningSegment, SegmentMatch>();
            var AnySegmentMatches = new Dictionary<MiddleSegment, SegmentMatch>();

            if (Valid && Options.Include_Segments_Beginning)
            {

                foreach (var item in Definition.Signature.Beginning)
                {
                    AllMatches += 1;

                    var Result = item.GetMatch(Content);
                    FrontSegmentMatches[item] = Result;

                    if (Result == NoSegmentMatch.Instance)
                    {
                        if(Options.Include_Matches_Partial == false && Options.Include_Matches_Failed == false)
                        {
                            Valid = false;
                            break;
                        }
                    } else {
                        GoodMatches += 1;
                    }

                }
            }

            if (Valid && Options.Include_Segments_Middle)
            {
                foreach (var item in Definition.Signature.Middle)
                {
                    AllMatches += 1;

                    var Result = item.GetMatch(UpperContent);
                    AnySegmentMatches[item] = Result;

                    if (Result == NoSegmentMatch.Instance)
                    {
                        if (Options.Include_Matches_Partial == false && Options.Include_Matches_Failed == false)
                        {
                            Valid = false;
                            break;
                        }
                    }
                    else
                    {
                        GoodMatches += 1;
                    }

                }
            }

            if(GoodMatches == 0 && !Options.Include_Matches_Failed)
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

        protected virtual long GetPoints(IDictionary<BeginningSegment, SegmentMatch> FrontSegmentMatches, IDictionary<MiddleSegment, SegmentMatch> AnySegmentMatches)
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

                } else if (Match is BeginningSegmentMatch V2)
                {
                    var Multiplier = V2.Segment.Start == 0
                        ? Multiplier_StartOfFile
                        : Multiplier_BeginningOfFile
                        ;
                    
                    PointsToAdd = Multiplier * V2.Segment.Pattern.Length;

                } else if (Match is MiddleSegmentMatch V3)
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
