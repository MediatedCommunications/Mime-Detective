using MimeDetective.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective.Definitions {
    public static partial class Default
    {

        public static partial class FileTypes {
            public static partial class Video {

                public static ImmutableArray<Definition> All() {
                    return new List<Definition>() {
                        FLV(),
                        
                        MOV(),

                        MP4_3GP5(),
                        MP4_Container(),
                        MP4_ISOM(),
                        MP4_MP42(),
                        MP4_MSNV(),

                        ThreeGP(),
                    }.ToImmutableArray();
                }


                public static ImmutableArray<Definition> MP4() {
                    return new List<Definition>() {
                        MP4_3GP5(),
                        MP4_Container(),
                        MP4_ISOM(),
                        MP4_MP42(),
                        MP4_MSNV(),
                    }.ToImmutableArray();
                }


                public static ImmutableArray<Definition> FLV() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"flv"}.ToImmutableArray(),
                                MimeType = "application/unknown",
                                Categories = new[]{
                                    Category.Vector,
                                    Category.Video,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "46 4C 56 01")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();



                }

                public static ImmutableArray<Definition> MOV() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"mov", }.ToImmutableArray(),
                                MimeType = "video/quicktime",
                                Categories = new[]{
                                    Category.Compressed,
                                    Category.Video,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(4, "66 74 79 70 71 74 20 20") 
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();

                }

                public static ImmutableArray<Definition> MP4_3GP5() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"mp4", }.ToImmutableArray(),
                                MimeType = "video/mp4",
                                Categories = new[]{
                                    Category.Compressed,
                                    Category.Video,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(4, "66 74 79 70 33 67 70 35")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();

                }

                public static ImmutableArray<Definition> MP4_Container() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"mp4", "m4v", "m4a", "mp4a", "mov"}.ToImmutableArray(),
                                MimeType = "video/mp4",
                                Categories = new[]{
                                    Category.Compressed,
                                    Category.Video,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(4, "66 74 79 70")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();

                }

                public static ImmutableArray<Definition> MP4_ISOM() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"mp4", }.ToImmutableArray(),
                                MimeType = "video/mp4",
                                Categories = new[]{
                                    Category.Compressed,
                                    Category.Video,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(4, "66 74 79 70 69 73 6F 6D")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> MP4_MP42() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"m4v", }.ToImmutableArray(),
                                MimeType = "video/x-m4v",
                                Categories = new[]{
                                    Category.Compressed,
                                    Category.Video,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(4, "66 74 79 70 6D 70 34 32")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();

                }

                public static ImmutableArray<Definition> MP4_MSNV() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"mp4", }.ToImmutableArray(),
                                MimeType = "video/mp4",
                                Categories = new[]{
                                    Category.Compressed,
                                    Category.Video,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(4, "66 74 79 70 4D 53 4E 56"),
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();

                }

                public static ImmutableArray<Definition> ThreeGP() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"3gp"}.ToImmutableArray(),
                                MimeType = "video/3gp",
                                Categories = new[]{
                                    Category.Compressed,
                                    Category.Video,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(4, "66 74 79 70 33 67 70"),
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();



                }


            }

        }
    }
}
