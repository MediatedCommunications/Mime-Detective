using MimeDetective.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

using MimeDetective.Storage;
using System.Linq;
using System.Net.Http.Headers;
using System.Collections.Frozen;
using System.Security.AccessControl;

namespace MimeDetective;

public record ContentInspectorResults : DisplayRecord {
    public required ImmutableDictionary<Definition, int> BytesMatched { get; init; }
    public required ImmutableDictionary<Definition, int> PointsAwarded { get; init; }

    public required ImmutableArray<Definition> Definitions { get; init; }

    public required ImmutableArray<IGrouping<string, Definition>> ByExtension { get; init; }
    public required ImmutableArray<IGrouping<string, Definition>> ByMimeType { get; init; }
}

internal static class ContentInspectorResultExtensions { 
    public static ContentInspectorResults ToResults(this ImmutableArray<Definition> This) {

        var BytesMatched = This
            .ToImmutableDictionary(x => x, x => x.GetBytes())
            ;

        var PointsAwarded = This
            .ToImmutableDictionary(x => x, x => x.GetPoints())
            ;

        var Definitions = PointsAwarded
            .OrderByDescending(x => x.Value)
            .Select(x => x.Key)
            .ToImmutableArray()
            ;

        var ByExtensions = (
            from Definition in This
            from Extension in Definition.File.Extensions
            select new {
                Extension,
                Definition,
            }).GroupBy(x => x.Extension, x => x.Definition, StringComparer.InvariantCultureIgnoreCase)
            .OrderByDescending(x => x.GetPoints())
            .ToImmutableArray();

        var ByMimeType = (
              from Definition in This
              let Mime = Definition.File.MimeType
              select new {
                  Mime,
                  Definition,
              }).GroupBy(x => x.Mime, x => x.Definition, StringComparer.InvariantCultureIgnoreCase)
              .OrderByDescending(x => x.GetPoints())
              .ToImmutableArray();


        var ret = new ContentInspectorResults() {
            BytesMatched = BytesMatched,
            PointsAwarded = PointsAwarded,
            Definitions = Definitions,
            ByExtension = ByExtensions,
            ByMimeType = ByMimeType,
        };

        return ret;

    }

    private static int GetPoints(this IEnumerable<Definition> Input) {
        var ret = Input.Select(x => x.GetPoints()).Sum();

        return ret;
    }

    private static int GetBytes(this Definition Input) {
        var ret = 0
            + Input.Signature.OffsetRules.Select(x => x.Bytes.Length).Sum()
            + Input.Signature.WildCardRules.Select(x => x.Bytes.Length).Sum()
            ;

        return ret;
    }

    private static int GetPoints(this Definition Input) {
        var ret = 0
            + Input.Signature.OffsetRules.Select(x => x.GetPoints()).Sum()
            + Input.Signature.WildCardRules.Select(x => x.GetPoints()).Sum()
            ;

        return ret;
    }

    private static int GetPoints(this OffsetRule Input) {
        var Multiplier = Input.Start == 0 ? 1000 : 5;

        var ret = Input.Bytes.Length * Multiplier;

        return ret;
    }

    private static int GetPoints(this WildCardRule Input) {
        var Multiplier = 1;

        var ret = Input.Bytes.Length * Multiplier;

        return ret;
    }

}