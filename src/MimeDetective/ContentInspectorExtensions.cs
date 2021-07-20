using MimeDetective.Engine;
using MimeDetective.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;

namespace MimeDetective {
    public static class ContentInspectorExtensions
    {

        public static ImmutableArray<DefinitionMatch> Inspect(this ContentInspector This, IEnumerable<byte> Content) {
            return This.Inspect(Content.ToImmutableArray());
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
