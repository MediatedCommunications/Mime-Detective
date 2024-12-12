using MimeDetective.Engine;
using MimeDetective.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace MimeDetective;

public static class ContentInspectorExtensions {
    /// <summary>
    ///     <see cref="IContentInspector.Inspect(ReadOnlySpan{byte})" /> the provided <paramref name="content" /> and and
    ///     determine matching definitions.
    /// </summary>
    /// <param name="This">The <see cref="IContentInspector" /> to use.</param>
    /// <param name="content">The binary content that should be inspected.</param>
    /// <returns></returns>
    public static ImmutableArray<DefinitionMatch> Inspect(this IContentInspector This, IEnumerable<byte> content) {
        return This.Inspect(content.ToArray().AsSpan());
    }

    /// <summary>
    ///     <see cref="IContentInspector.Inspect(ReadOnlySpan{byte})" /> the provided <paramref name="content" /> and and
    ///     determine matching definitions.
    /// </summary>
    /// <param name="This">The <see cref="IContentInspector" /> to use.</param>
    /// <param name="content">
    ///     The <see cref="Stream" /> that should be inspected.  Bytes will be read from this stream and the
    ///     position will NOT be reset.
    /// </param>
    /// <param name="reader">
    ///     The <see cref="ContentReader" /> to use.  If not is specified,
    ///     <see cref="ContentReader.Default" /> will be used.
    /// </param>
    /// <returns></returns>
    public static ImmutableArray<DefinitionMatch> Inspect(this IContentInspector This, Stream content,
        ContentReader? reader = default) {
        return Inspect(This, content, false, reader);
    }

    /// <summary>
    ///     <see cref="IContentInspector.Inspect(ReadOnlySpan{byte})" /> the provided <paramref name="content" /> and and
    ///     determine matching definitions.
    /// </summary>
    /// <param name="This">The <see cref="IContentInspector" /> to use.</param>
    /// <param name="content">
    ///     The <see cref="Stream" /> that should be inspected.  Bytes will be read from this stream and the
    ///     position will OPTIONALLY be reset.
    /// </param>
    /// <param name="resetPosition">Whether the position in the stream should be reset or not.</param>
    /// <param name="reader">
    ///     The <see cref="ContentReader" /> to use.  If not is specified,
    ///     <see cref="ContentReader.Default" /> will be used.
    /// </param>
    /// <returns></returns>
    public static ImmutableArray<DefinitionMatch> Inspect(this IContentInspector This, Stream content, bool resetPosition,
        ContentReader? reader = default) {
        var myReader = reader ?? ContentReader.Default;

        var myContent = myReader.ReadFromStream(content, resetPosition);

        return Inspect(This, myContent);
    }

    /// <summary>
    ///     <see cref="IContentInspector.Inspect(ReadOnlySpan{byte})" /> the provided <paramref name="Content" /> and and
    ///     determine matching definitions.
    /// </summary>
    /// <param name="This">The <see cref="IContentInspector" /> to use.</param>
    /// <param name="Content">The path to a file that should be inspected.</param>
    /// <param name="ResetPosition">Whether the position in the stream should be reset or not.</param>
    /// <param name="reader">
    ///     The <see cref="ContentReader" /> to use.  If not is specified,
    ///     <see cref="ContentReader.Default" /> will be used.
    /// </param>
    /// <returns></returns>
    public static ImmutableArray<DefinitionMatch> Inspect(this IContentInspector This, string fileName,
        ContentReader? reader = default) {
        var myReader = reader ?? ContentReader.Default;

        var myContent = myReader.ReadFromFile(fileName);

        return Inspect(This, myContent);
    }

    /// <summary>
    ///     <see cref="Inspect(ReadOnlySpan{byte})" /> the type of content.
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static ImmutableArray<DefinitionMatch> Inspect(this IContentInspector This, byte[] content) {
        return This.Inspect(content);
    }

    /// <summary>
    ///     <see cref="Inspect(ReadOnlySpan{byte})" /> the type of content.
    /// </summary>
    /// <param name="content"></param>
    /// <param name="start"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static ImmutableArray<DefinitionMatch> Inspect(this IContentInspector This, byte[] content, int start, int length) {
        return This.Inspect(content.AsSpan(start, length));
    }

    /// <summary>
    ///     <see cref="Inspect(ImmutableArray{byte})" /> the type of content.
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static ImmutableArray<DefinitionMatch> Inspect(this IContentInspector This, ImmutableArray<byte> content) {
        return This.Inspect(content.AsSpan());
    }

    /// <summary>
    ///     Group <see cref="DefinitionMatch" />s by <see cref="FileType.Extensions" />.
    /// </summary>
    /// <param name="This"></param>
    /// <returns></returns>
    public static ImmutableArray<FileExtensionMatch> ByFileExtension(this ImmutableArray<DefinitionMatch> This) {
        var ret = (
            from x in This
            from y in x.Definition.File.Extensions
            group x by y
            into G
            let Extension = G.Key
            let Matches = G.ToImmutableArray()
            let Points = Matches.Sum(x => x.Points)
            let v = new FileExtensionMatch {
                Extension = Extension,
                Matches = Matches,
                Points = Points
            }
            orderby v.Points descending
            select v
        ).ToImmutableArray();

        return ret;
    }

    /// <summary>
    ///     Group <see cref="DefinitionMatch" /> by <see cref="FileType.MimeType" />
    /// </summary>
    /// <param name="This"></param>
    /// <returns></returns>
    public static ImmutableArray<MimeTypeMatch> ByMimeType(this ImmutableArray<DefinitionMatch> This) {
        var ret = (
            from x in This
            group x by x.Definition.File.MimeType
            into G
            let MimeType = G.Key
            let Matches = G.ToImmutableArray()
            let Points = Matches.Sum(x => x.Points)
            let v = new MimeTypeMatch {
                MimeType = MimeType,
                Matches = Matches,
                Points = Points
            }
            orderby v.Points descending
            select v
        ).ToImmutableArray();

        return ret;
    }
}
