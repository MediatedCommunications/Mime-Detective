
using MimeDetective.Storage;
using System.Collections.Immutable;
using System.Text;

namespace MimeDetective.Web;

public sealed record InspectionReport(
    string FileName,
    long FileSize,
    int BytesInspected,
    string? BrowserContentType,
    DefinitionSet DefinitionSet,
    TimeSpan Elapsed,
    IReadOnlyList<ExtensionResult> Extensions,
    IReadOnlyList<MimeResult> MimeTypes,
    IReadOnlyList<MatchResult> Matches) {

    public bool Truncated => FileSize > BytesInspected;

    public ExtensionResult? LeadingExtension => Extensions.Count > 0 ? Extensions[0] : null;

    public MimeResult? LeadingMimeType => MimeTypes.Count > 0 ? MimeTypes[0] : null;

    public MatchResult? LeadingMatch => Matches.Count > 0 ? Matches[0] : null;

    public static InspectionReport Create(
        ContentInspectorResults results,
        ReadOnlySpan<byte> content,
        string fileName,
        long fileSize,
        int bytesInspected,
        string? browserContentType,
        DefinitionSet definitionSet,
        TimeSpan elapsed) {

        var extensions = new List<ExtensionResult>(results.ByExtension.Length);
        foreach (var group in results.ByExtension) {
            extensions.Add(new ExtensionResult(group.Key, PointsFor(group, results)));
        }

        var mimeTypes = new List<MimeResult>(results.ByMimeType.Length);
        foreach (var group in results.ByMimeType) {
            mimeTypes.Add(new MimeResult(group.Key, PointsFor(group, results)));
        }

        var matches = new List<MatchResult>(results.Definitions.Length);
        foreach (var definition in results.Definitions) {
            matches.Add(MatchResult.From(
                definition,
                results.PointsAwarded[definition],
                results.BytesMatched[definition],
                content));
        }

        return new InspectionReport(
            fileName,
            fileSize,
            bytesInspected,
            string.IsNullOrWhiteSpace(browserContentType) ? null : browserContentType,
            definitionSet,
            elapsed,
            extensions,
            mimeTypes,
            matches);
    }

    private static long PointsFor(IEnumerable<Definition> definitions, ContentInspectorResults results) {
        long total = 0;
        foreach (var definition in definitions) {
            total += results.BytesMatched[definition];
        }

        return total;
    }

    public static string FormatSize(long bytes) {
        const double kilobyte = 1024;
        const double megabyte = kilobyte * 1024;
        const double gigabyte = megabyte * 1024;

        return bytes switch {
            < 1024 => $"{bytes} B",
            < 1024 * 1024 => $"{bytes / kilobyte:0.#} KB",
            < 1024L * 1024 * 1024 => $"{bytes / megabyte:0.#} MB",
            _ => $"{bytes / gigabyte:0.#} GB",
        };
    }

    public static string FormatElapsed(TimeSpan elapsed) {
        var milliseconds = elapsed.TotalMilliseconds;
        return milliseconds switch {
            < 1 => "<1 ms",
            < 1000 => $"{milliseconds:0} ms",
            _ => $"{elapsed.TotalSeconds:0.00} s",
        };
    }
}

public sealed record ExtensionResult(string Extension, long Points);

public sealed record MimeResult(string? MimeType, long Points) {
    public string DisplayMimeType => string.IsNullOrWhiteSpace(MimeType) ? "(none)" : MimeType;
}

public sealed record MatchResult(
    string Description,
    string MimeType,
    IReadOnlyList<string> Extensions,
    IReadOnlyList<string> Categories,
    long Points,
    long Bytes,
    decimal Percentage,
    IReadOnlyList<SegmentDetail> Prefixes,
    IReadOnlyList<SegmentDetail> Strings) {

    public static MatchResult From(Definition definition, int points, int bytes, ReadOnlySpan<byte> content) {
        var file = definition.File;
        var description = string.IsNullOrWhiteSpace(file.Description) ? "No description" : file.Description;
        var mimeType = string.IsNullOrWhiteSpace(file.MimeType) ? "(none)" : file.MimeType;
        var signature = definition.Signature;

        var prefixes = new List<SegmentDetail>(signature.OffsetRules.Length);
        foreach (var prefix in signature.OffsetRules) {
            prefixes.Add(SegmentDetail.FromPrefix(prefix));
        }

        var strings = new List<SegmentDetail>(signature.WildCardRules.Length);
        foreach (var segment in signature.WildCardRules) {
            strings.Add(SegmentDetail.FromString(segment, content));
        }

        var ret = new MatchResult(
            description,
            mimeType,
            file.Extensions,
            [.. file.Categories.Where(static category => category != Category.None).Select(FormatCategory),],
            points,
            bytes,
            1,
            prefixes,
            strings);

        return ret;
    }

    public static string FormatCategory(Category category) {
        var name = category.ToString();
        if (name.Length < 2) {
            return name;
        }

        var builder = new StringBuilder(name.Length + 4);
        builder.Append(name[0]);
        for (var i = 1; i < name.Length; i++) {
            if (char.IsUpper(name[i]) && char.IsLower(name[i - 1])) {
                builder.Append(' ');
            }

            builder.Append(name[i]);
        }

        return builder.ToString();
    }
}

public sealed record SegmentDetail(int? Offset, string Text, string Hex) {
    public static SegmentDetail FromPrefix(OffsetRule segment) {
        var ret = new SegmentDetail(segment.Start, Readable(segment.Bytes), HexOf(segment.Bytes));
        return ret;
    }

    public static SegmentDetail FromString(WildCardRule segment, ReadOnlySpan<byte> content) {
        var index = content.IndexOf(segment.Bytes.AsSpan());
        int? foundAt = index >= 0 ? index : null;
        var ret = new SegmentDetail(foundAt, Readable(segment.Bytes), HexOf(segment.Bytes));
        return ret;
    }

    private static string Readable(ImmutableArray<byte> pattern) {
        if (pattern.IsDefaultOrEmpty) {
            return string.Empty;
        }

        var sb = new StringBuilder(pattern.Length * 4);

        foreach (var b in pattern) {

            var Text = b switch {
                (byte)'\\' => @"   \\",
                (byte)'\0' => @"   \0",
                (byte)'\a' => @"   \a",
                (byte)'\b' => @"   \b",
                (byte)'\f' => @"   \f",
                (byte)'\n' => @"   \n",
                (byte)'\r' => @"   \r",
                (byte)'\t' => @"   \t",
                (byte)'\v' => @"   \v",
                (byte)'\"' => "   \\\"",
                >= 0x20 and < 0x7F => $@"    {(char)b}",
                _ => $@"\\x{b:X2}",
            };

            sb.Append(Text);
        }

        var ret = sb.ToString();
        return ret;
    }


    private static string HexOf(ImmutableArray<byte> pattern) {
        if (pattern.IsDefaultOrEmpty) {
            return string.Empty;
        }

        var builder = new StringBuilder(pattern.Length * 3);
        for (var i = 0; i < pattern.Length; i++) {
            var Hex = pattern[i].ToString("X2");
            builder.Append("   ");
            builder.Append(Hex);
        }

        var ret = builder.ToString();

        return ret;
    }
}
