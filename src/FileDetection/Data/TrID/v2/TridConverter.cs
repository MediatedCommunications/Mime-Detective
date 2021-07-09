using System;
using System.Collections.Immutable;
using System.Linq;
using FileDetection.Data.Trid.v2;

namespace FileDetection.Data.Trid.v2
{

    /// <summary>
    /// Convert TrID XML definitions into <see cref="Engine.Definition"/>s.
    /// </summary>
    public static class TridConverter
    {

        public static Engine.Definition Convert(Definition V1)
        {
            var ret = new Engine.Definition()
            {
                File = ConvertFile(V1),
                Meta = ConvertMeta(V1),
                Signature = ConvertSignature(V1),
            };

            return ret;
        }

        private static Engine.FileType ConvertFile(Definition V1)
        {
            var Extensions = $@"{V1.Info?.FileExtension}"
                .Split(@"/", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .ToImmutableArray()
                ;

            var ret = new Engine.FileType()
            {
                Description = $@"{V1.Info?.FileType}",
                Extensions = Extensions,
                MimeType = $@"{V1.Info?.MimeType}",
            };

            return ret;
        }

        private static Engine.Meta ConvertMeta(Definition V1)
        {
            var ret = new Engine.Meta()
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
                    By = new Engine.Author()
                    {
                        Name = $@"{V1.Info?.Author}",
                        Email = $@"{V1.Info?.AuthorEmail}",
                        Website = $@"{V1.Info?.Website}",
                    },
                    Source = new Engine.Source()
                    {
                        Files = V1.General?.FileNum ?? 0,
                    },
                    Using = new Engine.Generator()
                    {
                        Application = $@"{V1.General?.Creator}"
                    },
                },
                Reference = new Engine.Reference()
                {
                    Text = $@"{V1.Info?.ExtraInfo?.Remark}",
                    Uri = $@"{V1.Info?.ExtraInfo?.ReferenceUrl}",
                },
            };

            return ret;
        }

        private static Engine.Signature ConvertSignature(Definition V1)
        {
            var ret = new Engine.Signature()
            {
                Strings = V1.GlobalStrings.Select(x => ConvertGlobalString(x)).ToImmutableArray(),
                Prefix = V1.FrontBlock.Select(x => ConvertPattern(x)).ToImmutableArray()
            };

            return ret;
        }

        private static Engine.StringSegment ConvertGlobalString(string V1)
        {
            var Converted = V1.Replace("'", "\0");
            var Bytes = System.Text.Encoding.UTF8.GetBytes(Converted);

            var ret = new Engine.StringSegment()
            {
                Pattern = Bytes.ToImmutableArray(),
            };
            return ret;
        }

        private static Engine.PrefixSegment ConvertPattern(Pattern V1)
        {
            var Position = V1.Position;

            var Bytes = V1.Bytes ?? string.Empty;
            while (Bytes.Length % 2 != 0)
            {
                Bytes = "0" + Bytes;
            }

            var Content = System.Convert.FromHexString(Bytes).ToImmutableArray();

            var ret = new Engine.PrefixSegment()
            {
                Start = Position,
                Pattern = Content,
            };

            return ret;

        }

    }
}
