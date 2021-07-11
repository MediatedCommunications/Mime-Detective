using FileDetection.Data;
using FileDetection.Engine;
using FileDetection.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FileDetection.Data
{
    [TestClass]
    public class Generator
    {

        private static List<Definition> SourceData()
        {
            var Profile = System.Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            var Folder = $@"{Profile}\Downloads\triddefs_xml";

            var Items = new List<Definition>();
            foreach (var FileName in System.IO.Directory.EnumerateFiles(Folder, "*.xml", System.IO.SearchOption.AllDirectories))
            {
                var Id = System.IO.Path.GetFileNameWithoutExtension(FileName);

                var Item = FileDetection.Storage.Trid.v2.TridSerializer.FromXmlFile(FileName)
                    ?? throw new NullReferenceException()
                    ;

                var Modern = FileDetection.Storage.Trid.v2.DefinitionExtensions.Modernize(Item);
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

            var Items = SourceData();


            WriteSmall(Root, Items);
            WriteLarge(Root, Items);
            //WriteTrimmed(Root, Items);

        }

        private static void WriteSmall(string Root, IEnumerable<Definition> AllItems)
        {
            //List from:
            //https://www.computerhope.com/issues/ch001789.htm

            var ExtensionList = new[]
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
            };


            var Extensions = ExtensionList.ToImmutableHashSet(StringComparer.InvariantCultureIgnoreCase);

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

            var Json = $@"{Root}\FileDetection.Data.Small\Data\data.json";
            var Bin = $@"{Root}\FileDetection.Data.Small\Data\data.bin";

            DefinitionJsonSerializer.ToJsonFile(Json, I3);
            DefinitionBinarySerializer.ToBinaryFile(Bin, I3);
        }

        private static void WriteLarge(string Root, IEnumerable<Definition> AllItems) {
            var Items = AllItems
                .TrimMeta()
                .OrderBy(x => x.File.Extensions.FirstOrDefault())
                ;

            var Json = $@"{Root}\FileDetection.Data.Large\Data\data.json";
            var Bin = $@"{Root}\FileDetection.Data.Large\Data\data.bin";

            DefinitionJsonSerializer.ToJsonFile(Json, Items);
            DefinitionBinarySerializer.ToBinaryFile(Bin, Items);
        }


        private static void WriteTrimmed(string Root, IEnumerable<Definition> Items)
        {
            var NewItems = (
                from x in Items
                let v = x with
                {
                    Meta = default,
                    File = x.File with
                    {
                        Description = default,
                        MimeType = default,
                    },
                }
                select v
                ).ToList();

            var Bin = $@"{Root}\FileDetection.Data.Trimmed\Data\data.bin";
            DefinitionBinarySerializer.ToBinaryFile(Bin, NewItems);
        }

    }
}
