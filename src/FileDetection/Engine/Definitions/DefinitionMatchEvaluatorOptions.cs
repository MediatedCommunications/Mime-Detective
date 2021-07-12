using FileDetection.Diagnostics;

namespace FileDetection.Engine
{
    /// <summary>
    /// Options for a <see cref="DefinitionMatchEvaluator"/>
    /// </summary>
    public record DefinitionMatchEvaluatorOptions : DisplayRecord
    {
        /// <summary>
        /// Analyze <see cref="FileDetection.Storage.PrefixSegment"/>s
        /// </summary>
        public bool Include_Segments_Prefix { get; init; } = true;

        /// <summary>
        /// Analyze <see cref="FileDetection.Storage.StringSegment"/>s
        /// </summary>
        public bool Include_Segments_Strings { get; init; } = true;

        /// <summary>
        /// Should <see cref="DefinitionMatch"/>es with <see cref="DefinitionMatch.Percentage"/> = 1 be returned.
        /// </summary>
        public bool Include_Matches_Complete { get; init; } = true;

        /// <summary>
        /// Should <see cref="DefinitionMatch"/>es with <see cref="DefinitionMatch.Percentage"/> between 0..1 (exclusive) be returned.
        /// </summary>
        public bool Include_Matches_Partial { get; init; } = false;

        /// Should <see cref="DefinitionMatch"/>es with <see cref="DefinitionMatch.Percentage"/> = 0 be returned.
        public bool Include_Matches_Failed { get; init; } = false;
    }

}
