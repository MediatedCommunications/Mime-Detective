using MimeDetective.Storage;
using System.Collections.Immutable;

namespace MimeDetective;

public static partial class MinimalDefinitions
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
                            Extensions = ["dll", "exe",],
                            MimeType = ApplicationOctetStream,
                            Categories = [
                                Category.Executable,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(0, [0x4D, 0x5A,]),
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
                        Signature = new Rule[] {
                            OffsetRule.Create(0, [0x7F, 0x45, 0x4C, 0x46,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> LIB_COFF() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["lib",],
                            MimeType = ApplicationOctetStream,
                            Categories = [
                                Category.Executable,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(0, [0x21, 0x3C, 0x61, 0x72, 0x63, 0x68, 0x3E, 0x0A,]),
                        }.ToSignature(),
                    },
                ];
            }

        }
    }
}
