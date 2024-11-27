using MimeDetective.Storage;

namespace MimeDetective.Engine;

internal class StringSegmentMatcherProviderLowMemory
    : StringSegmentMatcherProvider {
    public override StringSegmentMatcher CreateMatcher(StringSegment segment) {
        return StringSegmentMatcher.CreateLowMemory(segment);
    }
}