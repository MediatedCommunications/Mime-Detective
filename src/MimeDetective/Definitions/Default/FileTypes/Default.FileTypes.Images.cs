using MimeDetective.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective.Definitions {
    public static partial class DefaultDefinitions
    {
        public static partial class FileTypes {

            public static partial class Images {

                public static ImmutableArray<Definition> All() {
                    return [
                        .. BMP(),
                        .. GIF(),
                        .. ICO(),
                        .. JPEG(),
                        .. PNG(),
                        .. PSD(),

                        .. TIFF_Be(),
                        .. TIFF_Big(),
                        .. TIFF_Le(),
                        .. TIFF_NoBom(),

                        .. WEBP()

                    ];
                }

                public static ImmutableArray<Definition> TIFF() {
                    return [
                        .. TIFF_Be(),
                        .. TIFF_Big(),
                        .. TIFF_Le(),
                        .. TIFF_NoBom(),
                    ];
                }



                public static ImmutableArray<Definition> BMP() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["bmp"],
                                MimeType = "image/bmp",
                                Categories = [
                                    Category.Bitmap,
                                    Category.Lossless,
                                    Category.Image,
                                ],
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "42 4D")
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> GIF() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["gif"],
                                MimeType = "image/gif",
                                Categories = [
                                    Category.Image,
                                    Category.Compressed,
                                    Category.Lossless,
                                    Category.Bitmap,
                                ],
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "47 49 46 38"),
                                PrefixSegment.Create(5, "61")
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> ICO() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["ico"],
                                MimeType = "image/x-icon",
                                Categories = [
                                    Category.Lossless,
                                    Category.Image,
                                    Category.Bitmap,
                                ],
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "00 00 01 00")
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> JPEG() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["jpg", "jpeg"],
                                MimeType = "image/jpeg",
                                Categories = [
                                    Category.Compressed,
                                    Category.Lossy,
                                    Category.Image,
                                    Category.Bitmap,
                                ],
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "FF D8 FF") 
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> PNG() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["png"],
                                MimeType = "image/png",
                                Categories = [
                                    Category.Compressed,
                                    Category.Lossless,
                                    Category.Image,
                                    Category.Bitmap,
                                ],
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "89 50 4E 47 0D 0A 1A 0A")
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> PSD() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["psd"],
                                MimeType = ApplicationOctetStream,
                                Categories = [
                                    Category.Vector,
                                    Category.Image,
                                ],
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "38 42 50 53")
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> TIFF_Be() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["tif", "tiff"],
                                MimeType = "image/tiff",
                                Categories = [
                                    Category.Image,
                                    Category.Document,
                                    Category.BigEndian,
                                ],
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "4D 4D 00 2A")
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> TIFF_Big() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["tif", "tiff"],
                                MimeType = "image/tiff",
                                Categories = [
                                    Category.Image,
                                    Category.Document,
                                    Category.BigEndian,
                                ],
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "4D 4D 00 2B")
                            }.ToSignature(),
                        },
                    ];
                }
            
                public static ImmutableArray<Definition> TIFF_Le() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["tif", "tiff"],
                                MimeType = "image/tiff",
                                Categories = [
                                    Category.Image,
                                    Category.Document,
                                    Category.LittleEndian,
                                ],
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "49 49 2A 00")
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> TIFF_NoBom() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["tif", "tiff"],
                                MimeType = "image/tiff",
                                Categories = [
                                    Category.Image,
                                    Category.Document,
                                ],
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "49 20 49")
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> WEBP() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["webp"],
                                MimeType = "image/webp",
                                Categories = [
                                    Category.Image,
                                    Category.Document,
                                    Category.Compressed,
                                ],
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "52 49 46 46"), //At offset 0 in the file, expect the bytes "RIFF".
                                PrefixSegment.Create(8, "57 45 42 50") //At offset 8 in the file, expect the bytes "WEBP".
                            }.ToSignature(),
                        },
                    ];
                }


            }
        }

    }
}
