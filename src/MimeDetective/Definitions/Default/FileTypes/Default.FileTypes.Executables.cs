using MimeDetective.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective.Definitions {
    public static partial class DefaultDefinitions
    {

        public static partial class FileTypes {
            public static partial class Executables {

                public static ImmutableArray<Definition> All() {
                    return [
                        .. DLL_EXE(),
                        .. ELF(),
                        .. LIB_COFF(),
                    ];
                }

                public static ImmutableArray<Definition> DLL_EXE() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["dll", "exe"],
                                MimeType = ApplicationOctetStream,
                                Categories = [
                                    Category.Executable,
                                ],
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "4D 5A")
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> ELF() {
                    return [
                        new() {
                            File = new() {
                                MimeType = ApplicationOctetStream,
                                Categories = [
                                    Category.Executable,
                                ],
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "7F 45 4C 46")
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> LIB_COFF() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["lib"],
                                MimeType = ApplicationOctetStream,
                                Categories = [
                                    Category.Executable,
                                ],
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "21 3C 61 72 63 68 3E 0A")
                            }.ToSignature(),
                        },
                    ];
                }

            }
        }
    }
}
