using MimeDetective.Engine;
using MimeDetective.Storage;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective;

public class FileExtensionToMimeTypeLookup {
    public ImmutableDictionary<string, ImmutableArray<MimeTypeMatch>> Values { get; }


    internal FileExtensionToMimeTypeLookup(ImmutableArray<Definition> definitions) {
        Values = (
            from x1 in definitions
            where !string.IsNullOrWhiteSpace(x1.File.MimeType)
            from y1 in x1.File.Extensions
            where !string.IsNullOrWhiteSpace(y1)
            group x1 by y1.ToUpperInvariant()
            into G1
            let MimeTypes = (
                //Group the items by mime type and sort the m descending.
                from x2 in G1
                group x2 by x2.File.MimeType?.ToUpperInvariant()
                into G2
                let Matches = (
                    from y in G2
                    select new DefinitionMatch {
                        Definition = y,
                        Type = DefinitionMatchType.Unknown
                    }).ToImmutableArray()
                let v2 = new MimeTypeMatch {
                    MimeType = G2.Key,
                    Matches = Matches,
                    Points = Matches.Length
                }
                orderby v2.Points descending
                select v2
            ).ToImmutableArray()
            select new {
                G1.Key,
                MimeTypes
            }
        ).ToImmutableDictionary(x => x.Key, x => x.MimeTypes, StringComparer.InvariantCultureIgnoreCase);
    }

    public string? TryGetValue(string extension) {
        var ret = default(string?);

        if (TryGetValues(extension).FirstOrDefault() is { } v1) {
            ret = v1.MimeType;
        }


        return ret;
    }

    public ImmutableArray<MimeTypeMatch> TryGetValues(string extension) {
        extension = extension.TrimStart('.');

        var ret = ImmutableArray<MimeTypeMatch>.Empty;

        if (Values.TryGetValue(extension, out var tret)) {
            ret = tret;
        }

        return ret;
    }
}
