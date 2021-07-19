using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using MimeDetective.Storage.Xml.v2;

namespace MimeDetective.Storage.Xml.v2
{

    /// <summary>
    /// Convert XML definitions into <see cref="Engine.Definition"/>s.
    /// </summary>
    public static class XmlConverter
    {

        public static Storage.Definition Convert(Definition V1)
        {
            var ret = new Storage.Definition()
            {
                File = ConvertFile(V1),
                Meta = ConvertMeta(V1),
                Signature = ConvertSignature(V1),
            };

            return ret;
        }

        private static class CategoryExtractor {
            private static ImmutableDictionary<string, Category[]> Lookup { get; }
            
            public static string Tokenize(string Text) {
                var sb = new StringBuilder();

                var Space = (char)32;
                var Last = 0;
                foreach (var c in Text.ToLower()) {
                    var Next = c;
                    if (!char.IsLetterOrDigit(c)) {
                        Next = Space;
                    }

                    if (Next == Space && Last == Space) {
                        //Ignore it.
                    } else {
                        sb.Append(Next);
                    }

                    Last = Next;
                }

                var ret = sb.ToString();
                while (true) {
                    var New = ret.Replace("  ", " ");

                    if(New == ret) {
                        break;
                    }

                    ret = New;
                }

                return ret;
            }

            public static ImmutableHashSet<Category> Extract(string Text) {
                var ret = new List<Category>();

                var Input = Tokenize(Text);

                foreach (var item in Lookup) {
                    if (Input.Contains(item.Key)) {
                        ret.AddRange(item.Value);
                    }
                }

                if(ret.Count == 0) {

                }

                return ret.ToImmutableHashSet();
            }
            
            static CategoryExtractor() {
                Lookup = new Dictionary<string, Category[]>() {

                    
                    ["archive"] = new[] { Category.Archive },
                    ["backup"] = new[] { Category.Archive },
                    ["bundle"] = new[] { Category.Archive },
                    ["cabinet"] = new[] { Category.Archive },
                    ["collection"] = new[] { Category.Archive },
                    ["composite"] = new[] { Category.Archive },
                    ["compound"] = new[] { Category.Archive },
                    ["container"] = new[] { Category.Archive },
                    ["export"] = new[] { Category.Archive },
                    ["library"] = new[] { Category.Archive },
                    ["package"] = new[] { Category.Archive },
                    ["set"] = new[] { Category.Archive },
                    ["sets"] = new[] { Category.Archive },
                    ["zip"] = new[] { Category.Archive },

                    ["audio"] = new[] { Category.Audio },
                    ["instrument"] = new[] { Category.Audio },
                    ["music"] = new[] { Category.Audio },
                    ["sample"] = new[] { Category.Audio },
                    ["song"] = new[] { Category.Audio },
                    ["sound"] = new[] { Category.Audio },
                    ["track"] = new[] { Category.Audio },
                    ["voice"] = new[] { Category.Audio },
                    ["waveform"] = new[] { Category.Audio },

                    ["big endian"] = new[] { Category.BigEndian },

                    ["compress"] = new[] { Category.Compressed },
                    ["packed"] = new[] { Category.Compressed },

                    ["config"] = new[] { Category.Configuration },
                    ["profile"] = new[] { Category.Configuration },
                    ["preference"] = new[] { Category.Configuration },
                    ["setting"] = new[] { Category.Configuration },

                    ["bank"] = new[] { Category.Database },
                    ["cache"] = new[] {Category.Database},
                    ["dictionary"] = new[] { Category.Database },
                    ["database"] = new[] { Category.Database },
                    ["data base"] = new[] { Category.Database },
                    ["db"] = new[] { Category.Database },
                    ["index"] = new[] { Category.Database },
                    ["table"] = new[] { Category.Database },

                    ["document"] = new[] { Category.Document },

                    ["email"] = new[] { Category.Email },
                    ["e mail"] = new[] { Category.Email },

                    ["encrypted"] = new[] { Category.Encrypted },
                    ["password"] = new[] { Category.Encrypted },
                    ["protected"] = new[] { Category.Encrypted },

                    ["applet"] = new[] { Category.Executable },
                    ["application"] = new[] { Category.Executable },
                    ["assembly"] = new[] { Category.Executable },
                    ["bitcode"] = new[] { Category.Executable },
                    ["bytecode"] = new[] { Category.Executable },
                    ["codec"] = new[] { Category.Executable },
                    ["compiled"] = new[] { Category.Executable },
                    ["driver"] = new[] { Category.Executable },
                    ["executable"] = new[] { Category.Executable },
                    ["plug in"] = new[] { Category.Executable },
                    ["plugin"] = new[] { Category.Executable },
                    ["program"] = new[] { Category.Executable },

                    ["font"] = new[] { Category.Font },

                    ["drawing"] = new[] { Category.Image },
                    ["graphic"] = new[] { Category.Image },
                    ["icon"] = new[] { Category.Image },
                    ["image"] = new[] { Category.Image },
                    ["photo"] = new[] { Category.Image },
                    ["picture"] = new[] { Category.Image },
                    ["skin"] = new[] { Category.Image },
                    ["theme"] = new[] { Category.Image },
                    ["thumbnail"] = new[] { Category.Image },

                    ["little endian"] = new[] { Category.LittleEndian },

                    ["log"] = new[] { Category.Log },

                    ["lossless"] = new[] { Category.Lossless },

                    ["lossy"] = new[] { Category.Lossy },

                    ["powerpoint"] = new[] { Category.Presentation },

                    ["project"] = new[] {Category.Project},
                    ["workspace"] = new[] { Category.Project },

                    ["macro"] = new[] { Category.Script },
                    ["script"] = new[] { Category.Script },
                    ["source"] = new[] { Category.Script },

                    ["spreadsheet"] = new[] { Category.Spreadsheet },
                    ["workbook"] = new[] { Category.Spreadsheet },
                    ["worksheet"] = new[] { Category.Spreadsheet },

                    ["preset"] = new[] { Category.Template },
                    ["template"] = new[] {Category.Template},

                    ["utf8"] = new[] { Category.Utf8 },
                    ["utf 8"] = new[] { Category.Utf8 },

                    ["utf16"] = new[] { Category.Utf16 },
                    ["utf 16"] = new[] { Category.Utf16 },

                    ["utf32"] = new[] { Category.Utf32 },
                    ["utf 32"] = new[] { Category.Utf32 },

                    ["diagram"] = new[] { Category.Vector },
                    ["geometry"] = new[] { Category.Vector },
                    ["model"] = new[] { Category.Vector },
                    ["shape"] = new[] { Category.Vector },
                    ["vector"] = new[] { Category.Vector },
                    ["voxel"] = new[] {Category.Vector },

                    ["animation"] = new[] { Category.Video },
                    ["animated"] = new[] { Category.Video },
                    ["movie"] = new[] { Category.Video },
                    ["multimedia"] = new[] { Category.Video },
                    ["sprite"] = new[] { Category.Video },
                    ["video"] = new[] { Category.Video },

                    ["firmware"] = new[] { Category.Executable, Category.DiskImage },
                    ["rom"] = new[] { Category.Executable, Category.DiskImage },


                    ["playlist"] = new[] { Category.Audio, Category.Database },
                    ["installer"] = new[] { Category.Executable, Category.Archive },

                    
                    ["3d"] = new[] { Category.Image, Category.Vector },
                    ["2d"] = new[] { Category.Image, Category.Vector },
                    ["3 d"] = new[] { Category.Image, Category.Vector },
                    ["2 d"] = new[] { Category.Image, Category.Vector },
                    
                    ["disk image"] = new[] { Category.DiskImage, Category.Archive },
                    
                    ["raster"] = new[] { Category.Bitmap, Category.Image },
                    ["bitmap"] = new[] { Category.Bitmap, Category.Image },
                   

                }.ToImmutableDictionary();
            }
        }


        private static Storage.FileType ConvertFile(Definition V1)
        {
            var Extensions = $@"{V1.Info?.FileExtension}"
                .Split(new[] { @"/" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim().ToLower())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToImmutableArray()
                ;

            var Description = $@"{V1.Info?.FileType}";
            var MimeType = $@"{V1.Info?.MimeType}";

            var Categories = CategoryExtractor.Extract(Description);

            var ret = new Storage.FileType()
            {
                Description = Description,
                Extensions = Extensions,
                MimeType = MimeType,
                Categories = Categories,
            };

            return ret;
        }

        private static Storage.Meta ConvertMeta(Definition V1)
        {
            var ret = new Storage.Meta()
            {
                Created = new()
                {
                    At = new DateTime(
                            Math.Max(V1.General?.Date?.Year ?? 0, 1901),
                            Math.Max(V1.General?.Date?.Month ?? 0, 1),
                            Math.Max(V1.General?.Date?.Day ?? 0, 1),
                            V1.General?.Time?.Hour ?? 0,
                            V1.General?.Time?.Min ?? 0,
                            V1.General?.Time?.Sec ?? 0
                            ),
                    By = new Storage.Author()
                    {
                        Name = $@"{V1.Info?.Author}",
                        Email = $@"{V1.Info?.AuthorEmail}",
                        Website = $@"{V1.Info?.Website}",
                    },
                    Source = new Storage.Source()
                    {
                        Files = V1.General?.FileNum ?? 0,
                    },
                    Using = new Storage.Generator()
                    {
                        Application = $@"{V1.General?.Creator}"
                    },
                },
                Reference = new Storage.Reference()
                {
                    Text = $@"{V1.Info?.ExtraInfo?.Remark}",
                    Uri = $@"{V1.Info?.ExtraInfo?.ReferenceUrl}",
                },
            };

            return ret;
        }

        private static Storage.Signature ConvertSignature(Definition V1)
        {
            var ret = new Storage.Signature()
            {
                Prefix = V1
                    .FrontBlock
                    .Select(x => ConvertPattern(x))
                    .OrderBy(x => x.Start)
                    .ToImmutableArray()
                    ,

                Strings = V1
                    .GlobalStrings
                    .Select(x => ConvertGlobalString(x))
                    .OrderByDescending(x => x.Pattern.Length)
                    .ToImmutableArray()
                    ,

            };

            return ret;
        }

        private static Storage.StringSegment ConvertGlobalString(string V1)
        {
            return StringSegment.Create(V1, true);
        }

        private static Storage.PrefixSegment ConvertPattern(Pattern V1)
        {
            var Position = V1.Position;

            var Bytes = V1.Bytes ?? string.Empty;
            while (Bytes.Length % 2 != 0)
            {
                Bytes = "0" + Bytes;
            }

            var Content = System.Convert.FromHexString(Bytes).ToImmutableArray();

            var ret = new Storage.PrefixSegment()
            {
                Start = Position,
                Pattern = Content,
            };

            return ret;

        }

    }
}
