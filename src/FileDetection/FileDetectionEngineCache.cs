using FileDetection.Engine;
using FileDetection.Storage;
using System.Collections.Immutable;

namespace FileDetection {
    internal class FileDetectionEngineCache {

        public FileDetectionEngineCache(Definition Definition) {
            this.Definition = Definition;
        }

        public Definition Definition { get; }
        public ImmutableArray<PrefixSegmentMatcher> Prefixes { get; init; } = ImmutableArray<PrefixSegmentMatcher>.Empty;
        public ImmutableArray<StringSegmentMatcher> Strings { get; init; } = ImmutableArray<StringSegmentMatcher>.Empty;

    }
}
