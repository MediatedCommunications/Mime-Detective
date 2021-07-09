using FileDetection.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDetection.Engine
{
    public static class StringSegmentExtrator
    {

        private static ImmutableArray<bool> ValidBytes { get; }
        static StringSegmentExtrator()
        {
            var Valid = ""
                + "abcdefghijklmnopqrstuvwxyz"
                + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
                + " -+_.$(){}~*%\0"
                ;

            var ValidBytesArray = new bool[byte.MaxValue + 1];
            foreach (var item in Valid)
            {
                ValidBytesArray[(byte)item] = true;
            }

            ValidBytes = ValidBytesArray.ToImmutableArray();

        }

        public static ImmutableArray<StringSegment> ExtractStrings(ImmutableArray<byte> Content) {
            return Strings1(Content);
        }

        private static ImmutableArray<StringSegment> Strings1(ImmutableArray<byte> Content)
        {
            var tret = new Stack<List<byte>>();

            tret.Push(new());

            var delta = (byte) ('a' - 'A');

            for (var i = 0; i < Content.Length; i++)
            {
                var V1 = Content[i];
                
                if (ValidBytes[V1])
                {
                    if (V1 >= (byte)'a' && V1 <= (byte)'z')
                    {
                        V1 -= delta;
                    }

                    tret.Peek().Add(V1);
                } else if(tret.Peek().Count > 0)
                {
                    tret.Push(new());
                }
            }

            var ret = (
                from x in tret
                where x.Count >= 3
                select new StringSegment()
                {
                    Pattern = x.ToImmutableArray()
                }
                ).ToImmutableArray();

            return ret;
            
        }

    }
}
