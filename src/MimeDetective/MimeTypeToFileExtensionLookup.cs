using MimeDetective.Engine;
using MimeDetective.Storage;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective {
    public class MimeTypeToFileExtensionLookup {

        public ImmutableDictionary<string, ImmutableArray<FileExtensionMatch>> Values { get; }

        public string? TryGetValue(string MimeType) {
            var ret = default(string?);

            if (TryGetValues(MimeType).FirstOrDefault() is { } V1) {
                ret = V1.Extension;
            }


            return ret;
        }

        public ImmutableArray<FileExtensionMatch> TryGetValues(string MimeType) {
            var ret = ImmutableArray<FileExtensionMatch>.Empty;

            if (Values.TryGetValue(MimeType, out var tret)) {
                ret = tret;
            }

            return ret;
        }


        internal MimeTypeToFileExtensionLookup(ImmutableArray<Definition> Definitions) {
            this.Values = (
                from x1 in Definitions
                let mimetype = x1.File.MimeType?.ToLower()
                where !string.IsNullOrWhiteSpace(mimetype)

                group x1 by mimetype into G1

                let FileExtensions = (
                    from x2 in G1
                    from y2 in x2.File.Extensions
                    let extension = y2.ToLower()
                    where !string.IsNullOrWhiteSpace(extension)
                    group x2 by extension into G2
                    let Matches = (
                        from y in G2
                        select new DefinitionMatch() {
                            Definition = y,
                            Type = DefinitionMatchType.Unknown,
                        }).ToImmutableArray()

                    let v2 = new FileExtensionMatch() {
                        Extension = G2.Key,
                        Matches = Matches,
                        Points = Matches.Length
                    }
                    orderby v2.Points descending
                    select v2
                ).ToImmutableArray()
                select new {
                    G1.Key,
                    FileExtensions
                }
                ).ToImmutableDictionary(x => x.Key, x => x.FileExtensions, StringComparer.InvariantCultureIgnoreCase);




        }


    }
}
