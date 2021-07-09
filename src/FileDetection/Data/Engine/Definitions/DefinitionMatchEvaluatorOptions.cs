using FileDetection.Diagnostics;

namespace FileDetection.Engine
{
    /// <summary>
    /// Options for a <see cref="DefinitionMatchEvaluator"/>
    /// </summary>
    public record DefinitionMatchEvaluatorOptions : DisplayRecord
    {
        public bool Include_Segments_Prefix { get; init; } = true;
        public bool Include_Segments_Strings { get; init; } = true;

        public bool Include_Matches_Complete { get; init; } = true;
        public bool Include_Matches_Partial { get; init; } = false;
        public bool Include_Matches_Failed { get; init; } = false;
    }

}
