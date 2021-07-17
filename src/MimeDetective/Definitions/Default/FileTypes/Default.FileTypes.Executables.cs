using MimeDetective.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective.Definitions {
    public static partial class Default
    {

        public static partial class FileTypes {
            public static partial class Executables {

                public static ImmutableArray<Definition> All() {
                    return new List<Definition>() {
                        DLL_EXE(),
                        ELF(),
                        LIB_COFF(),
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> DLL_EXE() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"dll", "exe"}.ToImmutableArray(),
                                MimeType = ApplicationOctetStream,
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "4D 5A")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> ELF() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                MimeType = ApplicationOctetStream,
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "7F 45 4C 46")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> LIB_COFF() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"lib"}.ToImmutableArray(),
                                MimeType = ApplicationOctetStream,
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "21 3C 61 72 63 68 3E 0A")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

            }
        }
    }
}
