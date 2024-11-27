using MimeDetective.Storage;
using System.Collections.Immutable;

namespace MimeDetective.Definitions;

public static partial class Default {
    public static partial class FileTypes {
        public static class Video {
            public static ImmutableArray<Definition> All() {
                return [
                    .. FLV(),

                    .. MOV(),

                    .. MP4_3GP5(),
                    .. MP4_Container(),
                    .. MP4_ISOM(),
                    .. MP4_MP42(),
                    .. MP4_MSNV(),

                    .. ThreeGP()
                ];
            }

            public static ImmutableArray<Definition> MP4() {
                return [
                    .. MP4_3GP5(),
                    .. MP4_Container(),
                    ..MP4_ISOM(),
                    .. MP4_MP42(),
                    ..MP4_MSNV()
                ];
            }

            public static ImmutableArray<Definition> FLV() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["flv"],
                            MimeType = "application/unknown",
                            Categories = [
                                Category.Vector,
                                Category.Video
                            ]
                        },
                        Signature = SegmentExtensions.ToSignature([
                            PrefixSegment.Create(0, "46 4C 56 01")
                        ])
                    }
                ];
            }

            public static ImmutableArray<Definition> MOV() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["mov"],
                            MimeType = "video/quicktime",
                            Categories = [
                                Category.Compressed,
                                Category.Video
                            ]
                        },
                        Signature = SegmentExtensions.ToSignature([
                            PrefixSegment.Create(4, "66 74 79 70 71 74 20 20")
                        ])
                    }
                ];
            }

            public static ImmutableArray<Definition> MP4_3GP5() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["mp4"],
                            MimeType = "video/mp4",
                            Categories = [
                                Category.Compressed,
                                Category.Video
                            ]
                        },
                        Signature = SegmentExtensions.ToSignature([
                            PrefixSegment.Create(4, "66 74 79 70 33 67 70 35")
                        ])
                    }
                ];
            }

            public static ImmutableArray<Definition> MP4_Container() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["mp4", "m4v", "m4a", "mp4a", "mov"],
                            MimeType = "video/mp4",
                            Categories = [
                                Category.Compressed,
                                Category.Video
                            ]
                        },
                        Signature = SegmentExtensions.ToSignature([
                            PrefixSegment.Create(4, "66 74 79 70")
                        ])
                    }
                ];
            }

            public static ImmutableArray<Definition> MP4_ISOM() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["mp4"],
                            MimeType = "video/mp4",
                            Categories = [
                                Category.Compressed,
                                Category.Video
                            ]
                        },
                        Signature = SegmentExtensions.ToSignature([
                            PrefixSegment.Create(4, "66 74 79 70 69 73 6F 6D")
                        ])
                    }
                ];
            }

            public static ImmutableArray<Definition> MP4_MP42() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["m4v"],
                            MimeType = "video/x-m4v",
                            Categories = [
                                Category.Compressed,
                                Category.Video
                            ]
                        },
                        Signature = SegmentExtensions.ToSignature([
                            PrefixSegment.Create(4, "66 74 79 70 6D 70 34 32")
                        ])
                    }
                ];
            }

            public static ImmutableArray<Definition> MP4_MSNV() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["mp4"],
                            MimeType = "video/mp4",
                            Categories = [
                                Category.Compressed,
                                Category.Video
                            ]
                        },
                        Signature = SegmentExtensions.ToSignature([
                            PrefixSegment.Create(4, "66 74 79 70 4D 53 4E 56")
                        ])
                    }
                ];
            }

            public static ImmutableArray<Definition> ThreeGP() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["3gp"],
                            MimeType = "video/3gp",
                            Categories = [
                                Category.Compressed,
                                Category.Video
                            ]
                        },
                        Signature = SegmentExtensions.ToSignature([
                            PrefixSegment.Create(4, "66 74 79 70 33 67 70")
                        ])
                    }
                ];
            }
        }
    }
}
