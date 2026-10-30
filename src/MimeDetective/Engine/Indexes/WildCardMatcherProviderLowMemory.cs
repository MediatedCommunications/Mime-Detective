using MimeDetective.Storage;

namespace MimeDetective.Engine;

internal class WildCardMatcherProviderLowMemory
    : WildCardMatcherProvider {
    public override WildCardMatcher CreateMatcher(WildCardRule segment) {
        return WildCardMatcher.CreateLowMemory(segment);
    }
}
