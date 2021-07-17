using MimeDetective.Engine;
using MimeDetective.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective {

    public class ContentDetectionEngineBuilder {

        public PrefixSegmentOptionsBuilder PrefixSegmentOptions { get; set; } = new();
        public StringSegmentOptionsBuilder StringSegmentOptions { get; set; } = new();
        

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

            PrefixSegmentFilterProvider PrefixFilter = PrefixSegmentOptions.OptimizeFor switch {
                PrefixSegmentResourceOptimization.HighSpeed => new PrefixSegmentFilterProviderHighSpeed(),
                PrefixSegmentResourceOptimization.LowMemory => new PrefixSegmentFilterProviderLowMemory(),
                _ => new PrefixSegmentFilterProviderHighSpeed(),
            };

            StringSegmentMatcherProvider StringSegmentIndex = StringSegmentOptions.OptimizeFor switch {
                StringSegmentResourceOptimization.HighSpeed => new StringSegmentMatcherProviderHighSpeed(),
                StringSegmentResourceOptimization.LowMemory => new StringSegmentMatcherProviderLowMemory(),
                _ => new StringSegmentMatcherProviderHighSpeed()
            };

            var ret = new ContentDetectionEngine(Defs, Options, PrefixFilter, StringSegmentIndex, Parallel);

            return ret;
        }

    }
}
