using MimeDetective.Diagnostics;
using MimeDetective.Storage;
using System;

namespace MimeDetective.Engine;

public abstract class WildCardMatcher : DisplayClass, IMatcher {
    public WildCardRule Segment { get; }

    public WildCardMatcher(WildCardRule segment) {
        Segment = segment;
    }

    public abstract Match Match(ReadOnlySpan<byte> content);

    public override string? GetDebuggerDisplay() {
        return Segment.GetDebuggerDisplay();
    }


    public static WildCardMatcher Create(WildCardRule segment) {
        return CreateHighSpeed(segment);
    }

    public static WildCardMatcher CreateLowMemory(WildCardRule segment) {
        return new WildCardMatcherBrute(segment);
    }

    public static WildCardMatcher CreateHighSpeed(WildCardRule segment) {

        return new WildCardMatcherBoyerMoore(segment);

        if (segment.Value.Length <= 32) {
            return new WildCardMatcherBrute(segment);
        } else {
            return new WildCardMatcherBoyerMoore(segment);
        }

    }
}
