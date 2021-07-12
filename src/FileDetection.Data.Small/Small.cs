﻿using FileDetection.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDetection.Data
{
    public static class Small
    {
        public static ImmutableArray<Definition> Definitions()
        {
            var raw = FileDetection.Data.Resources.data;
            var ret = FileDetection.Storage.DefinitionBinarySerializer
                .FromBinary(raw)
                .ToImmutableArray()
                ;

            return ret;

        }

    }
}