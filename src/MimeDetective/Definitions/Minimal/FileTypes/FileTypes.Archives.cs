using MimeDetective.Storage;
using System.Collections.Immutable;

namespace MimeDetective;

public static partial class MinimalDefinitions
{

    public static partial class FileTypes {
        public static partial class Archives {

            public static ImmutableArray<Definition> All() {
                return [

                    .. BZ2(),
                    .. GZ(),
                    .. RAR(),
                    .. SevenZip(),
                    .. TAR_ARK(),
                    .. TAR_ZH(),
                    .. TAR_ZV(),
                    
                    .. ZIP_Archive(),
                    .. ZIP_Empty(),
                ];
            }

            public static ImmutableArray<Definition> TAR() {
                return [
                    .. TAR_ARK(),
                    .. TAR_ZH(),
                    .. TAR_ZV(),
                ];
            }

            public static ImmutableArray<Definition> ZIP() {
                return [
                    .. ZIP_Archive(),
                    .. ZIP_Empty(),
                ];
            }

            public static ImmutableArray<Definition> BZ2() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["bz2","tar","tbz2","tb2",],
                            MimeType = "application/x-bzip2",
                            Categories = [
                                Category.Compressed, 
                                Category.Archive,
                            ],
                        },
                        Signature = new Rule[]{
                            OffsetRule.Create(0, [0x42, 0x5A, 0x68,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> GZ() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["gz", "tgz",],
                            MimeType = "application/x-gz",
                            Categories = [
                                Category.Compressed,
                                Category.Archive,
                            ],
                        },
                        Signature = new Rule[]{
                            OffsetRule.Create(0, [0x1F, 0x8B, 0x08,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> RAR() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["rar",],
                            MimeType = ApplicationXCompressed,
                            Categories = [
                                Category.Compressed,
                                Category.Archive,
                            ],
                        },
                        Signature = new Rule[]{
                            OffsetRule.Create(0, [0x52, 0x61, 0x72, 0x21,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> SevenZip() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["7z",],
                            MimeType = ApplicationXCompressed,
                            Categories = [
                                Category.Compressed,
                                Category.Archive,
                            ],
                        },
                        Signature = new Rule[]{ 
                            OffsetRule.Create(0, [0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> TAR_ARK() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["tar",],
                            MimeType = ApplicationXTar,
                            Categories = [
                                Category.Compressed,
                                Category.Archive,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(257, [0x75, 0x73, 0x74, 0x61, 0x72,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> TAR_ZH() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["tar",],
                            MimeType = ApplicationXTar,
                            Categories = [
                                Category.Compressed,
                                Category.Archive,
                            ],
                        },
                        Signature = new Rule[]{
                            OffsetRule.Create(0, [0x1F, 0xA0,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> TAR_ZV() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["tar",],
                            MimeType = ApplicationXTar,
                            Categories = [
                                Category.Compressed,
                                Category.Archive,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(0, [0x1F, 0x9D,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> ZIP_Archive() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["zip",],
                            MimeType = ApplicationXCompressed,
                            Categories = [
                                Category.Compressed,
                                Category.Archive,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(0, [0x50, 0x4B, 0x03, 0x04,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> ZIP_Empty() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["zip",],
                            MimeType = ApplicationXCompressed,
                            Categories = [
                                Category.Compressed,
                                Category.Archive,
                            ],
                        },
                        Signature = new Rule[]{
                            OffsetRule.Create(0, [0x50, 0x4B, 0x05, 0x06,]),
                        }.ToSignature(),
                    },
                ];
            }

        }
    }
}
