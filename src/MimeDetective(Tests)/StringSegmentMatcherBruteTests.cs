using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeDetective.Engine;
using MimeDetective.Storage;
using System;

namespace MimeDetective.Tests;

[TestClass]
public class StringSegmentMatcherBruteTests {
    [TestMethod]
    public void Match_FindsFirstOccurrence() {
        var result = Match([0x01, 0x02, 0x03, 0x02, 0x03,], [0x02, 0x03,]);

        var match = result as StringSegmentMatch;
        Assert.IsNotNull(match);
        Assert.AreEqual(1, match.Index);
        Assert.AreSame(NoSegmentMatch.Instance, Match([0x01, 0x02, 0x02,], [0x02, 0x03,]));
    }

    [TestMethod]
    public void Match_HandlesEdges() {
        Assert.AreEqual(0, IndexOf([], []));
        Assert.AreEqual(0, IndexOf([0x01, 0x02,], []));
        Assert.AreEqual(-1, IndexOf([], [0x01,]));
        Assert.AreEqual(-1, IndexOf([0x01, 0x02,], [0x01, 0x02, 0x03,]));
        Assert.AreEqual(0, IndexOf([0x00,], [0x00,]));
        Assert.AreEqual(2, IndexOf([0x01, 0x02, 0x00, 0x03,], [0x00, 0x03,]));
        Assert.AreEqual(2, IndexOf([0x61, 0x61, 0x61, 0x62,], [0x61, 0x62,]));
        Assert.AreEqual(4, IndexOf([0x01, 0x02, 0x03, 0x04, 0x05,], [0x05,]));
    }

    [TestMethod]
    public void Match_AgreesWithNaiveScan() {
        var random = new Random(261115);

        for (var needleLength = 0; needleLength <= 24; needleLength++) {
            for (var haystackLength = 0; haystackLength <= 96; haystackLength++) {
                var needle = new byte[needleLength];
                var haystack = new byte[haystackLength];
                random.NextBytes(needle);
                random.NextBytes(haystack);

                Assert.AreEqual(Naive(haystack, needle), IndexOf(haystack, needle),
                    $"needle {needleLength}, haystack {haystackLength}");
            }
        }

        var textNeedle = "CONTENT_TYPES"u8.ToArray();
        var textHaystack = new byte[50_000];
        for (var i = 0; i < textHaystack.Length; i++) {
            textHaystack[i] = (byte)('A' + (i % 26));
        }

        Assert.AreEqual(-1, IndexOf(textHaystack, textNeedle));

        textNeedle.CopyTo(textHaystack.AsSpan(40_000));
        Assert.AreEqual(40_000, IndexOf(textHaystack, textNeedle));
        Assert.AreEqual(Naive(textHaystack, textNeedle), IndexOf(textHaystack, textNeedle));
    }

    private static int IndexOf(byte[] haystack, byte[] needle) {
        var result = Match(haystack, needle);
        return result is StringSegmentMatch match ? match.Index : -1;
    }

    private static SegmentMatch Match(byte[] haystack, byte[] needle) {
        return new StringSegmentMatcherBrute(StringSegment.Create(needle)).Match(haystack);
    }

    private static int Naive(ReadOnlySpan<byte> haystack, ReadOnlySpan<byte> needle) {
        for (var i = 0; i <= haystack.Length - needle.Length; i++) {
            var found = true;
            for (var j = 0; j < needle.Length; j++) {
                if (needle[j] != haystack[i + j]) {
                    found = false;
                    break;
                }
            }

            if (found) {
                return i;
            }
        }

        return -1;
    }
}
