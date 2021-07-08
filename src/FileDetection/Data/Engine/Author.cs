using FileDetection.Data.Diagnostics;

namespace FileDetection.Data.Engine
{
    /// <summary>
    /// Information regarding the Author for a <see cref="Definition"/>
    /// </summary>
    public record Author : DisplayRecord
    {
        /// <summary>
        /// The Author's <see cref="Name"/>
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// The Author's <see cref="Email"/> address
        /// </summary>
        public string? Email { get; init; }

        /// <summary>
        /// The Author's <see cref="Website"/>
        /// </summary>
        public string? Website { get; init; }

        public override string? GetDebuggerDisplay()
        {
            return $@"{Name} ({Email} /// {Website})";
        }
    }
}
