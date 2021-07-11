using FileDetection.Storage;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FileDetection.Engine {
    public static class StringSegmentExtrator {

        private static ImmutableArray<bool> ValidBytes { get; }
        private static readonly byte Separator = (byte)'|';
        private static readonly int MinSegmentLength = 3;


        static StringSegmentExtrator() {
            var Valid = ""
                + "abcdefghijklmnopqrstuvwxyz"
                + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
                + "0123456789"
                + " -+_.$(){}~*%\0"
                ;

            var ValidBytesArray = new bool[byte.MaxValue + 1];
            foreach (var item in Valid) {
                ValidBytesArray[(byte)item] = true;
            }

            ValidBytes = ValidBytesArray.ToImmutableArray();

        }


        public static byte[] ExtractBytes(ImmutableArray<byte> Content) {
            var ret = Array.Empty<byte>();

            var delta = (byte)('a' - 'A');

            var SegmentLength = 0;
            var LastBreaker = 0;

            var tret = new List<byte>(Content.Length) {
                Separator
            };

            for (var i = 0; i < Content.Length; i++) {
                var V1 = Content[i];

                if (ValidBytes[V1]) {
                    if (V1 >= (byte)'a' && V1 <= (byte)'z') {
                        V1 -= delta;
                    }

                    tret.Add(V1);

                    SegmentLength += 1;

                } else if (tret[^1] != Separator) {

                    if (SegmentLength >= MinSegmentLength) {
                        tret.Add(Separator);
                        LastBreaker = tret.Count;
                    } else {
                        tret.RemoveRange(LastBreaker, tret.Count - LastBreaker);
                    }

                    SegmentLength = 0;

                }
            }

            if(SegmentLength >= MinSegmentLength) {
                tret.Add(Separator);
            } else {
                tret.RemoveRange(LastBreaker, tret.Count - LastBreaker);
            }

            //Must be > because it contains a | at the start
            if(tret.Count > MinSegmentLength) {
                ret = tret.ToArray();
            }

            return ret;
        }

        public static ImmutableArray<StringSegment> ExtractBytesString(ImmutableArray<byte> Content) {
            var ret = ImmutableArray<StringSegment>.Empty;

            var tret = ExtractBytes(Content);

            if(tret.Length > 0) {
                ret = new[] {
                    new StringSegment() {
                        Pattern = tret.ToImmutableArray()
                    }
                }.ToImmutableArray();
            }

            return ret;
        }

        public static ImmutableArray<StringSegment> ExtractStrings(ImmutableArray<byte> Content) {
            return ExtractBytesString(Content);
        }

        public static ImmutableArray<StringSegment> ExtractAllStrings(ImmutableArray<byte> Content) {
            var tret = new List<ImmutableArray<byte>>();

            var Bytes = ExtractBytes(Content);

            var Start = 1;
            
            while(Start < Bytes.Length) {
                var NextEnd = Array.IndexOf(Bytes, Separator, Start);

                tret.Add(Bytes[Start..NextEnd].ToImmutableArray());

                Start = NextEnd + 1;
            }


            var ret = (
                from x in tret
                let v = new StringSegment() {
                    Pattern = x
                }
                orderby v.Pattern.Length descending
                select v
                ).ToImmutableArray();


            return ret;
        }

    }

}
