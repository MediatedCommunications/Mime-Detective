using System.Collections.Immutable;

namespace MimeDetective.Engine {

    /// <summary>
    /// The base interface for detecting if content matches a segment.
    /// </summary>
    internal interface ISegmentMatcher
    {
        SegmentMatch Match(ImmutableArray<byte> Content);
    }






}
