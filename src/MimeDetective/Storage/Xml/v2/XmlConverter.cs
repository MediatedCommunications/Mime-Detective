using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace MimeDetective.Storage.Xml.v2;

/// <summary>
///     Convert XML definitions into <see cref="Engine.Definition" />s.
/// </summary>
public static class XmlConverter {
    public static Storage.Definition Convert(Definition V1) {
        var ret = new Storage.Definition {
            File = ConvertFile(V1),
            Meta = ConvertMeta(V1),
            Signature = ConvertSignature(V1)
        };

        return ret;
    }


    private static FileType ConvertFile(Definition V1) {
        var Extensions = $@"{V1.Info?.FileExtension}"
                .Split([@"/"], StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim().ToLowerInvariant())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToImmutableArray()
            ;

        var Description = $@"{V1.Info?.FileType}";
        var MimeType = $@"{V1.Info?.MimeType}";

        var Categories = CategoryExtractor.Extract(Description);

        var ret = new FileType {
            Description = Description,
            Extensions = Extensions,
            MimeType = MimeType,
            Categories = Categories
        };

        return ret;
    }

    private static Meta ConvertMeta(Definition V1) {
        var ret = new Meta {
            Created = new() {
                At = new DateTime(
                    Math.Max(V1.General?.Date?.Year ?? 0, 1901),
                    Math.Max(V1.General?.Date?.Month ?? 0, 1),
                    Math.Max(V1.General?.Date?.Day ?? 0, 1),
                    V1.General?.Time?.Hour ?? 0,
                    V1.General?.Time?.Min ?? 0,
                    V1.General?.Time?.Sec ?? 0
                ),
                By = new() {
                    Name = $@"{V1.Info?.Author}",
                    Email = $@"{V1.Info?.AuthorEmail}",
                    Website = $@"{V1.Info?.Website}"
                },
                Source = new() { Files = V1.General?.FileNum ?? 0 },
                Using = new() { Application = $@"{V1.General?.Creator}" }
            },
            Reference = new() {
                Text = $@"{V1.Info?.ExtraInfo?.Remark}",
                Uri = $@"{V1.Info?.ExtraInfo?.ReferenceUrl}"
            }
        };

        return ret;
    }

    private static Signature ConvertSignature(Definition V1) {
        var ret = new Signature {
            Prefix = [
                ..V1
                    .FrontBlock
                    .Select(ConvertPattern)
                    .OrderBy(x => x.Start)
            ],
            Strings = [
                ..V1
                    .GlobalStrings
                    .Select(ConvertGlobalString)
                    .OrderByDescending(x => x.Pattern.Length)
            ]
        };

        return ret;
    }

    private static StringSegment ConvertGlobalString(string V1) {
        return StringSegment.Create(V1);
    }

    private static PrefixSegment ConvertPattern(Pattern V1) {
        var Position = V1.Position;

        var Bytes = V1.Bytes ?? string.Empty;
        while (Bytes.Length % 2 != 0) {
            Bytes = "0" + Bytes;
        }

        var Content = System.Convert.FromHexString(Bytes).ToImmutableArray();

        var ret = new PrefixSegment {
            Start = Position,
            Pattern = Content
        };

        return ret;
    }

    private static class CategoryExtractor {
        private static ImmutableDictionary<string, Category[]> Lookup { get; }

        static CategoryExtractor() {
            Lookup = new Dictionary<string, Category[]> {
                ["archive"] = [Category.Archive],
                ["backup"] = [Category.Archive],
                ["bundle"] = [Category.Archive],
                ["cabinet"] = [Category.Archive],
                ["collection"] = [Category.Archive],
                ["composite"] = [Category.Archive],
                ["compound"] = [Category.Archive],
                ["container"] = [Category.Archive],
                ["export"] = [Category.Archive],
                ["library"] = [Category.Archive],
                ["package"] = [Category.Archive],
                ["set"] = [Category.Archive],
                ["sets"] = [Category.Archive],
                ["zip"] = [Category.Archive],
                ["audio"] = [Category.Audio],
                ["instrument"] = [Category.Audio],
                ["music"] = [Category.Audio],
                ["sample"] = [Category.Audio],
                ["song"] = [Category.Audio],
                ["sound"] = [Category.Audio],
                ["track"] = [Category.Audio],
                ["voice"] = [Category.Audio],
                ["waveform"] = [Category.Audio],
                ["big endian"] = [Category.BigEndian],
                ["compress"] = [Category.Compressed],
                ["packed"] = [Category.Compressed],
                ["config"] = [Category.Configuration],
                ["profile"] = [Category.Configuration],
                ["preference"] = [Category.Configuration],
                ["setting"] = [Category.Configuration],
                ["bank"] = [Category.Database],
                ["cache"] = [Category.Database],
                ["dictionary"] = [Category.Database],
                ["database"] = [Category.Database],
                ["data base"] = [Category.Database],
                ["db"] = [Category.Database],
                ["index"] = [Category.Database],
                ["table"] = [Category.Database],
                ["document"] = [Category.Document],
                ["email"] = [Category.Email],
                ["e mail"] = [Category.Email],
                ["encrypted"] = [Category.Encrypted],
                ["password"] = [Category.Encrypted],
                ["protected"] = [Category.Encrypted],
                ["applet"] = [Category.Executable],
                ["application"] = [Category.Executable],
                ["assembly"] = [Category.Executable],
                ["bitcode"] = [Category.Executable],
                ["bytecode"] = [Category.Executable],
                ["codec"] = [Category.Executable],
                ["compiled"] = [Category.Executable],
                ["driver"] = [Category.Executable],
                ["executable"] = [Category.Executable],
                ["plug in"] = [Category.Executable],
                ["plugin"] = [Category.Executable],
                ["program"] = [Category.Executable],
                ["font"] = [Category.Font],
                ["drawing"] = [Category.Image],
                ["graphic"] = [Category.Image],
                ["icon"] = [Category.Image],
                ["image"] = [Category.Image],
                ["photo"] = [Category.Image],
                ["picture"] = [Category.Image],
                ["skin"] = [Category.Image],
                ["theme"] = [Category.Image],
                ["thumbnail"] = [Category.Image],
                ["little endian"] = [Category.LittleEndian],
                ["log"] = [Category.Log],
                ["lossless"] = [Category.Lossless],
                ["lossy"] = [Category.Lossy],
                ["powerpoint"] = [Category.Presentation],
                ["project"] = [Category.Project],
                ["workspace"] = [Category.Project],
                ["macro"] = [Category.Script],
                ["script"] = [Category.Script],
                ["source"] = [Category.Script],
                ["spreadsheet"] = [Category.Spreadsheet],
                ["workbook"] = [Category.Spreadsheet],
                ["worksheet"] = [Category.Spreadsheet],
                ["preset"] = [Category.Template],
                ["template"] = [Category.Template],
                ["utf8"] = [Category.Utf8],
                ["utf 8"] = [Category.Utf8],
                ["utf16"] = [Category.Utf16],
                ["utf 16"] = [Category.Utf16],
                ["utf32"] = [Category.Utf32],
                ["utf 32"] = [Category.Utf32],
                ["diagram"] = [Category.Vector],
                ["geometry"] = [Category.Vector],
                ["model"] = [Category.Vector],
                ["shape"] = [Category.Vector],
                ["vector"] = [Category.Vector],
                ["voxel"] = [Category.Vector],
                ["animation"] = [Category.Video],
                ["animated"] = [Category.Video],
                ["movie"] = [Category.Video],
                ["multimedia"] = [Category.Video],
                ["sprite"] = [Category.Video],
                ["video"] = [Category.Video],
                ["firmware"] = [Category.Executable, Category.DiskImage],
                ["rom"] = [Category.Executable, Category.DiskImage],
                ["playlist"] = [Category.Audio, Category.Database],
                ["installer"] = [Category.Executable, Category.Archive],
                ["3d"] = [Category.Image, Category.Vector],
                ["2d"] = [Category.Image, Category.Vector],
                ["3 d"] = [Category.Image, Category.Vector],
                ["2 d"] = [Category.Image, Category.Vector],
                ["disk image"] = [Category.DiskImage, Category.Archive],
                ["raster"] = [Category.Bitmap, Category.Image],
                ["bitmap"] = [Category.Bitmap, Category.Image]
            }.ToImmutableDictionary();
        }

        public static string Tokenize(string Text) {
            var sb = new StringBuilder();

            var Space = (char)32;
            var Last = 0;
            foreach (var c in Text.ToLowerInvariant()) {
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

                if (New == ret) {
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

            if (ret.Count == 0) { }

            return [..ret];
        }
    }
}
