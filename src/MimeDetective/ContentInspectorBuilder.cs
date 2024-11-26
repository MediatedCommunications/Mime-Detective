using MimeDetective.Engine;
using MimeDetective.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective;

/// <summary>
/// Use this class to create a new <see cref="IContentInspector"/>.
/// </summary>
public class ContentInspectorBuilder {

    public StringSegmentOptionsBuilder StringSegmentOptions { get; set; } = new();


    /// <summary>
    /// Options that control how deep the definitions are analyzed.
    /// </summary>
    public DefinitionMatchEvaluatorOptions MatchEvaluatorOptions { get; set; } = new();

    /// <summary>
    /// The definitions that will be run against input data.
    /// </summary>
    public IList<Definition> Definitions { get; set; } = [];

    /// <summary>
    /// Whether multiple definitions should be evaluated concurrently.  If you have thousands of definitions, set this to true, otherwise, set this to false.
    /// </summary>
    public bool Parallel { get; set; } = true;

    public IContentInspector Build() {

        var Options = MatchEvaluatorOptions;
        var Defs = Definitions.ToImmutableArray();

        StringSegmentMatcherProvider StringSegmentIndex = StringSegmentOptions.OptimizeFor switch {
            StringSegmentResourceOptimization.HighSpeed => new StringSegmentMatcherProviderHighSpeed(),
            StringSegmentResourceOptimization.LowMemory => new StringSegmentMatcherProviderLowMemory(),
            _ => new StringSegmentMatcherProviderHighSpeed()
            //_ => new StringSegmentMatcherProviderLowMemory()
        };

        var ret = new ContentInspectorImpl(Defs, Options, StringSegmentIndex, Parallel);

        return ret;
    }

}