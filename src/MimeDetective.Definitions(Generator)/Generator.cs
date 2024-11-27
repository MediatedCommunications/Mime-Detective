using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeDetective.Storage;
using MimeDetective.Storage.Xml.v2;
using MimeDetective.Tests;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MimeDetective.Definitions;

[TestClass]
public class Generator {
    private static readonly FrozenSet<string> Extensions = new[]
    {
        "aif", "cda","mid", "midi","mp3", "mpa", "ogg","wav","wma", "wpl",
        "7z", "arj", "deb","pkg","rar","rpm","tar.gz","z","zip",
        "bin","dmg","iso","toast","vcd",
        "mdb", "xml",
        "eml","emlx","msg","oft","ost","pst", "vcf",
        "apk", "exe", "com","jar", "msi",
        "fnt", "fon", "otf","ttf",
        "ai", "bmp","gif", "ico","jpg","jpeg","png", "ps","psd","svg","tif", "tiff",
        "key", "odp", "pps", "ppt", "pptx",
        "ods", "xls", "xlsm", "xlsx",
        "cab", "cur", "icns", "ico", "lnk",
        "3g2","3gp","avi", "flv", "h264","m4v", "mkv", "mov","mp4","mpg", "mpeg", "rm", "swf", "vob","wmv",
        "doc","docx","odt","pdf","rtf","tex","wpd",
    }.ToFrozenSet(StringComparer.OrdinalIgnoreCase);

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
        //        select y.ToLowerInvariant()
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
                .Where(x => x.File.Extensions.Any(ext => Extensions.Contains(ext)))
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
