using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective.Engine;

internal sealed class WildCardMatcherBoyerMooreProvider {
    private readonly ImmutableArray<byte> _needle;
    private readonly int[] _offsetTable;
    private readonly int[] _skipTable;

    public WildCardMatcherBoyerMooreProvider(ImmutableArray<byte> needle) {
        _needle = needle;
        _skipTable = MakeSkipTable(_needle);
        _offsetTable = MakeOffsetTable(_needle);
    }

    public IEnumerable<int> Search(IEnumerable<byte> haystack, bool multipleResults = true) {
        return Search(haystack.ToArray().AsMemory(), multipleResults);
    }

    public IEnumerable<int> Search(ReadOnlyMemory<byte> haystack, bool multipleResults = true) {
        if (_needle.Length == 0) {
            yield break;
        }

        var end = _needle.Length - 1;

        var found = false;

        for (var i = end; i < haystack.Length;) {
            int j;

            for (j = end; _needle[j] == haystack.Span[i]; --i, --j) {
                if (j != 0) {
                    continue;
                }

                yield return i;
                found = true;
                i += end;
                break;
            }

            i += Math.Max(_offsetTable[end - j], _skipTable[haystack.Span[i]]);
            if (!multipleResults && found) {
                break;
            }
        }
    }

    public int? SearchFirst(ReadOnlySpan<byte> haystack) {
        if (_needle.Length == 0) {
            return null;
        }

        var end = _needle.Length - 1;

        for (var i = end; i < haystack.Length;) {
            int j;

            for (j = end; _needle[j] == haystack[i]; --i, --j) {
                if (j != 0) {
                    continue;
                }

                return i;
            }

            i += Math.Max(_offsetTable[end - j], _skipTable[haystack[i]]);
        }

        return null;
    }

    private static int[] MakeSkipTable(ImmutableArray<byte> needle) {
        var ret = new int[byte.MaxValue + 1];
        Array.Fill(ret, needle.Length);
        var end = needle.Length - 1;

        for (var i = 0; i < needle.Length; ++i) {
            ret[needle[i]] = end - i;
        }

        return ret;
    }

    private static int[] MakeOffsetTable(ImmutableArray<byte> needle) {
        var table = new int[needle.Length];
        var lastPrefixPosition = needle.Length;
        var end = needle.Length - 1;
        for (var i = end; i >= 0; --i) {
            if (IsPrefix(needle, i + 1)) {
                lastPrefixPosition = i + 1;
            }

            table[end - i] = lastPrefixPosition - i + end;
        }

        for (var i = 0; i < end; ++i) {
            var slen = SuffixLength(needle, i);
            table[slen] = end - i + slen;
        }

        return table;
    }

    private static bool IsPrefix(ImmutableArray<byte> needle, int p) {
        var length = needle.Length - p;
        return needle.AsSpan(p, length).SequenceEqual(needle.AsSpan(0, length));
    }

    private static int SuffixLength(ImmutableArray<byte> needle, int p) {
        var len = 0;

        for (int i = p, j = needle.Length - 1; i >= 0 && needle[i] == needle[j]; --i, --j) {
            ++len;
        }

        return len;
    }
}
