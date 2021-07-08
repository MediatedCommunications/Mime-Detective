using FileDetection.Data.Engine;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDetection.Data
{
    public static class Large
    {
        public static ImmutableArray<Definition> Definitions()
        {
            var raw = FileDetection.Data.Resources.data;
            var ret = FileDetection.Data.Engine.DefinitionBinarySerializer
                .FromBinary(raw)
                .ToImmutableArray()
                ;

            return ret;

        }

    }

    internal class LicenseJson
    {
        public string Owner { get; init; } = string.Empty;
        public DateTimeOffset Valid_From { get; init; }
        public DateTimeOffset Valid_Till { get; init; }
    }
}
