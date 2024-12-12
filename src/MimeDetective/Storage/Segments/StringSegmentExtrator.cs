using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective.Storage;

public static class StringSegmentExtrator {
    private static readonly byte Separator = (byte)'|';
    private static readonly int MinSegmentLength = 3;

    private static ImmutableArray<bool> ValidBytes { get; }


    static StringSegmentExtrator() {
        const string valid = ""
                + "abcdefghijklmnopqrstuvwxyz"
                + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
                + "0123456789"
                + " -+_.$(){}~*%\0"
            ;

        var validBytesArray = new bool[byte.MaxValue + 1];
        foreach (var item in valid) {
            validBytesArray[(byte)item] = true;
        }

        ValidBytes = [.. validBytesArray];
    }


    public static byte[] ExtractBytes(ReadOnlySpan<byte> content) {
        var ret = Array.Empty<byte>();

        var delta = (byte)('a' - 'A');

        var segmentLength = 0;

        var tret = new List<byte>(content.Length) { Separator };

        var segmentStart = tret.Count;


        for (var i = 0; i < content.Length; i++) {
            var v1 = content[i];

            if (ValidBytes[v1]) {
                if (v1 is >= (byte)'a' and <= (byte)'z') {
                    v1 -= delta;
                }

                tret.Add(v1);

                segmentLength += 1;
            } else if (tret[^1] != Separator) {
                if (segmentLength >= MinSegmentLength) {
                    tret.Add(Separator);
                    segmentStart = tret.Count;
                } else {
                    var amountToRemove = tret.Count - segmentStart;
                    tret.RemoveRange(segmentStart, amountToRemove);
                }

                segmentLength = 0;
            }
        }

        if (segmentLength >= MinSegmentLength) {
            tret.Add(Separator);
        } else {
            var amountToRemove = tret.Count - segmentStart;
            tret.RemoveRange(segmentStart, amountToRemove);
        }

        //Must be > because it contains a | at the start
        if (tret.Count > MinSegmentLength) {
            ret = [.. tret];
        }

        return ret;
    }

    public static ImmutableArray<StringSegment> ExtractBytesString(ReadOnlySpan<byte> content) {
        var ret = ImmutableArray<StringSegment>.Empty;

        var tret = ExtractBytes(content);

        if (tret.Length > 0) {
            ret = [
                ..new[] { new StringSegment { Pattern = [..tret] } }
            ];
        }

        return ret;
    }

    public static ImmutableArray<StringSegment> ExtractStrings(ReadOnlySpan<byte> content) {
        return ExtractBytesString(content);
    }

    public static ImmutableArray<StringSegment> ExtractAllStrings(ReadOnlySpan<byte> content) {
        var tret = new List<ImmutableArray<byte>>();

        var bytes = ExtractBytes(content);

        var start = 1;

        while (start < bytes.Length) {
            var nextEnd = Array.IndexOf(bytes, Separator, start);

            tret.Add([.. bytes.AsSpan(start, nextEnd - start)]);

            start = nextEnd + 1;
        }


        var ret = (
            from x in tret
            let v = new StringSegment { Pattern = x }
            orderby v.Pattern.Length descending
            select v
        ).ToImmutableArray();


        return ret;
    }
}
