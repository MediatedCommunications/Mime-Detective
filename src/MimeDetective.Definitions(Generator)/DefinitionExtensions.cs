using MimeDetective.Engine;
using MimeDetective.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective.Definitions
{
    public static class DefinitionExtensions
    {




        public static IEnumerable<Definition> Minify(this IEnumerable<Definition> Values)
        {
            var Input = Values.ToList();
            var Any = true;

            while (Any)
            {
                Any = false;

                for (var i = Input.Count - 1; i >= 1; i--)
                {

                    for (var j = i - 1; j >= 0; j--)
                    {

                        var v = Intersection(Input[i], Input[j]);
                        if (v is { })
                        {
                            Input.RemoveAt(i);
                            Input.RemoveAt(j);
                            Input.Add(v);
                            Any = true;
                            break;
                        }

                    }

                }

            }

            return Input;

        }

        public static Definition? Intersection(Definition A, Definition B)
        {
            var ret = default(Definition?);

            var Extensions = A.File.Extensions
                .ToHashSet(StringComparer.InvariantCultureIgnoreCase);
            Extensions.IntersectWith(B.File.Extensions);


            var MimeType = StringComparer.InvariantCultureIgnoreCase.Compare(A.File.MimeType, B.File.MimeType) == 0
                ? A.File.MimeType
                : default
                ;

            var Signature = Intersection(A.Signature, B.Signature);

            if(Signature is { } && Extensions.Count > 0)
            {
                ret = new Definition()
                {
                    File = new()
                    {
                        Description = string.Join("/", Extensions),
                        Extensions = Extensions.ToImmutableArray(),
                        MimeType = MimeType,
                    },
                    Signature = Signature,
                };
            }

            return ret;
        }

        public static Signature? Intersection(Signature A, Signature B)
        {
            var ret = default(Signature);

            var Any = Intersection(A.Strings, B.Strings);
            var Front = Intersection(A.Prefix, B.Prefix);

            if(Any.Length > 0 || Front.Length > 0)
            {
                ret = new Signature()
                {
                    Strings = Any,
                    Prefix = Front,
                };
            }

            return ret;
        }

        public static ImmutableArray<PrefixSegment> Intersection(ImmutableArray<PrefixSegment> A, ImmutableArray<PrefixSegment> B)
        {
            var Values = (
                from x in A
                from y in B
                from z in Intersection(x, y)
                select z
                ).ToImmutableArray();

            return Values;
        }

        public static ImmutableArray<PrefixSegment> Intersection(PrefixSegment A, PrefixSegment B)
        {
            var ret = new List<PrefixSegment>();

            var Start1 = A.Start;
            var End1 = A.ExclusiveEnd();

            var Start2 = B.Start;
            var End2 = B.ExclusiveEnd();

            var RangeStart = Math.Max(Start1, Start2);
            var RangeEnd = Math.Min(End1, End2);


            {
                var Found = new List<byte>();
                for (var Start = RangeStart; Start <= RangeEnd; Start++)
                {
                    var Terminal = Start == RangeEnd;
                    var Yield = Terminal;

                    if (!Terminal)
                    {
                        var AS = A.Pattern[Start - A.Start];
                        var BS = B.Pattern[Start - B.Start];

                        if (AS == BS)
                        {
                            Found.Add(AS);
                        } else
                        {
                            Yield = true;
                        }
                    }


                    if(Yield && Found.Count > 0) {
                        ret.Add(new PrefixSegment()
                        {
                            Pattern = Found.ToImmutableArray(),
                            Start = Start - Found.Count,
                        });

                        Found = new();
                    }

                }




            }

            return ret.ToImmutableArray();
        }


        public static ImmutableArray<StringSegment> Intersection(ImmutableArray<StringSegment> A, ImmutableArray<StringSegment> B)
        {
            var Values = (
                from x in A
                from y in B
                let v = Intersection(x, y)
                where v is { }
                select v
                ).ToImmutableArray();

            return Values;
        }

        private static StringSegment? Intersection(StringSegment A, StringSegment B)
        {
            var ret = default(StringSegment?);

            if (A.Pattern.SequenceEqual(B.Pattern))
            {
                ret = A;
            }

            return ret;
        }

    }

}
