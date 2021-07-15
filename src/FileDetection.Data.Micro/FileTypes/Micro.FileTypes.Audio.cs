using FileDetection.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FileDetection.Data {
    public static partial class Micro
    {


        public static partial class FileTypes {
            public static partial class Audio {

                public static ImmutableArray<Definition> All() {
                    return new List<Definition>() {
                        FLAC(),
                        M4A(),
                        MIDI(),
                        MP3(),
                        OGG(),
                        WAV(),
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> FLAC() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"flac"}.ToImmutableArray(),
                                MimeType = "audio/x-flac",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x66, 0x4C, 0x61, 0x43
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> M4A() {
                    return new List<Definition>() {
                    new() {
                        File = new() {
                            Extensions = new[]{"m4a", "mp4a"}.ToImmutableArray(),
                            MimeType = "audio/mp4",
                        },
                        Signature = new() {
                            Prefix = new[] {
                                new PrefixSegment() {
                                    Start = 4,
                                    Pattern = new byte[] {
                                        0x66, 0x74, 0x79, 0x70, 0x4D, 0x34, 0x41, 0x20
                                    }.ToImmutableArray()
                                }
                            }.ToImmutableArray()
                        },
                    },
                }.ToImmutableArray();

                }

                public static ImmutableArray<Definition> MIDI() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"midi", "mid"}.ToImmutableArray(),
                                MimeType = "audio/midi",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x4D, 0x54, 0x68, 0x64
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> MP3() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"mp3"}.ToImmutableArray(),
                                MimeType = "audio/mpeg",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x49, 0x44, 0x33
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }
                
                public static ImmutableArray<Definition> OGG() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"ogg", "oga", "ogv", "ogx",}.ToImmutableArray(),
                                MimeType = "application/ogg",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x4F, 0x67, 0x67, 0x53
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> WAV() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"wav"}.ToImmutableArray(),
                                MimeType = "audio/wav",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Start = 0,
                                        Pattern = new byte[] {
                                             0x52, 0x49, 0x46, 0x46,
                                        }.ToImmutableArray()
                                    },
                                    new PrefixSegment() {
                                        Start = 8,
                                        Pattern = new byte[] {
                                              0x57, 0x41, 0x56, 0x45, 0x66, 0x6D, 0x74, 0x20
                                        }.ToImmutableArray()
                                    },
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

            }
        }
    }
}
