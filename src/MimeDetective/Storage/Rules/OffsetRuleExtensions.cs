using System.Collections.Generic;

namespace MimeDetective.Storage;

public static class OffsetRuleExtensions {
    /// <summary>
    ///     The exclusive upper bound of the <see cref="OffsetRule" />'s <see cref="Rule.Content" />
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static int ExclusiveEnd(this OffsetRule This) {
        return This.Start + This.Bytes.Length;
    }

    /// <summary>
    ///     Break a OffsetRules segment into individual 1-byte segments.
    /// </summary>
    /// <param name="this"></param>
    /// <returns></returns>
    public static IEnumerable<OffsetRule> Singularize(this OffsetRule This) {
        for (var i = 0; i < This.Bytes.Length; i++) {
            var ret = OffsetRule.Create(This.Start + i, This.Bytes[i]);

            yield return ret;
        }
    }
}
