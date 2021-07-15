using FileDetection.Engine;
using FileDetection.Storage;
using System.Collections.Immutable;

namespace FileDetection {
    public class ContentDetectionEngineBuilder {
        /// <summary>
        /// Options that control how deep the definitions are analyzed.
        /// </summary>
        public DefinitionMatchEvaluatorOptions MatchEvaluatorOptions { get; set; } = new();
        
        /// <summary>
        /// The definitions that will be run against input data.
        /// </summary>
        public ImmutableArray<Definition> Definitions { get; set; } = ImmutableArray<Definition>.Empty;

        /// <summary>
        /// Perform initial caching so that the initial <see cref="IFileDetectionEngine.Detect(ImmutableArray{byte})"/> will run at maximum performance.
        /// </summary>
        public bool WarmUp { get; set; } = true;

        public IContentDetectionEngine Build() {
            var ret = new ContentDetectionEngine() {
                Definitions = Definitions,
                MatchEvaluatorOptions = MatchEvaluatorOptions,
            };

            if (WarmUp) {
                ret.WarmUp();
            }

            return ret;
        }

    }
}
