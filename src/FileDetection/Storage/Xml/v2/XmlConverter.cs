using System;
using System.Collections.Immutable;
using System.Linq;
using FileDetection.Storage.Xml.v2;

namespace FileDetection.Storage.Xml.v2
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

        private static Storage.FileType ConvertFile(Definition V1)
        {
            var Extensions = $@"{V1.Info?.FileExtension}"
                .Split(new[] { @"/" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim().ToLower())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToImmutableArray()
                ;

            var ret = new Storage.FileType()
            {
                Description = $@"{V1.Info?.FileType}",
                Extensions = Extensions,
                MimeType = $@"{V1.Info?.MimeType}",
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
                Strings = V1
                    .GlobalStrings
                    .Select(x => ConvertGlobalString(x))
                    .OrderByDescending(x => x.Pattern.Length)
                    .ToImmutableArray()
                    ,

                Prefix = V1
                    .FrontBlock
                    .Select(x => ConvertPattern(x))
                    .OrderBy(x => x.Start)
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
