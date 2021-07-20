using MimeDetective.Engine;
using System.Collections.Immutable;

namespace MimeDetective {
    /// <summary>
    /// Detect the type of file from a raw set of bytes.
    /// </summary>
    public interface ContentInspector {
        /// <summary>
        /// <see cref="Inspect(ImmutableArray{byte})"/> the type of content.
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        ImmutableArray<DefinitionMatch> Inspect(ImmutableArray<byte> Content);
    }
}
