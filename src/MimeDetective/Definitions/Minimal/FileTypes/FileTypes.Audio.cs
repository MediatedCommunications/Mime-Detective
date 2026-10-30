using MimeDetective.Storage;
using System.Collections.Immutable;

namespace MimeDetective;

public static partial class MinimalDefinitions
{


    public static partial class FileTypes {
        public static partial class Audio {

            public static ImmutableArray<Definition> All() {
                return [
                    .. FLAC(),
                    .. M4A(),
                    .. MIDI(),
                    .. MP3_ID3v1(),
                    .. MP3_ID3v2(),
                    .. OGG(),
                    .. WAV(),
                ];
            }

            public static ImmutableArray<Definition> MP3() {
                return [
                    .. MP3_ID3v1(),
                    .. MP3_ID3v2(),
                ];
            }


            public static ImmutableArray<Definition> FLAC() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["flac",],
                            MimeType = "audio/x-flac",
                            Categories = [
                                Category.Compressed,
                                Category.Lossless,
                                Category.Audio,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(0, [0x66, 0x4C, 0x61, 0x43,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> M4A() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["m4a", "mp4a",],
                            MimeType = "audio/mp4",
                            Categories = [
                                Category.Compressed,
                                Category.Audio,
                            ],
                    },
                    Signature = new Rule[] {
                        OffsetRule.Create(4, [0x66, 0x74, 0x79, 0x70, 0x4D, 0x34, 0x41, 0x20,]),
                    }.ToSignature(),
                },
            ];

            }

            public static ImmutableArray<Definition> MIDI() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["midi", "mid",],
                            MimeType = "audio/midi",
                            Categories = [
                                Category.Audio,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(0, [0x4D, 0x54, 0x68, 0x64,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> MP3_ID3v1() {

                ImmutableArray<string> Extensions = ["mp3",];
                var MimeType = "audio/mpeg";
                ImmutableHashSet<Category> Categories = [
                    Category.Compressed,
                    Category.Lossy,
                    Category.Audio,
                ];

                return [
                    new() {
                        File = new() {
                            Extensions = Extensions,
                            MimeType = MimeType,
                            Categories = Categories,
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(0, [0xFF, 0xFB,]),
                            WildCardRule.Create("TAG"),
                        }.ToSignature(),
                    },
                    new() {
                        File = new() {
                            Extensions = Extensions,
                            MimeType = MimeType,
                            Categories = Categories,
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(0, [0xFF, 0xF2,]),
                            WildCardRule.Create("TAG"),
                        }.ToSignature(),
                    },
                    new() {
                        File = new() {
                            Extensions = Extensions,
                            MimeType = MimeType,
                            Categories = Categories,
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(0, [0xFF, 0xF3,]),
                            WildCardRule.Create("TAG"),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> MP3_ID3v2() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["mp3",],
                            MimeType = "audio/mpeg",
                            Categories = [
                                Category.Compressed,
                                Category.Lossy,
                                Category.Audio,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(0, [0x49, 0x44, 0x33,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> OGG() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["ogg", "oga", "ogv", "ogx",],
                            MimeType = "application/ogg",
                            Categories = [
                                Category.Compressed,
                                Category.Lossy,
                                Category.Audio,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(0, [0x4F, 0x67, 0x67, 0x53,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> WAV() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["wav",],
                            MimeType = "audio/wav",
                            Categories = [
                                Category.Lossless,
                                Category.Audio,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(0, [0x52, 0x49, 0x46, 0x46,]),
                            OffsetRule.Create(8, [0x57, 0x41, 0x56, 0x45, 0x66, 0x6D, 0x74, 0x20,]),
                        }.ToSignature(),
                    },
                ];
            }

        }
    }
}
