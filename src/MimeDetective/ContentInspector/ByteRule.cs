using MimeDetective.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;

namespace MimeDetective.Storage;

[DebuggerDisplay(DisplayBase.GetDebuggerDisplay)]
internal record struct ByteRule : IGetDebuggerDisplay {
    public int Start { get; init; }
    public byte Value { get; init; }

    public static ByteRule Create(int Start, byte Value) {
        return new ByteRule() { Start = Start, Value = Value, };
    }

    string? IGetDebuggerDisplay.GetDebuggerDisplay() {
        return GetDebuggerDisplay();
    }

    public readonly string? GetDebuggerDisplay() {
        return $@"[{Start}] = {Value}";
    }

}

internal static class OffsetRuleExtensions2 {

    public static ByteRule[] ToByteRules(this ImmutableArray<OffsetRule> This) {
        var ret = This.SelectMany(ToByteRules).ToArray();

        return ret;
    }

    public static ByteRule[] ToByteRules(this OffsetRule This) {
        var ret = new ByteRule[This.Bytes.Length];

        for(var i = 0; i < This.Bytes.Length; ++i) {
            ret[i] = new() {
                Start = This.Start + i,
                Value = This.Bytes[i],
            };
        }

        return ret;
    }
}