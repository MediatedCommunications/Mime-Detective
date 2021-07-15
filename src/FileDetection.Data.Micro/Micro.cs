using FileDetection.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FileDetection.Data {
    public static partial class Micro
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
