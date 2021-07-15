using FileDetection.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FileDetection.Data {
    public static partial class Micro
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
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                             0x46, 0x4C, 0x56, 0x01
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();



                }

                public static ImmutableArray<Definition> MOV() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"mov", }.ToImmutableArray(),
                                MimeType = "video/quicktime",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Start = 4,
                                        Pattern = new byte[] {
                                            0x66, 0x74, 0x79, 0x70, 0x71, 0x74, 0x20, 0x20
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();

                }

                public static ImmutableArray<Definition> MP4_3GP5() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"mp4", }.ToImmutableArray(),
                                MimeType = "video/mp4",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Start = 4,
                                        Pattern = new byte[] {
                                            0x66, 0x74, 0x79, 0x70, 0x33, 0x67, 0x70, 0x35
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();

                }

                public static ImmutableArray<Definition> MP4_Container() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"mp4", "m4v", "m4a", "mp4a", "mov"}.ToImmutableArray(),
                                MimeType = "video/mp4",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Start = 4,
                                        Pattern = new byte[] {
                                            0x66, 0x74, 0x79, 0x70
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();

                }

                public static ImmutableArray<Definition> MP4_ISOM() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"mp4", }.ToImmutableArray(),
                                MimeType = "video/mp4",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Start = 4,
                                        Pattern = new byte[] {
                                            0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> MP4_MP42() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"m4v", }.ToImmutableArray(),
                                MimeType = "video/x-m4v",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Start = 4,
                                        Pattern = new byte[] {
                                            0x66, 0x74, 0x79, 0x70, 0x6D, 0x70, 0x34, 0x32
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();

                }

                public static ImmutableArray<Definition> MP4_MSNV() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"mp4", }.ToImmutableArray(),
                                MimeType = "video/mp4",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Start = 4,
                                        Pattern = new byte[] {
                                            0x66, 0x74, 0x79, 0x70, 0x4D, 0x53, 0x4E, 0x56
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();

                }

                public static ImmutableArray<Definition> ThreeGP() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"3gp"}.ToImmutableArray(),
                                MimeType = "video/3gp",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                             0x66, 0x74, 0x79, 0x70, 0x33, 0x67, 0x70
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();



                }


            }

        }
    }
}
