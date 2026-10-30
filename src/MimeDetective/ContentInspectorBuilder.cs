using MimeDetective.Engine;
using MimeDetective.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective;

/// <summary>
///     Use this class to create a new <see cref="IContentInspector" />.
/// </summary>
public class ContentInspectorBuilder {
    public WildCardRuleOptionsBuilder WildCardRuleOptions { get; set; } = new();


    /// <summary>
    ///     Options that control how deep the definitions are analyzed.
    /// </summary>
    public DefinitionMatchEvaluatorOptions MatchEvaluatorOptions { get; set; } = new();

    /// <summary>
    ///     The definitions that will be run against input data.
    /// </summary>
    public IList<Definition> Definitions { get; set; } = [];

    /// <summary>
    ///     Whether multiple definitions should be evaluated concurrently.  If you have thousands of definitions, set this to
    ///     true, otherwise, set this to false.
    /// </summary>
    public bool Parallel { get; set; } = true;

    public IContentInspector Build() {
        var options = MatchEvaluatorOptions;
        var defs = Definitions.ToImmutableArray();

        WildCardMatcherProvider WildCardRuleIndex = WildCardRuleOptions.OptimizeFor switch {
            WildCardRuleResourceOptimization.HighSpeed => new WildCardMatcherProviderHighSpeed(),
            WildCardRuleResourceOptimization.LowMemory => new WildCardMatcherProviderLowMemory(),
            _ => new WildCardMatcherProviderHighSpeed(),
            //_ => new WildCardMatcherProviderLowMemory()
        };

        var ret = new ContentInspectorImpl(defs, options, WildCardRuleIndex, Parallel);

        return ret;
    }
}
