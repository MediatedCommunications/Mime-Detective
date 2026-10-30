using MimeDetective.Engine;
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
        ImmutableArray<DefinitionMatch> matches,
        string fileName,
        long fileSize,
        int bytesInspected,
        string? browserContentType,
        DefinitionSet definitionSet,
        TimeSpan elapsed) {

        var ordered = matches
            .OrderByDescending(static match => match.Points)
            .ThenByDescending(static match => match.Percentage)
            .ToImmutableArray();

        return new InspectionReport(
            fileName,
            fileSize,
            bytesInspected,
            string.IsNullOrWhiteSpace(browserContentType) ? null : browserContentType,
            definitionSet,
            elapsed,
            [.. ordered.ByFileExtension().Select(static match => new ExtensionResult(match.Extension, match.Points)),],
            [.. ordered.ByMimeType().Select(static match => new MimeResult(match.MimeType, match.Points)),],
            [.. ordered.Select(static match => MatchResult.From(match)),]);
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
    decimal Percentage,
    string MatchType,
    IReadOnlyList<SegmentDetail> Prefixes,
    IReadOnlyList<SegmentDetail> Strings) {

    public static MatchResult From(DefinitionMatch match) {
        var file = match.Definition.File;
        var description = string.IsNullOrWhiteSpace(file.Description) ? "No description" : file.Description;
        var mimeType = string.IsNullOrWhiteSpace(file.MimeType) ? "(none)" : file.MimeType;
        var signature = match.Definition.Signature;

        return new MatchResult(
            description,
            mimeType,
            file.Extensions,
            [.. file.Categories.Where(static category => category != Category.None).Select(FormatCategory),],
            match.Points,
            match.Percentage,
            match.Type.ToString(),
            [.. signature.Prefix.Select(prefix => SegmentDetail.FromPrefix(prefix, match)),],
            [.. signature.Strings.Select(segment => SegmentDetail.FromString(segment, match)),]);
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

public sealed record SegmentDetail(int? Offset, string Text, string Hex, bool Matched, int? FoundAt) {
    public static SegmentDetail FromPrefix(PrefixSegment segment, DefinitionMatch match) {
        var matched = false;
        foreach (var (key, result) in match.PrefixSegmentMatches) {
            if (!PrefixSegmentEqualityComparer.Instance.Equals(key, segment)) {
                continue;
            }

            matched = result is not NoSegmentMatch;
            break;
        }

        return new(segment.Start, Readable(segment.Pattern), HexOf(segment.Pattern), matched, null);
    }

    public static SegmentDetail FromString(StringSegment segment, DefinitionMatch match) {
        int? foundAt = null;
        foreach (var (key, result) in match.StringSegmentMatches) {
            if (!StringSegmentEqualityComparer.Instance.Equals(key, segment)) {
                continue;
            }

            if (result is StringSegmentMatch hit) {
                foundAt = hit.Index;
            }

            break;
        }

        return new(null, Readable(segment.Pattern), HexOf(segment.Pattern), foundAt is not null, foundAt);
    }

    private static string Readable(ImmutableArray<byte> pattern) {
        if (pattern.IsDefaultOrEmpty) {
            return string.Empty;
        }

        var chars = new char[pattern.Length];
        for (var i = 0; i < pattern.Length; i++) {
            var value = pattern[i];
            chars[i] = value switch {
                0 => '\'',
                >= 32 and < 127 => (char)value,
                _ => '·',
            };
        }

        return new string(chars);
    }

    private static string HexOf(ImmutableArray<byte> pattern) {
        if (pattern.IsDefaultOrEmpty) {
            return string.Empty;
        }

        var builder = new StringBuilder(pattern.Length * 3);
        for (var i = 0; i < pattern.Length; i++) {
            if (i > 0) {
                builder.Append(' ');
            }

            builder.Append(pattern[i].ToString("X2"));
        }

        return builder.ToString();
    }
}
