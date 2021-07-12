using FileDetection.Engine;
using System.Collections.Immutable;

namespace FileDetection {
    public interface IFileDetectionEngine {
        /// <summary>
        /// Perform initial caching so that the initial <see cref="Detect(ImmutableArray{byte})"/> will run at maximum performance.
        /// </summary>
        void WarmUp();

        /// <summary>
        /// <see cref="Detect(ImmutableArray{byte})"/> the type of content.
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        ImmutableArray<DefinitionMatch> Detect(ImmutableArray<byte> Content);
    }
}
