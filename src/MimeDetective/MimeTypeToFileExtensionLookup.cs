using MimeDetective.Engine;
using MimeDetective.Storage;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective;

public class MimeTypeToFileExtensionLookup {
    public ImmutableDictionary<string, ImmutableArray<FileExtensionMatch>> Values { get; }


    internal MimeTypeToFileExtensionLookup(ImmutableArray<Definition> definitions) {
        Values = (
            from x1 in definitions
            let mimetype = x1.File.MimeType?.ToLowerInvariant()
            where !string.IsNullOrWhiteSpace(mimetype)
            group x1 by mimetype
            into G1
            let FileExtensions = (
                from x2 in G1
                from y2 in x2.File.Extensions
                let extension = y2.ToLowerInvariant()
                where !string.IsNullOrWhiteSpace(extension)
                group x2 by extension
                into G2
                let Matches = (
                    from y in G2
                    select new DefinitionMatch {
                        Definition = y,
                        Type = DefinitionMatchType.Unknown
                    }).ToImmutableArray()
                let v2 = new FileExtensionMatch {
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

    public string? TryGetValue(string mimeType) {
        var ret = default(string?);

        if (TryGetValues(mimeType).FirstOrDefault() is { } v1) {
            ret = v1.Extension;
        }


        return ret;
    }

    public ImmutableArray<FileExtensionMatch> TryGetValues(string mimeType) {
        var ret = ImmutableArray<FileExtensionMatch>.Empty;

        if (Values.TryGetValue(mimeType, out var tret)) {
            ret = tret;
        }

        return ret;
    }
}
