using System;
using System.Collections.Immutable;

namespace MimeDetective.Storage {
    /// <summary>
    /// The base class representing <see cref="Pattern"/> that exists in the target file.
    /// </summary>
    public abstract class PatternSegment : Segment
    {
        /// <summary>
        /// The <see cref="Pattern"/> that must exist in the target file.
        /// </summary>
        public required ImmutableArray<byte> Pattern { get; init; } = [];

        protected static ImmutableArray<byte> BytesFromText(string Text, bool ApostropheIsNull = true) {
            if (ApostropheIsNull) {
                Text = Text.Replace("'", "\0");
            }
            var ret = System.Text.Encoding.UTF8
                .GetBytes(Text)
                .ToImmutableArray()
                ;

            return ret;
        }

        public override string? GetDebuggerDisplay()
        {
            var Hex = System.Convert.ToHexString([.. Pattern,]);
            var String = System.Text.Encoding.UTF8.GetString([.. Pattern,]);
            String = String.Replace("\0", "'");

            return $@"{String} /// {Hex}";
        }

        private int? GetHashCode_Value;
        public override sealed int GetHashCode() {
            
            if (! (GetHashCode_Value is { } V1)) {
                V1 = GetHashCodeInternal();
                GetHashCode_Value = V1;
            }

            return V1;   
        }

        protected virtual int GetHashCodeInternal() {
            return EnumerableComparer<byte>.Instance.GetHashCode(Pattern);
        }

    }
}
