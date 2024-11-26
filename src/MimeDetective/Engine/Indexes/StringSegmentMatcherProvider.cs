using MimeDetective.Storage;

namespace MimeDetective.Engine;

internal abstract class StringSegmentMatcherProvider {
    public abstract StringSegmentMatcher CreateMatcher(StringSegment segment);
}