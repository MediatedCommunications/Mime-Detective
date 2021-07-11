using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDetection.Engine
{
    internal sealed class StringSegmentMatcherBoyerMooreProvider
    {
        private readonly int[] SkipTable;
        private readonly IReadOnlyList<byte> Needle;
        private readonly int[] OffsetTable;

        public StringSegmentMatcherBoyerMooreProvider(IEnumerable<byte> needle) : this(needle.ToImmutableArray())
        {

        }

        public StringSegmentMatcherBoyerMooreProvider(IReadOnlyList<byte> needle)
        {
            Needle = needle;
            SkipTable = MakeSkipTable(Needle);
            OffsetTable = MakeOffsetTable(Needle);
        }

        public IEnumerable<int> Search(IEnumerable<byte> haystack, bool MultipleResults = true)
        {
            return Search(haystack.ToImmutableArray(), MultipleResults);
        }

        public IEnumerable<int> Search(IReadOnlyList<byte> haystack, bool MultipleResults = true)
        {
            if (Needle.Count == 0)
            {
                yield break;
            }

            var end = Needle.Count - 1;

            var found = false;

            for (var i = end; i < haystack.Count;)
            {
                int j;

                for (j = end; Needle[j] == haystack[i]; --i, --j)
                {
                    if (j != 0)
                    {
                        continue;
                    }

                    yield return i;
                    found = true;
                    i += end;
                    break;
                }

                i += Math.Max(OffsetTable[end - j], SkipTable[haystack[i]]);
                if (!MultipleResults && found)
                {
                    break;
                }
            }
        }

        private static int[] MakeSkipTable(IReadOnlyList<byte> Needle)
        {
            var ret = new int[byte.MaxValue + 1];
            Array.Fill(ret, Needle.Count);
            var end = Needle.Count - 1;

            for (var i = 0; i < Needle.Count; ++i)
            {
                ret[Needle[i]] = end - i;
            }

            return ret;
        }

        private static int[] MakeOffsetTable(IReadOnlyList<byte> needle)
        {
            var table = new int[needle.Count];
            var lastPrefixPosition = needle.Count;
            var end = needle.Count - 1;
            for (var i = end; i >= 0; --i)
            {
                if (IsPrefix(needle, i + 1))
                {
                    lastPrefixPosition = i + 1;
                }

                table[end - i] = lastPrefixPosition - i + end;
            }

            for (var i = 0; i < end; ++i)
            {
                var slen = SuffixLength(needle, i);
                table[slen] = end - i + slen;
            }

            return table;
        }

        private static bool IsPrefix(IReadOnlyList<byte> needle, int p)
        {
            for (int i = p, j = 0; i < needle.Count; ++i, ++j)
            {
                if (needle[i] != needle[j])
                {
                    return false;
                }
            }

            return true;
        }

        private static int SuffixLength(IReadOnlyList<byte> needle, int p)
        {
            var len = 0;

            for (int i = p, j = needle.Count - 1; i >= 0 && needle[i] == needle[j]; --i, --j)
            {
                ++len;
            }

            return len;
        }
    }
}
