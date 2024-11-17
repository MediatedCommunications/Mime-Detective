using MimeDetective.Storage;
using System.Collections.Immutable;

namespace MimeDetective.Definitions;

public static partial class Default {
    public static partial class FileTypes {
        public static class Audio {
            public static ImmutableArray<Definition> All() {
                return [
                    .. FLAC(),
                    .. M4A(),
                    .. MIDI(),
                    .. MP3_ID3v1(),
                    .. MP3_ID3v2(),
                    .. OGG(),
                    .. WAV()
                ];
            }

            public static ImmutableArray<Definition> MP3() {
                return [
                    .. MP3_ID3v1(),
                    .. MP3_ID3v2()
                ];
            }


            public static ImmutableArray<Definition> FLAC() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["flac"],
                            MimeType = "audio/x-flac",
                            Categories = [
                                Category.Compressed,
                                Category.Lossless,
                                Category.Audio
                            ]
                        },
                        Signature = SegmentExtensions.ToSignature([
                            PrefixSegment.Create(0, "66 4C 61 43")
                        ])
                    }
                ];
            }

            public static ImmutableArray<Definition> M4A() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["m4a", "mp4a"],
                            MimeType = "audio/mp4",
                            Categories = [
                                Category.Compressed,
                                Category.Audio
                            ]
                        },
                        Signature = SegmentExtensions.ToSignature([
                            PrefixSegment.Create(4, "66 74 79 70 4D 34 41 20")
                        ])
                    }
                ];
            }

            public static ImmutableArray<Definition> MIDI() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["midi", "mid"],
                            MimeType = "audio/midi",
                            Categories = [Category.Audio]
                        },
                        Signature = SegmentExtensions.ToSignature([
                            PrefixSegment.Create(0, "4D 54 68 64")
                        ])
                    }
                ];
            }

            public static ImmutableArray<Definition> MP3_ID3v1() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["mp3"],
                            MimeType = "audio/mpeg3",
                            Categories = [
                                Category.Compressed,
                                Category.Lossy,
                                Category.Audio
                            ]
                        },
                        Signature = SegmentExtensions.ToSignature<Segment>([
                            PrefixSegment.Create(0, "FF"),
                            StringSegment.Create("TAG")
                        ])
                    }
                ];
            }

            public static ImmutableArray<Definition> MP3_ID3v2() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["mp3"],
                            MimeType = "audio/mpeg3",
                            Categories = [
                                Category.Compressed,
                                Category.Lossy,
                                Category.Audio
                            ]
                        },
                        Signature = SegmentExtensions.ToSignature([
                            PrefixSegment.Create(0, "49 44 33")
                        ])
                    }
                ];
            }

            public static ImmutableArray<Definition> OGG() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["ogg", "oga", "ogv", "ogx"],
                            MimeType = "application/ogg",
                            Categories = [
                                Category.Compressed,
                                Category.Lossy,
                                Category.Audio
                            ]
                        },
                        Signature = SegmentExtensions.ToSignature([
                            PrefixSegment.Create(0, "4F 67 67 53")
                        ])
                    }
                ];
            }

            public static ImmutableArray<Definition> WAV() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["wav"],
                            MimeType = "audio/wav",
                            Categories = [
                                Category.Lossless,
                                Category.Audio
                            ]
                        },
                        Signature = SegmentExtensions.ToSignature<Segment>([
                            PrefixSegment.Create(0, "52 49 46 46"),
                            PrefixSegment.Create(8, "57 41 56 45 66 6D 74 20")
                        ])
                    }
                ];
            }
        }
    }
}
