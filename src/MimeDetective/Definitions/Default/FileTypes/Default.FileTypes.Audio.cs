using MimeDetective.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective.Definitions {
    public static partial class Default
    {


        public static partial class FileTypes {
            public static partial class Audio {

                public static ImmutableArray<Definition> All() {
                    return new List<Definition>() {
                        FLAC(),
                        M4A(),
                        MIDI(),
                        MP3_ID3v1(),
                        MP3_ID3v2(),
                        OGG(),
                        WAV(),
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> MP3() {
                    return new List<Definition>() {
                        MP3_ID3v1(),
                        MP3_ID3v2(),
                    }.ToImmutableArray();
                }


                public static ImmutableArray<Definition> FLAC() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"flac"}.ToImmutableArray(),
                                MimeType = "audio/x-flac",
                                Categories = new[]{
                                    Category.Compressed,
                                    Category.Lossless,
                                    Category.Audio,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0,"66 4C 61 43")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> M4A() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"m4a", "mp4a"}.ToImmutableArray(),
                                MimeType = "audio/mp4",
                                Categories = new[]{
                                    Category.Compressed,
                                    Category.Audio
                                }.ToImmutableHashSet(),
                        },
                        Signature = new Segment[] {
                            PrefixSegment.Create(4, "66 74 79 70 4D 34 41 20")
                        }.ToSignature(),
                    },
                }.ToImmutableArray();

                }

                public static ImmutableArray<Definition> MIDI() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"midi", "mid"}.ToImmutableArray(),
                                MimeType = "audio/midi",
                                Categories = new[]{
                                    Category.Audio
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "4D 54 68 64")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> MP3_ID3v1() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"mp3"}.ToImmutableArray(),
                                MimeType = "audio/mpeg3",
                                Categories = new[]{
                                    Category.Compressed,
                                    Category.Lossy,
                                    Category.Audio,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "FF"),
                                StringSegment.Create("TAG"),
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> MP3_ID3v2() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"mp3"}.ToImmutableArray(),
                                MimeType = "audio/mpeg3",
                                Categories = new[]{
                                    Category.Compressed,
                                    Category.Lossy,
                                    Category.Audio,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "49 44 33"),
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> OGG() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"ogg", "oga", "ogv", "ogx",}.ToImmutableArray(),
                                MimeType = "application/ogg",
                                Categories = new[]{
                                    Category.Compressed,
                                    Category.Lossy,
                                    Category.Audio,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "4F 67 67 53")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> WAV() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"wav"}.ToImmutableArray(),
                                MimeType = "audio/wav",
                                Categories = new[]{
                                    Category.Lossless,
                                    Category.Audio,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "52 49 46 46"),
                                PrefixSegment.Create(8, "57 41 56 45 66 6D 74 20")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

            }
        }
    }
}
