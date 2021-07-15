using FileDetection.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FileDetection.Data {
    public static partial class Micro
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
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x4D, 0x5A
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> ELF() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                MimeType = ApplicationOctetStream,
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x7F, 0x45, 0x4C, 0x46
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
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
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x21, 0x3C, 0x61, 0x72, 0x63, 0x68, 0x3E, 0x0A
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

            }
        }
    }
}
