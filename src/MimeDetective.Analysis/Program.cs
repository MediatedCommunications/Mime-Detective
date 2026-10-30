using MimeDetective.Storage;
using MimeDetective;
using System.Collections.Immutable;

var definitions = ExhaustiveDefinitions.Load(UsageType.PersonalNonCommercial);
var matches = definitions
    .Select(definition => (Definition: definition, Bytes: OffsetBytes(definition)))
    .Where(item => item.Bytes.Count == 1)
    .Where(item => item.Definition.Signature.WildCardRules.IsEmpty)
    .OrderBy(item => item.Bytes[0].Value)
    .ThenBy(item => item.Bytes[0].Start)
    .ThenBy(item => item.Definition.File.Extensions.FirstOrDefault())
    .ToArray();

Console.WriteLine($"{matches.Length:N0} exhaustive definitions have exactly one offset-rule byte.");
Console.WriteLine();

foreach (var (definition, bytes) in matches) {
    var rule = bytes[0];
    var file = definition.File;
    var extensions = file.Extensions.IsDefaultOrEmpty ? "(none)" : string.Join(", ", file.Extensions);
    var mimeType = string.IsNullOrWhiteSpace(file.MimeType) ? "(none)" : file.MimeType;
    var description = string.IsNullOrWhiteSpace(file.Description) ? "(no description)" : file.Description;
    var wildcards = definition.Signature.WildCardRules
        .Select(rule => Readable(rule.Bytes))
        .Where(text => text.Length > 0)
        .ToArray();

    //Console.WriteLine($"{definition.Meta?.Id ?? "(no id)"}  .{extensions}  {mimeType}");
    Console.WriteLine($"  {description}");
    //Console.WriteLine($"  offset @{rule.Start} = 0x{rule.Bytes:X2} {Printable(rule.Bytes)}");
    //Console.WriteLine(wildcards.Length == 0
    //    ? "  wildcards: (none)"
    //    : $"  wildcards: {string.Join(", ", wildcards)}");
    //Console.WriteLine();
}

static List<OffsetByte> OffsetBytes(Definition definition) {
    var bytes = new List<OffsetByte>();
    foreach (var rule in definition.Signature.OffsetRules) {
        var value = rule.Bytes;
        for (var i = 0; i < value.Length; i++) {
            bytes.Add(new OffsetByte(rule.Start + i, value[i]));
        }
    }

    return bytes;
}

static string Readable(ImmutableArray<byte> value) {
    if (value.IsDefaultOrEmpty) {
        return string.Empty;
    }

    return string.Concat(value.Select(b => b switch {
        (byte)'\0' => "\\0",
        >= 0x20 and < 0x7F => ((char)b).ToString(),
        _ => $"\\x{b:X2}",
    }));
}

static string Printable(byte value) {
    return value is >= 0x20 and < 0x7F ? $"'{((char)value)}'" : "(non-printable)";
}

readonly record struct OffsetByte(int Start, byte Value);
