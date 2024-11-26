using MimeDetective.Storage;

namespace MimeDetective.Engine;

internal class StringSegmentMatcherProviderLowMemory
    : StringSegmentMatcherProvider {
    public override StringSegmentMatcher CreateMatcher(StringSegment Segment) {
        return StringSegmentMatcher.CreateLowMemory(Segment);
    }
}