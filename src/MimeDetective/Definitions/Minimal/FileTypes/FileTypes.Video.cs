using MimeDetective.Storage;
using System.Collections.Immutable;

namespace MimeDetective;

public static partial class MinimalDefinitions
{

    public static partial class FileTypes {
        public static partial class Video {

            public static ImmutableArray<Definition> All() {
                return [
                    .. FLV(),
                    
                    .. MOV(),

                    .. MP4_3GP5(),
                    .. MP4_Container(),
                    .. MP4_ISOM(),
                    .. MP4_MP42(),
                    .. MP4_MSNV(),

                    .. ThreeGP(),
                ];
            }


            public static ImmutableArray<Definition> MP4() {
                return [
                    .. MP4_3GP5(),
                    .. MP4_Container(),
                    .. MP4_ISOM(),
                    .. MP4_MP42(),
                    .. MP4_MSNV(),
                ];
            }


            public static ImmutableArray<Definition> FLV() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["flv",],
                            MimeType = "application/unknown",
                            Categories = [
                                Category.Vector,
                                Category.Video,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(0, [0x46, 0x4C, 0x56, 0x01,]),
                        }.ToSignature(),
                    },
                ];



            }

            public static ImmutableArray<Definition> MOV() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["mov",],
                            MimeType = "video/quicktime",
                            Categories = [
                                Category.Compressed,
                                Category.Video,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(4, [0x66, 0x74, 0x79, 0x70, 0x71, 0x74, 0x20, 0x20,]), 
                        }.ToSignature(),
                    },
                ];

            }

            public static ImmutableArray<Definition> MP4_3GP5() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["mp4",],
                            MimeType = "video/mp4",
                            Categories = [
                                Category.Compressed,
                                Category.Video,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(4, [0x66, 0x74, 0x79, 0x70, 0x33, 0x67, 0x70, 0x35,]),
                        }.ToSignature(),
                    },
                ];

            }

            public static ImmutableArray<Definition> MP4_Container() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["mp4", "m4v", "m4a", "mp4a", "mov",],
                            MimeType = "video/mp4",
                            Categories = [
                                Category.Compressed,
                                Category.Video,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(4, [0x66, 0x74, 0x79, 0x70,]),
                        }.ToSignature(),
                    },
                ];

            }

            public static ImmutableArray<Definition> MP4_ISOM() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["mp4",],
                            MimeType = "video/mp4",
                            Categories = [
                                Category.Compressed,
                                Category.Video,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(4, [0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> MP4_MP42() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["m4v",],
                            MimeType = "video/x-m4v",
                            Categories = [
                                Category.Compressed,
                                Category.Video,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(4, [0x66, 0x74, 0x79, 0x70, 0x6D, 0x70, 0x34, 0x32,]),
                        }.ToSignature(),
                    },
                ];

            }

            public static ImmutableArray<Definition> MP4_MSNV() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["mp4",],
                            MimeType = "video/mp4",
                            Categories = [
                                Category.Compressed,
                                Category.Video,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(4, [0x66, 0x74, 0x79, 0x70, 0x4D, 0x53, 0x4E, 0x56,]),
                        }.ToSignature(),
                    },
                ];

            }

            public static ImmutableArray<Definition> ThreeGP() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["3gp",],
                            MimeType = "video/3gp",
                            Categories = [
                                Category.Compressed,
                                Category.Video,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(4, [0x66, 0x74, 0x79, 0x70, 0x33, 0x67, 0x70,]),
                        }.ToSignature(),
                    },
                ];



            }


        }

    }
}
