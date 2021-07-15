using FileDetection.Engine;
using FileDetection.Storage;
using System.Collections.Generic;
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
        public IList<Definition> Definitions { get; set; } = new List<Definition>();

        /// <summary>
        /// Whether multiple definitions should be evaluated concurrently.  If you have thousands of definitions, set this to true, otherwise, set this to false.
        /// </summary>
        public bool Parallel { get; set; } = true;

        public IContentDetectionEngine Build() {

            var Options = MatchEvaluatorOptions;
            var Defs = Definitions.ToImmutableArray();

            var ret = new ContentDetectionEngine(Defs, Options, Parallel);

            return ret;
        }

    }
}
