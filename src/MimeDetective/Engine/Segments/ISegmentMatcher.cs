using System;

namespace MimeDetective.Engine;

/// <summary>
///     The base interface for detecting if content matches a segment.
/// </summary>
internal interface ISegmentMatcher {
    SegmentMatch Match(ReadOnlySpan<byte> content);
}
