using MimeDetective.Engine;
using MimeDetective.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace MimeDetective
{
    public static class ContentInspectorExtensions
    {
        /// <summary>
        /// <see cref="ContentInspector.Inspect(ImmutableArray{byte})"/> the provided <paramref name="Content"/> and and determine matching definitions. 
        /// </summary>
        /// <param name="This">The <see cref="ContentInspector"/> to use.</param>
        /// <param name="Content">The binary content that should be inspected.</param>
        /// <returns></returns>
        public static ImmutableArray<DefinitionMatch> Inspect(this ContentInspector This, IEnumerable<byte> Content) {
            return This.Inspect(Content.ToImmutableArray());
        }

        /// <summary>
        /// <see cref="ContentInspector.Inspect(ImmutableArray{byte})"/> the provided <paramref name="Content"/> and and determine matching definitions. 
        /// </summary>
        /// <param name="This">The <see cref="ContentInspector"/> to use.</param>
        /// <param name="Content">The <see cref="Stream"/> that should be inspected.  Bytes will be read from this stream and the position will NOT be reset.</param>
        /// <param name="Reader">The <see cref="ContentReader"/> to use.  If not is specified, <see cref="ContentReader.Default"/> will be used.</param>
        /// <returns></returns>
        public static ImmutableArray<DefinitionMatch> Inspect(this ContentInspector This, Stream Content, ContentReader? Reader = default) {
            return Inspect(This, Content, false, Reader);
        }

        /// <summary>
        /// <see cref="ContentInspector.Inspect(ImmutableArray{byte})"/> the provided <paramref name="Content"/> and and determine matching definitions. 
        /// </summary>
        /// <param name="This">The <see cref="ContentInspector"/> to use.</param>
        /// <param name="Content">The <see cref="Stream"/> that should be inspected.  Bytes will be read from this stream and the position will OPTIONALLY be reset.</param>
        /// <param name="ResetPosition">Whether the position in the stream should be reset or not.</param>
        /// <param name="Reader">The <see cref="ContentReader"/> to use.  If not is specified, <see cref="ContentReader.Default"/> will be used.</param>
        /// <returns></returns>
        public static ImmutableArray<DefinitionMatch> Inspect(this ContentInspector This, Stream Content, bool ResetPosition, ContentReader? Reader = default) {
            var MyReader = Reader ?? ContentReader.Default;

            var MyContent = MyReader.ReadFromStream(Content, ResetPosition);

            return Inspect(This, MyContent);
        }

        /// <summary>
        /// <see cref="ContentInspector.Inspect(ImmutableArray{byte})"/> the provided <paramref name="Content"/> and and determine matching definitions. 
        /// </summary>
        /// <param name="This">The <see cref="ContentInspector"/> to use.</param>
        /// <param name="Content">The path to a file that should be inspected.</param>
        /// <param name="ResetPosition">Whether the position in the stream should be reset or not.</param>
        /// <param name="Reader">The <see cref="ContentReader"/> to use.  If not is specified, <see cref="ContentReader.Default"/> will be used.</param>
        /// <returns></returns>
        public static ImmutableArray<DefinitionMatch> Inspect(this ContentInspector This, string FileName, ContentReader? Reader = default) {
            var MyReader = Reader ?? ContentReader.Default;

            var MyContent = MyReader.ReadFromFile(FileName);

            return Inspect(This, MyContent);
        }

        /// <summary>
        /// Group <see cref="DefinitionMatch"/>s by <see cref="FileType.Extensions"/>.
        /// </summary>
        /// <param name="This"></param>
        /// <returns></returns>
        public static ImmutableArray<FileExtensionMatch> ByFileExtension(this ImmutableArray<DefinitionMatch> This)
        {
            var ret = (
                from x in This
                from y in x.Definition.File.Extensions
                group x by y into G
                let Extension = G.Key
                let Matches = G.ToImmutableArray()
                let Points = Matches.Sum(x => x.Points)
                let v = new FileExtensionMatch()
                {
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
        /// Group <see cref="DefinitionMatch"/> by <see cref="FileType.MimeType"/>
        /// </summary>
        /// <param name="This"></param>
        /// <returns></returns>
        public static ImmutableArray<MimeTypeMatch> ByMimeType(this ImmutableArray<DefinitionMatch> This)
        {
            var ret = (
                from x in This
                group x by x.Definition.File.MimeType into G
                let MimeType = G.Key
                let Matches = G.ToImmutableArray()
                let Points = Matches.Sum(x => x.Points)
                let v = new MimeTypeMatch()
                {
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
}
