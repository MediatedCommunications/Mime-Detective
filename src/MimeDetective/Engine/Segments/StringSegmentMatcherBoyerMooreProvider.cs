using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective.Engine;

internal sealed class StringSegmentMatcherBoyerMooreProvider {
    private readonly int[] _skipTable;
    private readonly IReadOnlyList<byte> _needle;
    private readonly int[] _offsetTable;

    public StringSegmentMatcherBoyerMooreProvider(IEnumerable<byte> needle) : this(needle.ToImmutableArray()) {

    }

    public StringSegmentMatcherBoyerMooreProvider(IReadOnlyList<byte> needle) {
        this._needle = needle;
        this._skipTable = MakeSkipTable(this._needle);
        this._offsetTable = MakeOffsetTable(this._needle);
    }

    public IEnumerable<int> Search(IEnumerable<byte> haystack, bool multipleResults = true) {
        return Search(haystack.ToArray().AsMemory(), multipleResults);
    }

    public IEnumerable<int> Search(ReadOnlyMemory<byte> haystack, bool multipleResults = true) {
        if (this._needle.Count == 0) {
            yield break;
        }

        var end = this._needle.Count - 1;

        var found = false;

        for (var i = end; i < haystack.Length;) {
            int j;

            for (j = end; this._needle[j] == haystack.Span[i]; --i, --j) {
                if (j != 0) {
                    continue;
                }

                yield return i;
                found = true;
                i += end;
                break;
            }

            i += Math.Max(this._offsetTable[end - j], this._skipTable[haystack.Span[i]]);
            if (!multipleResults && found) {
                break;
            }
        }
    }
    public int? SearchFirst(ReadOnlySpan<byte> haystack) {
        if (this._needle.Count == 0) {
            return null;
        }

        var end = this._needle.Count - 1;

        for (var i = end; i < haystack.Length;) {
            int j;

            for (j = end; this._needle[j] == haystack[i]; --i, --j) {
                if (j != 0) {
                    continue;
                }

                return i;
            }

            i += Math.Max(this._offsetTable[end - j], this._skipTable[haystack[i]]);
        }

        return null;
    }

    private static int[] MakeSkipTable(IReadOnlyList<byte> needle) {
        var ret = new int[byte.MaxValue + 1];
        Array.Fill(ret, needle.Count);
        var end = needle.Count - 1;

        for (var i = 0; i < needle.Count; ++i) {
            ret[needle[i]] = end - i;
        }

        return ret;
    }

    private static int[] MakeOffsetTable(IReadOnlyList<byte> needle) {
        var table = new int[needle.Count];
        var lastPrefixPosition = needle.Count;
        var end = needle.Count - 1;
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

    private static bool IsPrefix(IReadOnlyList<byte> needle, int p) {
        for (int i = p, j = 0; i < needle.Count; ++i, ++j) {
            if (needle[i] != needle[j]) {
                return false;
            }
        }

        return true;
    }

    private static int SuffixLength(IReadOnlyList<byte> needle, int p) {
        var len = 0;

        for (int i = p, j = needle.Count - 1; i >= 0 && needle[i] == needle[j]; --i, --j) {
            ++len;
        }

        return len;
    }
}