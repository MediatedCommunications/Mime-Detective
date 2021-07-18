using MimeDetective.Definitions;
using MimeDetective.Engine;
using MimeDetective.Storage;
using MimeDetective.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective.Definitions
{
    [TestClass]
    public class Generator
    {



        private static List<Definition> MacroSourceData()
        {
            
            var Folder = SourceDefinitions.DefinitionRoot();

            var Items = new List<Definition>();
            foreach (var FileName in System.IO.Directory.EnumerateFiles(Folder, "*.xml", System.IO.SearchOption.AllDirectories))
            {
                var Id = System.IO.Path.GetFileNameWithoutExtension(FileName);

                var Item = MimeDetective.Storage.Xml.v2.XmlSerializer.FromXmlFile(FileName)
                    ?? throw new NullReferenceException()
                    ;

                var Modern = MimeDetective.Storage.Xml.v2.DefinitionExtensions.Modernize(Item);
                Modern = Modern with
                {
                    Meta = (Modern.Meta ?? new()) with
                    {
                        Id = Id
                    }
                };

                Items.Add(Modern);

            }

            return Items;
        }

        [TestMethod]
        public void Generate()
        {
            
            var Root = System.IO.Path.GetFullPath(@"..\..\..\..\");

            var Items = MacroSourceData();


            WriteSmall(Root, Items);
            WriteLarge(Root, Items);
            //WriteTrimmed(Root, Items);

        }

        private static void WriteSmall(string Root, IEnumerable<Definition> AllItems)
        {
            //List from:
            //https://www.computerhope.com/issues/ch001789.htm

            var Extensions = new[]
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
            }.ToImmutableHashSet(StringComparer.InvariantCultureIgnoreCase);


            var I1 = (
                from x in AllItems
                let HasExtension = x.File.Extensions.Any(ext => Extensions.Contains(ext))
                where HasExtension
                select x
                ).ToList();

            var I2 = I1
                .TrimMeta()
                .TrimExtensions(Extensions)
                .ToList()
                ;

            var I3 = I2
                //.Minify()
                .TrimDescription()
                .OrderBy(x => x.File.Extensions.FirstOrDefault())
                .ToList()
                ;

            var Json = $@"{Root}\MimeDetective.Definitions.Common\Data\data.json";
            var Binn = $@"{Root}\MimeDetective.Definitions.Common\Data\data.bin";

            DefinitionJsonSerializer.ToJsonFile(Json, I3);
            DefinitionBinarySerializer.ToBinaryFile(Binn, I3);
        }

        private static void WriteLarge(string Root, IEnumerable<Definition> AllItems) {
            var Items = AllItems
                .TrimMeta()
                .OrderBy(x => x.File.Extensions.FirstOrDefault())
                ;

            var Json = $@"{Root}\MimeDetective.Definitions.Exhaustive\Data\data.json";
            var Binn = $@"{Root}\MimeDetective.Definitions.Exhaustive\Data\data.bin";

            DefinitionJsonSerializer.ToJsonFile(Json, Items);
            DefinitionBinarySerializer.ToBinaryFile(Binn, Items);
        }

    }
}
