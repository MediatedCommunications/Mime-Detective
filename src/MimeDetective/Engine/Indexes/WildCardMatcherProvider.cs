using MimeDetective.Storage;

namespace MimeDetective.Engine;

internal abstract class WildCardMatcherProvider {
    public abstract WildCardMatcher CreateMatcher(WildCardRule segment);
}
