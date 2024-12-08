using MimeDetective.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective.Definitions;

public static class DefinitionExtensions {
    public static IEnumerable<Definition> Minify(this IEnumerable<Definition> values) {
        var input = values.ToList();
        var any = true;

        while (any) {
            any = false;

            for (var i = input.Count - 1; i >= 1; i--) {
                for (var j = i - 1; j >= 0; j--) {
                    var v = Intersection(input[i], input[j]);
                    if (v is not null) {
                        input.RemoveAt(i);
                        input.RemoveAt(j);
                        input.Add(v);
                        any = true;
                        break;
                    }
                }
            }
        }

        return input;
    }

    public static Definition? Intersection(Definition a, Definition b) {
        var ret = default(Definition?);

        var extensions = a.File.Extensions
            .ToHashSet(StringComparer.InvariantCultureIgnoreCase);
        extensions.IntersectWith(b.File.Extensions);


        var mimeType = StringComparer.InvariantCultureIgnoreCase.Compare(a.File.MimeType, b.File.MimeType) == 0
                ? a.File.MimeType
                : default
            ;

        var signature = Intersection(a.Signature, b.Signature);

        if (signature is not null && extensions.Count > 0) {
            ret = new() {
                File = new() {
                    Description = string.Join("/", extensions),
                    Extensions = extensions.ToImmutableArray(),
                    MimeType = mimeType
                },
                Signature = signature
            };
        }

        return ret;
    }

    public static Signature? Intersection(Signature a, Signature b) {
        var ret = default(Signature);

        var any = Intersection(a.Strings, b.Strings);
        var front = Intersection(a.Prefix, b.Prefix);

        if (any.Length > 0 || front.Length > 0) {
            ret = new() {
                Strings = any,
                Prefix = front
            };
        }

        return ret;
    }

    public static ImmutableArray<PrefixSegment> Intersection(ImmutableArray<PrefixSegment> a, ImmutableArray<PrefixSegment> b) {
        var values = (
            from x in a
            from y in b
            from z in Intersection(x, y)
            select z
        ).ToImmutableArray();

        return values;
    }

    public static ImmutableArray<PrefixSegment> Intersection(PrefixSegment a, PrefixSegment b) {
        var ret = new List<PrefixSegment>();

        var start1 = a.Start;
        var end1 = a.ExclusiveEnd();

        var start2 = b.Start;
        var end2 = b.ExclusiveEnd();

        var rangeStart = Math.Max(start1, start2);
        var rangeEnd = Math.Min(end1, end2);


        {
            var found = new List<byte>();
            for (var start = rangeStart; start <= rangeEnd; start++) {
                var terminal = start == rangeEnd;
                var yield = terminal;

                if (!terminal) {
                    var @as = a.Pattern[start - a.Start];
                    var bs = b.Pattern[start - b.Start];

                    if (@as == bs) {
                        found.Add(@as);
                    } else {
                        yield = true;
                    }
                }


                if (yield && found.Count > 0) {
                    ret.Add(new() {
                        Pattern = found.ToImmutableArray(),
                        Start = start - found.Count
                    });

                    found = [];
                }
            }
        }

        return ret.ToImmutableArray();
    }


    public static ImmutableArray<StringSegment> Intersection(ImmutableArray<StringSegment> a, ImmutableArray<StringSegment> b) {
        var values = (
            from x in a
            from y in b
            let v = Intersection(x, y)
            where v is not null
            select v
        ).ToImmutableArray();

        return values;
    }

    private static StringSegment? Intersection(StringSegment a, StringSegment b) {
        var ret = default(StringSegment?);

        if (a.Pattern.SequenceEqual(b.Pattern)) {
            ret = a;
        }

        return ret;
    }
}
