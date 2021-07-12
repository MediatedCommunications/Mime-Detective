using FileDetection.Engine;
using FileDetection.Storage;
using System.Collections.Immutable;

namespace FileDetection {
    public class FileDetectionEngineArgs {
        /// <summary>
        /// Options that control how deep the definitions are analyzed.
        /// </summary>
        public DefinitionMatchEvaluatorOptions MatchEvaluatorOptions { get; set; } = new();
        
        /// <summary>
        /// The definitions that will be run against input data.
        /// </summary>
        public ImmutableArray<Definition> Definitions { get; set; } = ImmutableArray<Definition>.Empty;

        public IFileDetectionEngine Create() {
            var ret = new FileDetectionEngine() {
                Definitions = Definitions,
                MatchEvaluatorOptions = MatchEvaluatorOptions,
            };

            return ret;
        }

    }
}
