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
        private readonly Dictionary<byte, int> SkipTable;

        private int SkipTableValue(byte Index)
        {
            if (!SkipTable.TryGetValue(Index, out var ret))
            {
                ret = Needle.Length;
            }
            return ret;
        }

        private readonly ImmutableArray<byte> Needle;
        private readonly ImmutableArray<int> OffsetTable;

        public StringSegmentMatcherBoyerMooreProvider(IEnumerable<byte> needle) : this(needle.ToImmutableArray())
        {

        }

        public StringSegmentMatcherBoyerMooreProvider(ImmutableArray<byte> needle)
        {
            Needle = needle;
            SkipTable = MakeByteTable(Needle);
            OffsetTable = MakeOffsetTable(Needle).ToImmutableArray();
        }

        public IEnumerable<int> Search(IEnumerable<byte> haystack, bool MultipleResults = true)
        {
            return Search(haystack.ToImmutableArray(), MultipleResults);
        }

        public IEnumerable<int> Search(ImmutableArray<byte> haystack, bool MultipleResults = true)
        {
            if (Needle.Length == 0)
            {
                yield break;
            }

            var found = false;

            for (var i = Needle.Length - 1; i < haystack.Length;)
            {
                int j;

                for (j = Needle.Length - 1; Needle[j] == haystack[i]; --i, --j)
                {
                    if (j != 0)
                    {
                        continue;
                    }

                    yield return i;
                    found = true;
                    i += Needle.Length - 1;
                    break;
                }

                i += Math.Max(OffsetTable[Needle.Length - 1 - j], SkipTableValue(haystack[i]));
                if (!MultipleResults && found)
                {
                    break;
                }
            }
        }

        private static Dictionary<byte, int> MakeByteTable(ImmutableArray<byte> Needle)
        {
            var ret = new Dictionary<byte, int>();

            for (var i = 0; i < Needle.Length - 1; ++i)
            {
                ret[Needle[i]] = Needle.Length - 1 - i;
            }

            return ret;
        }

        private static int[] MakeOffsetTable(ImmutableArray<byte> needle)
        {
            var table = new int[needle.Length];
            var lastPrefixPosition = needle.Length;

            for (var i = needle.Length - 1; i >= 0; --i)
            {
                if (IsPrefix(needle, i + 1))
                {
                    lastPrefixPosition = i + 1;
                }

                table[needle.Length - 1 - i] = lastPrefixPosition - i + needle.Length - 1;
            }

            for (var i = 0; i < needle.Length - 1; ++i)
            {
                var slen = SuffixLength(needle, i);
                table[slen] = needle.Length - 1 - i + slen;
            }

            return table;
        }

        private static bool IsPrefix(ImmutableArray<byte> needle, int p)
        {
            for (int i = p, j = 0; i < needle.Length; ++i, ++j)
            {
                if (needle[i] != needle[j])
                {
                    return false;
                }
            }

            return true;
        }

        private static int SuffixLength(ImmutableArray<byte> needle, int p)
        {
            var len = 0;

            for (int i = p, j = needle.Length - 1; i >= 0 && needle[i] == needle[j]; --i, --j)
            {
                ++len;
            }

            return len;
        }
    }
}
