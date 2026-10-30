using MimeDetective.Storage;

namespace MimeDetective.Engine;

internal class WildCardMatcherProviderHighSpeed
    : WildCardMatcherProvider {
    public override WildCardMatcher CreateMatcher(WildCardRule segment) {
        return WildCardMatcher.CreateHighSpeed(segment);
    }
}
