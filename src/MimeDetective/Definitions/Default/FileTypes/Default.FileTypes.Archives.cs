using MimeDetective.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective.Definitions {
    public static partial class DefaultDefinitions
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
                                Extensions = ["bz2","tar","tbz2","tb2"],
                                MimeType = "application/x-bzip2",
                                Categories = [
                                    Category.Compressed, 
                                    Category.Archive
                                ],
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "42 5A 68")
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> GZ() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["gz", "tgz"],
                                MimeType = "application/x-gz",
                                Categories = [
                                    Category.Compressed,
                                    Category.Archive
                                ],
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "1F 8B 08"),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> RAR() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["rar"],
                                MimeType = ApplicationXCompressed,
                                Categories = [
                                    Category.Compressed,
                                    Category.Archive
                                ],
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "52 61 72 21"),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> SevenZip() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["7z"],
                                MimeType = ApplicationXCompressed,
                                Categories = [
                                    Category.Compressed,
                                    Category.Archive
                                ],
                            },
                            Signature = new Segment[]{ 
                                PrefixSegment.Create(0, "37 7A BC AF 27 1C")
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> TAR_ARK() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["tar"],
                                MimeType = ApplicationXTar,
                                Categories = [
                                    Category.Compressed,
                                    Category.Archive
                                ],
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(257, "75 73 74 61 72")
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> TAR_ZH() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["tar"],
                                MimeType = ApplicationXTar,
                                Categories = [
                                    Category.Compressed,
                                    Category.Archive
                                ],
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "1F A0")
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> TAR_ZV() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["tar"],
                                MimeType = ApplicationXTar,
                                Categories = [
                                    Category.Compressed,
                                    Category.Archive
                                ],
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "1F 9D")
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> ZIP_Archive() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["zip"],
                                MimeType = ApplicationXCompressed,
                                Categories = [
                                    Category.Compressed,
                                    Category.Archive
                                ],
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "50 4B 03 04")
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> ZIP_Empty() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["zip"],
                                MimeType = ApplicationXCompressed,
                                Categories = [
                                    Category.Compressed,
                                    Category.Archive
                                ],
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "50 4B 05 06")
                            }.ToSignature(),
                        },
                    ];
                }

            }
        }
    }
}
