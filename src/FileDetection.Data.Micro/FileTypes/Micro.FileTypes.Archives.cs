using FileDetection.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FileDetection.Data {
    public static partial class Micro
    {

        public static partial class FileTypes {
            public static partial class Archives {

                public static ImmutableArray<Definition> All() {
                    return new List<Definition>() {

                        BZ2(),
                        GZ(),
                        RAR(),
                        SevenZip(),
                        TAR_ZH(),
                        TAR_ZV(),
                        ZIP_Archive(),
                        ZIP_Empty(),
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TAR() {
                    return new List<Definition>() {
                        TAR_ZH(),
                        TAR_ZV(),
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> ZIP() {
                    return new List<Definition>() {
                        ZIP_Archive(),
                        ZIP_Empty(),
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> BZ2() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"bz2","tar","tbz2","tb2"}.ToImmutableArray(),
                                MimeType = "application/x-bzip2",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x42, 0x5A, 0x68
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> GZ() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"gz", "tgz"}.ToImmutableArray(),
                                MimeType = "application/x-gz",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x1F, 0x8B, 0x08
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> RAR() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"rar"}.ToImmutableArray(),
                                MimeType = "application/x-compressed",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x52, 0x61, 0x72, 0x21
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> SevenZip() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"7z"}.ToImmutableArray(),
                                MimeType = "application/x-compressed",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TAR_ZH() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"tar.z"}.ToImmutableArray(),
                                MimeType = "application/x-tar",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x1F, 0xA0
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TAR_ZV() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"tar.z"}.ToImmutableArray(),
                                MimeType = "application/x-tar",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x1F, 0x9D
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> ZIP_Archive() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"zip"}.ToImmutableArray(),
                                MimeType = "application/x-compressed",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                             0x50, 0x4B, 0x03, 0x04
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> ZIP_Empty() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"zip"}.ToImmutableArray(),
                                MimeType = "application/x-compressed",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x50, 0x4B, 0x05, 0x06
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
