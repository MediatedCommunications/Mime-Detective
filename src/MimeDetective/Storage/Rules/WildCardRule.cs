using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective.Storage;

/// <summary>
///     Represents a <see cref="Rule" /> that can occur anywhere in the content.
/// </summary>
public class WildCardRule : ValueRule {

    public static WildCardRule Create(string text, bool apostropheIsNull = true) {
        var ret = new WildCardRule { Bytes = BytesFromText(text, apostropheIsNull), };
        return ret;
    }

    public static WildCardRule Create(IEnumerable<byte> bytes) {
        return new() { Bytes = bytes.ToImmutableArray(), };
    }
}
