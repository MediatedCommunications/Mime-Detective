using MimeDetective.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective.Definitions
{
    public static partial class Default
    {

        public static ImmutableArray<Definition> All()
        {
            return new List<Definition>() {
                FileTypes.Archives.All(),
                FileTypes.Audio.All(),
                FileTypes.Cryptographic.All(),
                FileTypes.Documents.All(),
                FileTypes.Email.All(),
                FileTypes.Executables.All(),
                FileTypes.Images.All(),
                FileTypes.Text.All(),
                FileTypes.Video.All(),
                FileTypes.Xml.All(),
            }.ToImmutableArray();
        }
    }
}
