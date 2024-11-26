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
        var Folder = SourceDefinitions.DefinitionRoot();

        var definitions =
            from FileName in Directory.EnumerateFiles(Folder, "*.xml", SearchOption.AllDirectories).AsParallel()
            let Id = Path.GetFileNameWithoutExtension(FileName)
            let Item = XmlSerializer.FromXmlFile(FileName)
                ?? throw new InvalidOperationException($"Unable to deserialize {FileName}")
            let Modern = Item.Modernize()
            select Modern with { Meta = (Modern.Meta ?? new()) with { Id = Id } };

        return [.. definitions.OrderBy(item => item.Meta?.Id, StringComparer.OrdinalIgnoreCase)];
    }

    [TestMethod]
    public void Generate() {
        var Root = Path.GetFullPath(Path.Combine("..", "..", "..", ".."));

        var Items = MacroSourceData();

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

        WriteSmall(Root, Items);
        WriteLarge(Root, Items);
        //WriteTrimmed(Root, Items);
    }

    private static void WriteSmall(string Root, IEnumerable<Storage.Definition> AllItems) {
        //List from:
        //https://www.computerhope.com/issues/ch001789.htm


        var I1 = AllItems
                .Where(x => x.File.Extensions.Any(ext => Extensions.Contains(ext)))
#if DEBUG
                .ToArray()
#endif
            ;

        var I2 = I1
                .TrimMeta()
                .TrimExtensions(Extensions)
#if DEBUG
                .ToArray()
#endif
            ;

        var I3 = I2
                //.Minify()
                .TrimDescription()
                .OrderBy(x => x.File.Extensions.FirstOrDefault())
                .ToArray()
            ;

        var BaseDir = Path.Combine(Root, "MimeDetective.Definitions.Condensed", "Data");

        WriteFiles(BaseDir, I3);
    }

    private static void WriteLarge(string Root, IEnumerable<Storage.Definition> AllItems) {
        var Items = AllItems
                .TrimMeta()
                .OrderBy(x => x.File.Extensions.FirstOrDefault())
                .ToArray()
            ;

        var BaseDir = Path.Combine(Root, "MimeDetective.Definitions.Exhaustive", "Data");

        WriteFiles(BaseDir, Items);
        var Json = $@"{Root}\MimeDetective.Definitions.Exhaustive\Data\data.json";
        var Binn = $@"{Root}\MimeDetective.Definitions.Exhaustive\Data\data.bin";

        DefinitionJsonSerializer.ToJsonFile(Json, Items);
        DefinitionBinarySerializer.ToBinaryFile(Binn, Items);
    }

    private static void WriteFiles(string BaseDir, IReadOnlyCollection<Storage.Definition> Items) {
        var Json = Path.Combine(BaseDir, "data.json");
        var Binn = Path.Combine(BaseDir, "data.bin");

        DefinitionJsonSerializer.ToJsonFile(Json, Items);
        DefinitionBinarySerializer.ToBinaryFile(Binn, Items);
    }
}
