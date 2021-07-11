using FileDetection.Engine;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace FileDetection.Storage
{
    /// <summary>
    /// The base class representing <see cref="Pattern"/> that exists in the target file.
    /// </summary>
    public abstract class PatternSegment : Segment
    {
        /// <summary>
        /// The <see cref="Pattern"/> that must exist in the target file.
        /// </summary>
        public ImmutableArray<byte> Pattern { get; init; } = ImmutableArray<byte>.Empty;

        public override string? GetDebuggerDisplay()
        {
            var Hex = System.Convert.ToHexString(Pattern.ToArray());
            var String = System.Text.Encoding.UTF8.GetString(Pattern.ToArray());
            String = String.Replace("\0", "'");

            return $@"{String} /// {Hex}";
        }

        private int? GetHashCode_Value;
        public sealed override int GetHashCode() {
            
            if (! (GetHashCode_Value is { } V1)) {
                V1 = GetHashCodeInternal();
                GetHashCode_Value = V1;
            }

            return V1;   
        }

        protected virtual int GetHashCodeInternal() {
            return ArrayComparer<byte>.Instance.GetHashCode(Pattern);
        }

    }
}
