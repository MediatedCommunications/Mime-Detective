using MimeDetective.Storage;
using System.Collections.Immutable;

namespace MimeDetective;

public static partial class MinimalDefinitions
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

                    .. WEBP(),

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
                            Extensions = ["bmp",],
                            MimeType = "image/bmp",
                            Categories = [
                                Category.Bitmap,
                                Category.Lossless,
                                Category.Image,
                            ],
                        },
                        Signature = new Rule[]{
                            OffsetRule.Create(0, [0x42, 0x4D,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> GIF() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["gif",],
                            MimeType = "image/gif",
                            Categories = [
                                Category.Image,
                                Category.Compressed,
                                Category.Lossless,
                                Category.Bitmap,
                            ],
                        },
                        Signature = new Rule[]{
                            OffsetRule.Create(0, [0x47, 0x49, 0x46, 0x38,]),
                            OffsetRule.Create(5, [0x61,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> ICO() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["ico",],
                            MimeType = "image/x-icon",
                            Categories = [
                                Category.Lossless,
                                Category.Image,
                                Category.Bitmap,
                            ],
                        },
                        Signature = new Rule[]{
                            OffsetRule.Create(0, [0x00, 0x00, 0x01, 0x00,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> JPEG() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["jpg", "jpeg",],
                            MimeType = "image/jpeg",
                            Categories = [
                                Category.Compressed,
                                Category.Lossy,
                                Category.Image,
                                Category.Bitmap,
                            ],
                        },
                        Signature = new Rule[]{
                            OffsetRule.Create(0, [0xFF, 0xD8, 0xFF,]), 
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> PNG() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["png",],
                            MimeType = "image/png",
                            Categories = [
                                Category.Compressed,
                                Category.Lossless,
                                Category.Image,
                                Category.Bitmap,
                            ],
                        },
                        Signature = new Rule[]{
                            OffsetRule.Create(0, [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> PSD() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["psd",],
                            MimeType = ApplicationOctetStream,
                            Categories = [
                                Category.Vector,
                                Category.Image,
                            ],
                        },
                        Signature = new Rule[]{
                            OffsetRule.Create(0, [0x38, 0x42, 0x50, 0x53,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> TIFF_Be() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["tif", "tiff",],
                            MimeType = "image/tiff",
                            Categories = [
                                Category.Image,
                                Category.Document,
                                Category.BigEndian,
                            ],
                        },
                        Signature = new Rule[]{
                            OffsetRule.Create(0, [0x4D, 0x4D, 0x00, 0x2A,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> TIFF_Big() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["tif", "tiff",],
                            MimeType = "image/tiff",
                            Categories = [
                                Category.Image,
                                Category.Document,
                                Category.BigEndian,
                            ],
                        },
                        Signature = new Rule[]{
                            OffsetRule.Create(0, [0x4D, 0x4D, 0x00, 0x2B,]),
                        }.ToSignature(),
                    },
                ];
            }
        
            public static ImmutableArray<Definition> TIFF_Le() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["tif", "tiff",],
                            MimeType = "image/tiff",
                            Categories = [
                                Category.Image,
                                Category.Document,
                                Category.LittleEndian,
                            ],
                        },
                        Signature = new Rule[]{
                            OffsetRule.Create(0, [0x49, 0x49, 0x2A, 0x00,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> TIFF_NoBom() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["tif", "tiff",],
                            MimeType = "image/tiff",
                            Categories = [
                                Category.Image,
                                Category.Document,
                            ],
                        },
                        Signature = new Rule[]{
                            OffsetRule.Create(0, [0x49, 0x20, 0x49,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> WEBP() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["webp",],
                            MimeType = "image/webp",
                            Categories = [
                                Category.Image,
                                Category.Document,
                                Category.Compressed,
                            ],
                        },
                        Signature = new Rule[]{
                            OffsetRule.Create(0, [0x52, 0x49, 0x46, 0x46,]), //At offset 0 in the file, expect the bytes "RIFF".
                            OffsetRule.Create(8, [0x57, 0x45, 0x42, 0x50,]), //At offset 8 in the file, expect the bytes "WEBP".
                        }.ToSignature(),
                    },
                ];
            }


        }
    }

}
