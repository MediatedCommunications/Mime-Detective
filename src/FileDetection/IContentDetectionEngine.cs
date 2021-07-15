using FileDetection.Engine;
using System.Collections.Immutable;

namespace FileDetection {
    public interface IContentDetectionEngine {
        /// <summary>
        /// <see cref="Detect(ImmutableArray{byte})"/> the type of content.
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        ImmutableArray<DefinitionMatch> Detect(ImmutableArray<byte> Content);
    }
}
