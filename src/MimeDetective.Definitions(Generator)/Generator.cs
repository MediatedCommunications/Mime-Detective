using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeDetective.Storage;
using MimeDetective.Storage.Xml.v2;
using MimeDetective.Tests;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MimeDetective.Definitions.Generator;

[TestClass]
public class Generator {
    private static readonly FrozenSet<string> Extensions = FrozenSet.Create(
        StringComparer.OrdinalIgnoreCase, [
        // @formatter:off
        // Audio
        "aif", "aiff", "aac", "cda", "flac", "m4a", "mid", "midi", "mp3", "mpa", "ogg", "wav", "wma", "wpl",
        // Archive
        "7z", "arj", "bz", "bz2", "deb", "pkg", "rar", "rpm", "tar", "tar.gz", "xz", "z", "zip",
        // Binary images
        "bin", "dmg", "iso", "toast", "vcd",
        // Data
        "accdb", "json", "mdb", "odb", "xml",
        // EMail
        "eml", "emlx", "msg", "oft", "ost", "pst", "vcf",
        // Installers
        "apk", "appx", "exe", "com", "jar", "msi", "msix",
        // Fonts
        "fnt", "fon", "otf", "ttf",
        // Images
        "ai", "avif", "bmp", "gif", "heic", "heif", "ico", "jpg", "jpeg", "png", "ps", "psd", "svg", "tif", "tiff", "webp",
        // Presentation
        "key", "odp", "pps", "ppt", "pptx",
        // Spreadsheet
        "ods", "xls", "xlsm", "xlsx",
        // Windows
        "cab", "cur", "icns", "ico", "lnk",
        // Videos
        "3g2", "3gp", "avi", "flv", "h264", "m4v", "mkv", "mov", "mp4", "mpg", "mpeg", "rm", "swf", "vob", "webm", "wmv",
        // Documents
        "doc", "docx", "odt", "pdf", "rtf", "tex", "wpd"
        // @formatter:on
    ]);

    private static Storage.Definition[] MacroSourceData() {
        var folder = SourceDefinitions.DefinitionRoot();

        var definitions =
            from FileName in Directory.EnumerateFiles(folder, "*.xml", SearchOption.AllDirectories).AsParallel()
            let Id = Path.GetFileNameWithoutExtension(FileName)
            let Item = XmlSerializer.FromXmlFile(FileName)
                ?? throw new InvalidOperationException($"Unable to deserialize {FileName}")
            let Modern = Item.Modernize()
            select Modern with { Meta = (Modern.Meta ?? new()) with { Id = Id } };

        return [.. definitions.OrderBy(item => item.Meta?.Id, StringComparer.OrdinalIgnoreCase)];
    }

    [TestMethod]
    public void Generate() {
        var root = Path.GetFullPath(Path.Combine("..", "..", "..", ".."));

        var items = MacroSourceData();

        //var NoNames = (
        //        from x in Items
        //        where x.File.Categories.IsEmpty
        //        from y in (x.File.Description ?? "").Split(" ",
        //            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
        //        select y.ToUpperInvariant()
        //    ).GroupBy(x => x)
        //    .Select(x => (Key: x, Value: x.Count()))
        //    .OrderByDescending(x => x.Value)
        //    .ToArray();

        WriteSmall(root, items);
        WriteLarge(root, items);
        //WriteTrimmed(Root, Items);
    }

    private static void WriteSmall(string root, IEnumerable<Storage.Definition> allItems) {
        //List from:
        //https://www.computerhope.com/issues/ch001789.htm


        var i1 = allItems
                .Where(x => x.File.Extensions.Any(Extensions.Contains))
#if DEBUG
                .ToArray()
#endif
            ;

        var i2 = i1
                .TrimMeta()
                .TrimExtensions(Extensions)
#if DEBUG
                .ToArray()
#endif
            ;

        var i3 = i2
                //.Minify()
                .TrimDescription()
                .OrderBy(x => x.File.Extensions.FirstOrDefault())
                .ToArray()
            ;

        var baseDir = Path.Combine(root, "MimeDetective.Definitions.Condensed", "Data");

        WriteFiles(baseDir, i3);
    }

    private static void WriteLarge(string root, IEnumerable<Storage.Definition> allItems) {
        var items = allItems
                .TrimMeta()
                .OrderBy(x => x.File.Extensions.FirstOrDefault())
                .ToArray()
            ;

        var baseDir = Path.Combine(root, "MimeDetective.Definitions.Exhaustive", "Data");

        WriteFiles(baseDir, items);
        var json = $@"{root}\MimeDetective.Definitions.Exhaustive\Data\data.json";
        var binn = $@"{root}\MimeDetective.Definitions.Exhaustive\Data\data.bin";

        DefinitionJsonSerializer.ToJsonFile(json, items);
        DefinitionBinarySerializer.ToBinaryFile(binn, items);
    }

    private static void WriteFiles(string baseDir, IReadOnlyCollection<Storage.Definition> items) {
        var json = Path.Combine(baseDir, "data.json");
        var binn = Path.Combine(baseDir, "data.bin");

        DefinitionJsonSerializer.ToJsonFile(json, items);
        DefinitionBinarySerializer.ToBinaryFile(binn, items);
    }
}
