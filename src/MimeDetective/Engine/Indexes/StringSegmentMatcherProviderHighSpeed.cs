using MimeDetective.Storage;

namespace MimeDetective.Engine;

internal class StringSegmentMatcherProviderHighSpeed
    : StringSegmentMatcherProvider {
    public override StringSegmentMatcher CreateMatcher(StringSegment segment) {
        return StringSegmentMatcher.CreateHighSpeed(segment);
    }
}
