using MimeDetective.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective.Definitions {
    public static partial class Default
    {

        public static partial class FileTypes {
            public static partial class Archives {

                public static ImmutableArray<Definition> All() {
                    return new List<Definition>() {

                        BZ2(),
                        GZ(),
                        RAR(),
                        SevenZip(),
                        TAR_ARK(),
                        TAR_ZH(),
                        TAR_ZV(),
                        
                        ZIP_Archive(),
                        ZIP_Empty(),
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TAR() {
                    return new List<Definition>() {
                        TAR_ARK(),
                        TAR_ZH(),
                        TAR_ZV(),
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> ZIP() {
                    return new List<Definition>() {
                        ZIP_Archive(),
                        ZIP_Empty(),
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> BZ2() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"bz2","tar","tbz2","tb2"}.ToImmutableArray(),
                                MimeType = "application/x-bzip2",
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "42 5A 68")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> GZ() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"gz", "tgz"}.ToImmutableArray(),
                                MimeType = "application/x-gz",
                            },
                            Signature = new Segment[]{
                                            PrefixSegment.Create(0, "1F 8B 08"),
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> RAR() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"rar"}.ToImmutableArray(),
                                MimeType = ApplicationXCompressed,
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "52 61 72 21"),
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> SevenZip() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"7z"}.ToImmutableArray(),
                                MimeType = ApplicationXCompressed,
                            },
                            Signature = new Segment[]{ 
                                PrefixSegment.Create(0, "37 7A BC AF 27 1C")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TAR_ARK() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"tar"}.ToImmutableArray(),
                                MimeType = ApplicationXTar,
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(257, "75 73 74 61 72")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TAR_ZH() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"tar"}.ToImmutableArray(),
                                MimeType = ApplicationXTar,
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "1F A0")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TAR_ZV() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"tar"}.ToImmutableArray(),
                                MimeType = ApplicationXTar,
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "1F 9D")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> ZIP_Archive() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"zip"}.ToImmutableArray(),
                                MimeType = ApplicationXCompressed,
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "50 4B 03 04")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> ZIP_Empty() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"zip"}.ToImmutableArray(),
                                MimeType = ApplicationXCompressed,
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "50 4B 05 06")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                

                

            }
        }
    }
}
