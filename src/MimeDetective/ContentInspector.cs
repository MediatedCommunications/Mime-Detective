using MimeDetective.Engine;
using System;
using System.Collections.Immutable;

namespace MimeDetective {
    /// <summary>
    /// Detect the type of file from a raw set of bytes.
    /// </summary>
    public interface IContentInspector {
        /// <summary>
        /// <see cref="Inspect(ReadOnlySpan{byte})"/> the type of content.
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        ImmutableArray<DefinitionMatch> Inspect(ReadOnlySpan<byte> Content);
    }
}
