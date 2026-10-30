using System;

namespace MimeDetective.Engine;

/// <summary>
///     The base interface for detecting if content matches a segment.
/// </summary>
internal interface IMatcher {
    Match Match(ReadOnlySpan<byte> content);
}
