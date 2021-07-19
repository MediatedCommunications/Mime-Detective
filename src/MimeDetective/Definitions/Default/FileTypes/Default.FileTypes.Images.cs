using MimeDetective.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective.Definitions {
    public static partial class Default
    {
        public static partial class FileTypes {

            public static partial class Images {

                public static ImmutableArray<Definition> All() {
                    return new List<Definition>() {
                        BMP(),
                        GIF(),
                        ICO(),
                        JPEG(),
                        PNG(),
                        PSD(),

                        TIFF_Be(),
                        TIFF_Big(),
                        TIFF_Le(),
                        TIFF_NoBom(),

                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TIFF() {
                    return new List<Definition>() {
                        TIFF_Be(),
                        TIFF_Big(),
                        TIFF_Le(),
                        TIFF_NoBom(),
                    }.ToImmutableArray();
                }



                public static ImmutableArray<Definition> BMP() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"bmp"}.ToImmutableArray(),
                                MimeType = "image/bmp",
                                Categories = new[]{
                                    Category.Bitmap,
                                    Category.Lossless,
                                    Category.Image,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "42 4D")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> GIF() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"gif"}.ToImmutableArray(),
                                MimeType = "image/gif",
                                Categories = new[]{
                                    Category.Image,
                                    Category.Compressed,
                                    Category.Lossless,
                                    Category.Bitmap,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "47 49 46 38"),
                                PrefixSegment.Create(5, "61")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> ICO() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"ico"}.ToImmutableArray(),
                                MimeType = "image/x-icon",
                                Categories = new[]{
                                    Category.Lossless,
                                    Category.Image,
                                    Category.Bitmap,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "00 00 01 00")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> JPEG() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"jpg", "jpeg"}.ToImmutableArray(),
                                MimeType = "image/jpeg",
                                Categories = new[]{
                                    Category.Compressed,
                                    Category.Lossy,
                                    Category.Image,
                                    Category.Bitmap,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "FF D8 FF") 
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> PNG() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"png"}.ToImmutableArray(),
                                MimeType = "image/png",
                                Categories = new[]{
                                    Category.Compressed,
                                    Category.Lossless,
                                    Category.Image,
                                    Category.Bitmap,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "89 50 4E 47 0D 0A 1A 0A")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> PSD() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"psd"}.ToImmutableArray(),
                                MimeType = ApplicationOctetStream,
                                Categories = new[]{
                                    Category.Vector,
                                    Category.Image,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "38 42 50 53")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TIFF_Be() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"tif", "tiff"}.ToImmutableArray(),
                                MimeType = "image/tiff",
                                Categories = new[]{
                                    Category.Image,
                                    Category.Document,
                                    Category.BigEndian,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "4D 4D 00 2A")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TIFF_Big() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"tif", "tiff"}.ToImmutableArray(),
                                MimeType = "image/tiff",
                                Categories = new[]{
                                    Category.Image,
                                    Category.Document,
                                    Category.BigEndian,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "4D 4D 00 2B")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }
            
                public static ImmutableArray<Definition> TIFF_Le() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"tif", "tiff"}.ToImmutableArray(),
                                MimeType = "image/tiff",
                                Categories = new[]{
                                    Category.Image,
                                    Category.Document,
                                    Category.LittleEndian,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "49 49 2A 00")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TIFF_NoBom() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"tif", "tiff"}.ToImmutableArray(),
                                MimeType = "image/tiff",
                                Categories = new[]{
                                    Category.Image,
                                    Category.Document,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "49 20 49")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }


            }
        }

    }
}
